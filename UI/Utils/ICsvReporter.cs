using System.Collections.Generic;

namespace UI.Utils
{
    public interface ICsvReporter
    {
        string ToCsv<T>(string separator, IEnumerable<T> objectlist);
    }
}