using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Tag.Common.Services
{
    public class DataService
    {
        private ILogger<DataService> logger { get; set; }
        public DataService()
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();
            logger = serviceProvider.GetRequiredService<ILogger<DataService>>();
        }
        public bool HasDiskSpace(long totalBytes)
        {
             var dir = AppDomain.CurrentDomain.BaseDirectory;
             var allDrives = DriveInfo.GetDrives();
             var drive = Array.Find(allDrives, x => dir.Contains(x.Name));

             if(drive == null)
             {
                 logger.LogInformation("There was a problem getting the drive information");
                 return false;
             }

             return drive.TotalFreeSpace > totalBytes;

        }
    }
}