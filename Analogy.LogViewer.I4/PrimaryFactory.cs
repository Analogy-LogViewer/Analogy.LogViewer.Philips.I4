using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analogy.Interfaces;
using Analogy.Interfaces.Factories;

namespace Analogy.LogViewer.Augmedics
{
    public class PrimaryFactory : IAnalogyFactory
    {
        public Guid FactoryID { get; } = new Guid("314F8E9C-BF88-4FAA-9892-B604FFBF46EF");
        public string Title { get; } = "I4 Log Parser";
        public IAnalogyDataProvidersFactory DataProviders { get; }
        public IAnalogyCustomActionsFactory Actions { get; } = new EmptyActionsFactory(); // if no custom action needed
        public IEnumerable<IAnalogyChangeLog> ChangeLog { get; } = ChangeLogList.GetChangeLog();
        public IEnumerable<string> Contributors { get; } = new List<string> { "Lior Banai" };
        public string About { get; } = "Log Parser for   Log files";

        public PrimaryFactory()
        {
            DataProviders = new DataProvidersFactory();
        }
    }
}
