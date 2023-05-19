using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Repositories;

namespace ZdravoCorp.Domain
{
    public class Log
    {
        public List<LogElement> Elements { get; set; }
        public int MakeCounter { get; set; }
        public int UpdateCancelCounter { get; set; }
        public Log()
        {
            Elements = LogRepository.Load();
            LogRepository.Refresh(Elements);
            MakeCounter = 0;
            UpdateCancelCounter = 0;
        }
        public Log(List<LogElement> elements) { Elements = elements; }

    }
}
