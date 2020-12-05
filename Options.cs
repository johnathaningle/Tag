using CommandLine;

namespace Tag
{
    public class Options
    {
        [Option('d', "directory", Required = false, HelpText = "The base directory to search for files")]
        public string WorkingDirectory { get; set; }

        [Option('r', "retag", Default = false, HelpText = "Retagging does not skip files that currently have tags")]
        public bool ReTag {get; set;}

        [Option('s', "search", Required = false, HelpText = "Search a workspace for certain tags")]
        public string SearchQuery { get; set; }

        [Option('D', "dedup", Required = false, HelpText = "Find and remove duplicate files")]
        public bool Deduplicate { get; set; }

        [Option(Default = false, HelpText = "Only process video files")]
        public bool VideoOnly { get; set; }
    }
}