using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Tag
{
    internal enum DeduplicationOptions : byte
    {
        Keep_First_Duplicate_File = 0,
        Manaully_Review_Each_Duplicate = 1,
    }
    public class Tagger
    {
        private string workingDirectory { get; }
        private string tagIgnoreFile => workingDirectory + "/.tagignore";
        private string workspaceDirectory => workingDirectory + "/.tags/";
        private string tagFile => workspaceDirectory + "tags.json";
        private string archiveFolder => workingDirectory + "/Archive/";

        /// <summary>
        /// The string key represents the file name and the file info is the tag data
        /// </summary>
        /// <value></value>
        private Dictionary<string, FileInfo> tagData { get; set; }
        private List<FileInfo> duplicateFiles { get; set; }
        private List<string> IgnoredDirectories { get; set; }
        public Tagger(string workingDirectory)
        {
            this.workingDirectory = workingDirectory.Trim();
            this.tagData = new Dictionary<string, FileInfo>();
            this.duplicateFiles = new List<FileInfo>();
            this.IgnoredDirectories = new List<string> { workspaceDirectory };
            Initialize();
        }
        private static string[] imageFileTypes => new string[] {
            "png",
            "jpg",
            "jpeg",
            "gif",
        };

        private static string[] videoFileTypes => new string[] {
            "avi",
            "mp4",
            "webm",
            "mov",
        };

        /// <summary>
        /// Save the tagging data to the workspace directory
        /// </summary>
        public void SaveChanges() 
        {
            File.WriteAllText(this.tagFile, JsonConvert.SerializeObject(this.tagData));
        }

        /// <summary>
        /// Return true if the file name represents a media file
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        private bool IsValidMediaFile(FileInfo fileInfo)
        {
            foreach(var videoExtention in videoFileTypes)
            {
                if(fileInfo.FileName.Contains(videoExtention, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            foreach(var photoExtention in imageFileTypes)
            {
                if(fileInfo.FileName.Contains(photoExtention, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Initialize the tagging data directories
        /// </summary>
        private void Initialize()
        {
            //if the .tagignore file does not exist
            //if the .tags directory doesn't exist, create it, it will store the json file containing the tagging info
            if (!Directory.Exists(workspaceDirectory))
                Directory.CreateDirectory(workspaceDirectory);

            if(!File.Exists(tagFile))
            {
                File.WriteAllText(tagFile, JsonConvert.SerializeObject(tagData));
            }
            else
            {
                var existingTagData = File.ReadAllText(tagFile);
                tagData = JsonConvert.DeserializeObject<Dictionary<string, FileInfo>>(existingTagData);
            }

            //create the tagignore file if it doesn't exist already
            if(!File.Exists(tagIgnoreFile))
            {
                File.WriteAllText(tagIgnoreFile, string.Join("\n", this.IgnoredDirectories));
            }
            //if the tagignore file exists, read in all the ignored directories
            else
            {
                this.IgnoredDirectories = File.ReadAllLines(tagIgnoreFile).ToList();
            }
        }

        /// <summary>
        /// This will list all files that meet a certain search query
        /// example: "exterior, doorway" lists all media tagged with "exterior" or "doorway"
        /// </summary>
        /// <param name="searchQuery"></param>
        public void Search(string searchQuery)
        {
            var tags = searchQuery.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            foreach(var media in this.tagData.Select(x => x.Value))
            {
                if(tags.Any(x => media.Tags.Any(y => y.Contains(x, StringComparison.InvariantCultureIgnoreCase))))
                    Console.WriteLine($"Match: \"{media.FilePath}\"");
            }
        }

        /// <summary>
        /// Run through all the files in the media directory and propt the user to tag the footage
        /// </summary>
        /// <param name="isRetagging"></param>
        public void Tag(bool isRetagging)
        {
            var directoryInfo = new DirectoryInfo(this.workingDirectory);

            Console.WriteLine("Getting file information from the specified directory...");

            var fileInformation = getFileInformation(directoryInfo);
            var taggedFileNames = tagData
                .Where(x => x.Value.Tags.Count > 0)
                .Select(x => x.Value.FileName)
                .ToList();

            var toTag = isRetagging ? fileInformation : fileInformation.Where(x => !taggedFileNames.Contains(x.FileName)).ToList();

            foreach(var info in toTag.Where(x => IsValidMediaFile(x)))
            {
                Task.Run(() => OpenMedia(info.FilePath));
                Console.WriteLine($"Enter the tags (comma separated) for the file: {info.FileName}");
                var tagInfo = Console.ReadLine();
                var tags = tagInfo.Split(',').ToList();
                info.Tags = tags;
                if (!tagData.TryAdd(info.FileName, info))
                    Console.WriteLine("The tag data could not be added...");
            }
        }

        /// <summary>
        /// Attempt to open the media file using the default operating system program
        /// </summary>
        /// <param name="filePath"></param>
        private void OpenMedia(string filePath)
        {
            try
            {
                var startInfo = new ProcessStartInfo(filePath);
                var p = new Process();
                p.StartInfo = startInfo;
                p.StartInfo.UseShellExecute = true;
                p.Start();
            }
            catch
            {
                Console.WriteLine($"The media could not be opened. Make sure \"{filePath}\" is a supported media type");
            }
        }

        public void FindDuplicateMedia()
        {
            var directoryInfo = new DirectoryInfo(this.workingDirectory);

            //get media info
            var fileInformation = getFileInformation(directoryInfo).Where(x => IsValidMediaFile(x));

            //group the file by name
            var groups = fileInformation
                .GroupBy(x => x.FileName)
                .ToDictionary(x => x.Key, x => x.ToList());

            Console.WriteLine("Choose a deduplication option:");
            Enum.GetNames(typeof(DeduplicationOptions))
                .Enumerate()
                .ToList()
                .ForEach(option => {
                    (var idx, var o) = option;
                    var name = o.Replace("_", " ");
                    Console.WriteLine($"[{idx}] {name}");
                });
            var dedupInput = Console.ReadLine();
            var dedupSelection = DeduplicationOptions.Manaully_Review_Each_Duplicate;

            if(byte.TryParse(dedupInput, out var dedupValue))
                dedupSelection = (DeduplicationOptions)dedupValue;

            //process potential duplicates
            foreach(var group in groups.Where(x => x.Value.Count > 1))
            {
                var processedFiles = new List<FileInfo>();
                foreach(var file in group.Value)
                {
                    //skip files in the group that have been deduped
                    if(!processedFiles.Any(x => x.Equals(file) && x.FilePath != file.FilePath))
                    {
                        var duplicateFiles = group.Value
                            .Where(x => x.Equals(file))
                            .ToList();

                        //if there are duplicates, prompt to delete and add them to processed files list
                        if(duplicateFiles.Count > 0)
                        {
                            if(dedupSelection == DeduplicationOptions.Manaully_Review_Each_Duplicate)
                            {
                                PromptFileCleanup(duplicateFiles);
                            }
                            else
                            {
                                //grab all files except the first one in the list
                                var toArchive = duplicateFiles.Enumerate()
                                    .Where(x => x.Item1 > 0)
                                    .Select(x => x.Item2)
                                    .ToList();

                                //move the files the archive directory
                                ArchiveFiles(toArchive);
                            }
                            processedFiles.AddRange(duplicateFiles);
                        }
                    }
                }
            }
        }

        public void ArchiveFiles(List<FileInfo> files)
        {
            if(!Directory.Exists(this.archiveFolder))
                Directory.CreateDirectory(this.archiveFolder);

            foreach(var file in files)
            {
                var toPath = this.archiveFolder + "/" + file.FileName;
                Console.WriteLine($"{file.FileName} is being archived to {toPath}");
                File.Move(file.FilePath, toPath);
            }
        }

        private void PromptFileCleanup(List<FileInfo> duplicateFiles)
        {
            Console.WriteLine("Which of the following files would you like to keep?");
            foreach((var idx, var item) in duplicateFiles.Enumerate())
            {
                Console.WriteLine($"[{idx}] - {item.FilePath}");
            }
            Console.WriteLine("Enter your input in a comma separated list:");
            var input = Console.ReadLine();
            input = input.Trim();

        }

        private List<FileInfo> getFileInformation(DirectoryInfo directoryInfo)
        {
            var fileInfo = new List<FileInfo>();
            foreach(var file in directoryInfo.GetFiles())
            {
                var fi = new FileInfo();
                fi.FileName = file.Name;
                fi.FilePath = file.FullName;
                fi.FileSize = file.Length;
                fileInfo.Add(fi);
            }
            var ignoredPaths = IgnoredDirectories
                .ConvertAll(x => Path.TrimEndingDirectorySeparator(Path.GetFullPath(x)));

            foreach(var directory in directoryInfo.GetDirectories())
            {
                var pathName = Path.TrimEndingDirectorySeparator(Path.GetFullPath(directory.FullName));
                if(!ignoredPaths.Any(x => x.Equals(pathName)))
                {
                    fileInfo.AddRange(getFileInformation(directory));
                }
            }

            return fileInfo;
        }
    }
}