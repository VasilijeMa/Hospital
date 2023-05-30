using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Domain;

namespace ZdravoCorp.Core.Repositories.Interfaces
{
    public interface ILogRepository
    {
        public List<LogElement> Load();

        public void Write();

        public void AddElement(Appointment appointment, Patient patient);

        public void UpdateCancelElement(Appointment appointment, Patient patient);

        public void Refresh();

        public Log GetLog();

        public void SetLog(Log log);
    }
}
