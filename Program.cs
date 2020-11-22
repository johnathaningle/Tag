using System;
using System.IO;
using CommandLine;

namespace Tag
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed<Options>(o =>
                   {
                       //if the user does not specify a directory, use the one that the application is called from
                       if (string.IsNullOrEmpty(o.WorkingDirectory))
                       {
                           o.WorkingDirectory = Directory.GetCurrentDirectory();
                           Console.WriteLine("Searching using the default path:");
                       }
                       else
                       {
                           Console.WriteLine("Searching using the custom directory specified:");
                       }

                       Console.WriteLine(o.WorkingDirectory);

                       var tagger = new Tagger(o.WorkingDirectory);

                       //the user chose to remove duplicate media
                       if (o.Deduplicate)
                       {
                           tagger.FindDuplicateMedia();
                       }
                       //if the user is tagging the media instead of searching
                       else if(string.IsNullOrEmpty(o.SearchQuery))
                       {
                            Console.CancelKeyPress += (object sender, ConsoleCancelEventArgs e) => tagger.SaveChanges();
                            tagger.Tag(o.ReTag ?? false);
                            tagger.SaveChanges();
                       }
                       //the user chose to search for media
                       else
                       {
                           tagger.Search(o.SearchQuery);
                       }
                   })
                   .WithNotParsed(x => {
                       foreach(var err in x)
                       {
                           Console.WriteLine(err.ToString());
                       }
                   });
        }
    }
}
