using CommandLine;

namespace Tag
{
    public class Options
    {
        [Option('d', "directory", Required = false, HelpText = "The base directory to search for files")]
        public string WorkingDirectory { get; set; }

        [Option('r', "retag", Required = false, HelpText = "Retagging does not skip files that currently have tags")]
        public bool? ReTag {get; set;}
    }
}