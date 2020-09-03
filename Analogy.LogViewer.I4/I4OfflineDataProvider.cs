using Analogy.Interfaces;
using Analogy.LogViewer.Philips.I4.Managers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Analogy.LogViewer.Philips.I4
{
    class I4OfflineDataProvider : IAnalogyOfflineDataProvider
    {
        public Guid Id { get; } = new Guid("266B3711-5D05-496E-8297-3805E3C78388");
        public Image LargeImage => null;
        public Image SmallImage => null;

        public string OptionalTitle { get; } = "I4 Offline log";

        public bool CanSaveToLogFile { get; } = false;
        public string FileOpenDialogFilters { get; } = "Plain nlog file (*.nlog)|*.nlog";
        public string FileSaveDialogFilters { get; } = string.Empty;
        public IEnumerable<string> SupportFormats { get; } = new[] { "*.nlog" };
        public string InitialFolderFullPath { get; } = Environment.CurrentDirectory;
        public bool DisableFilePoolingOption { get; } = false;
        private ILogParserSettings LogParserSettings { get; set; }
        private I4FileParser I4FileParser { get; set; }
        private string I4FileSetting { get; } = "i4Settings.json";
        public bool UseCustomColors { get; set; } = false;
        public IEnumerable<(string originalHeader, string replacementHeader)> GetReplacementHeaders()
            => Array.Empty<(string, string)>();

        public (Color backgroundColor, Color foregroundColor) GetColorForMessage(IAnalogyLogMessage logMessage)
            => (Color.Empty, Color.Empty);
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


        private static List<FileInfo> GetSupportedFilesInternal(DirectoryInfo dirInfo, bool recursive)
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
