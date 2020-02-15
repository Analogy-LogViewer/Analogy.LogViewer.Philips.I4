using System.Collections.Generic;
using Analogy.Interfaces;
using Analogy.Interfaces.Factories;

namespace Analogy.LogViewer.Augmedics
{
    public class DataProvidersFactory : IAnalogyDataProvidersFactory
    {
        public string Title => "Augmedics Offline Logs";

        public IEnumerable<IAnalogyDataProvider> Items { get; set; }

        public DataProvidersFactory()
        {
            Items= new List<IAnalogyDataProvider> { new OfflineDataProvider() };
        }
    }
}
