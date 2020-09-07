using Analogy.Interfaces;
using Analogy.Interfaces.Factories;
using System;
using System.Collections.Generic;

namespace Analogy.LogViewer.Philips.I4
{
    public class DataProvidersFactory : IAnalogyDataProvidersFactory
    {
        public Guid FactoryId { get; set; } = PrimaryFactory.Id;
        public string Title { get; set; } = "I4 Offline Logs";
        public IEnumerable<IAnalogyDataProvider> DataProviders { get; } = new List<IAnalogyDataProvider> { new I4OfflineDataProvider() };

    }
}
