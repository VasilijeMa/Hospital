using System.Collections.Generic;

namespace ZdravoCorp.Core.Domain
{
    public class Log
    {
        public List<LogElement> Elements { get; set; }

        public int MakeCounter { get; set; }

        public int UpdateCancelCounter { get; set; }

        public Log()
        {
            Elements = new List<LogElement>();
        }

        public Log(List<LogElement> elements) { Elements = elements; }

    }
}
