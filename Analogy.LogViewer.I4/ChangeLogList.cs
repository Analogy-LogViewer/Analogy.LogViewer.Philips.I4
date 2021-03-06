﻿using System;
using System.Collections.Generic;
using Analogy.Interfaces;

namespace Analogy.LogViewer.Philips.I4
{
    public static class ChangeLogList
    {
        public static IEnumerable<AnalogyChangeLog> GetChangeLog()
        {
            yield return new AnalogyChangeLog("Initial commit", AnalogChangeLogType.None, "Lior Banai", new DateTime(2020, 02, 15));
        }
    }
}
