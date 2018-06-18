using System;
using System.Collections.Generic;
using System.Text;

namespace Adbp.Extensions
{
    public static class BoolExtension
    {
        public static string SourceName(this bool value, string name)
        {
            return $"BOOL_{name.ToUpper()}_{value.ToString().ToUpper()}";
        }
    }
}
