﻿using Analogy.Interfaces;
using Analogy.Interfaces.Factories;
using System;
using System.Collections.Generic;

namespace Analogy.LogViewer.Philips.I4
{
    public class PrimaryFactory : IAnalogyFactory
    {
        internal static Guid Id = new Guid("314F8E9C-BF88-4FAA-9892-B604FFBF46EF");
        public Guid FactoryId { get; } = Id;
        public string Title { get; } = "I4 Log Parser";
        public IEnumerable<IAnalogyChangeLog> ChangeLog { get; } = ChangeLogList.GetChangeLog();
        public IEnumerable<string> Contributors { get; } = new List<string> { "Lior Banai" };
        public string About { get; } = "Log Parser for I4 Log files";
    }
}
