using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Threading.Tasks;
using Analogy.Interfaces;
using Analogy.LogViewer.Augmedics.Managers;

namespace Analogy.LogViewer.Augmedics
{
    class I4OfflineDataProvider : IAnalogyOfflineDataProvider
    {
        public Guid ID { get; } = new Guid("266B3711-5D05-496E-8297-3805E3C78388");
        public string OptionalTitle { get; } = "I4 Offline log";

        public bool CanSaveToLogFile { get; } = false;
        public string FileOpenDialogFilters { get; } = "Plain nlog file (*.nlog)|*.nlog";
        public string FileSaveDialogFilters { get; } = String.Empty;
        public IEnumerable<string> SupportFormats { get; } = new[] { "*.nlog" };
        public string InitialFolderFullPath { get; } = Environment.CurrentDirectory;
        private ILogParserSettings LogParserSettings { get; set; }
        private I4FileParser I4FileParser { get; set; }
        private string AugmedicsFileSetting { get; } = "i4Settings.json";
        public I4OfflineDataProvider()
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
            return SupportFormats.Any(f => f.EndsWith(Path.GetExtension(fileName)));
        }

        public bool CanOpenAllFiles(IEnumerable<string> fileNames)
        {
            return fileNames.All(CanOpenFile);
        }
        public static List<FileInfo> GetSupportedFilesInternal(DirectoryInfo dirInfo, bool recursive)
        {
            List<FileInfo> files = dirInfo.GetFiles("*.nlog")
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
