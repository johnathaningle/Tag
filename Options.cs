using CommandLine;

namespace Tag
{
    public class Options
    {
        [Option('d', "directory", Required = false, HelpText = "The base directory to search for files")]
        public string WorkingDirectory { get; set; }

        [Option('r', "retag", Required = false, HelpText = "Retagging does not skip files that currently have tags")]
        public bool? ReTag {get; set;}

        [Option('s', "search", Required = false, HelpText = "Search a workspace for certain tags")]
        public string SearchQuery { get; set; }
    }
}