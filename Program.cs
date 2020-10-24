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
                       Console.CancelKeyPress += (object sender, ConsoleCancelEventArgs e) => tagger.SaveChanges();

                       tagger.Tag(o.ReTag ?? false);
                       tagger.SaveChanges();
                   });
        }
    }
}
