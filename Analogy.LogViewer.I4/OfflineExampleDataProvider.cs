using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Analogy.Interfaces;
using Analogy.LogViewer.Augmedics.Managers;

namespace Analogy.LogViewer.Augmedics
{
    class OfflineDataProvider : IAnalogyOfflineDataProvider
    {
        public Guid ID { get; } = new Guid("73132E1B-4B37-4746-9620-8C761AAA40CE");
        public string OptionalTitle { get; } = "Augmedics Offline log";

        public bool CanSaveToLogFile { get; } = false;
        public string FileOpenDialogFilters { get; } = "All supported Analogy log file types|*.log;*.txt|Plain log file (*.log)|*.log|Plain log file (*.txt)|*.txt";
        public string FileSaveDialogFilters { get; } = String.Empty;
        public IEnumerable<string> SupportFormats { get; } = new[] { "*.log", "*.txt" };
        public string InitialFolderFullPath { get; } = Environment.CurrentDirectory;
        private ILogParserSettings LogParserSettings { get; set; }
        private I4FileParser I4FileParser { get; set; }
        private string AugmedicsFileSetting { get; } = "AugmedicsSettings.json";
        public OfflineDataProvider()
        {
        }

        public Task InitializeDataProviderAsync(IAnalogyLogger logger)
        {
            LogManager.Instance.SetLogger(logger);
            I4FileParser = new I4FileParser(LogParserSettings);
            return Task.CompletedTask;
        }

        public void MessageOpened(AnalogyLogMessage message)
        {
            //nop
        }

        public async Task<IEnumerable<AnalogyLogMessage>> Process(string fileName, CancellationToken token, ILogMessageCreatedHandler messagesHandler)
        {
            if (CanOpenFile(fileName))
                return await I4FileParser.Process(fileName, token, messagesHandler);
            return new List<AnalogyLogMessage>(0);
        }

        public IEnumerable<FileInfo> GetSupportedFiles(DirectoryInfo dirInfo, bool recursiveLoad)
            => GetSupportedFilesInternal(dirInfo, recursiveLoad);

        public Task SaveAsync(List<AnalogyLogMessage> messages, string fileName)
        {
            return Task.CompletedTask;
        }

        public bool CanOpenFile(string fileName)
        {
            return true;
        }

        public bool CanOpenAllFiles(IEnumerable<string> fileNames)
        {
            return fileNames.All(CanOpenFile);
        }
        public static List<FileInfo> GetSupportedFilesInternal(DirectoryInfo dirInfo, bool recursive)
        {
            List<FileInfo> files = dirInfo.GetFiles("*.txt")
                .Concat(dirInfo.GetFiles("*.log"))
                .ToList();
            if (!recursive)
                return files;
            try
            {
                foreach (DirectoryInfo dir in dirInfo.GetDirectories())
                {
                    files.AddRange(GetSupportedFilesInternal(dir, true));
                }
            }
            catch (Exception)
            {
                return files;
            }

            return files;
        }
    }
}
