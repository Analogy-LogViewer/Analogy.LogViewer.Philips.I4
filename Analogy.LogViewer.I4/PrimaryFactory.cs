using Analogy.Interfaces;
using Analogy.Interfaces.Factories;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Analogy.LogViewer.Philips.I4
{
    public class PrimaryFactory : IAnalogyFactory
    {
        internal static Guid Id = new Guid("314F8E9C-BF88-4FAA-9892-B604FFBF46EF");
        public Guid FactoryId { get; set; } = Id;
        public string Title { get; set; } = "I4 Log Parser";
        public IEnumerable<IAnalogyChangeLog> ChangeLog { get; set; } = ChangeLogList.GetChangeLog();
        public Image LargeImage { get; set; } = null;
        public Image SmallImage { get; set; } = null;
        public IEnumerable<string> Contributors { get; set; } = new List<string> { "Lior Banai" };
        public string About { get; set; } = "Log Parser for I4 Log files";
    }
}
