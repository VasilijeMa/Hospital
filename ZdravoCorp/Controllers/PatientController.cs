using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Domain;
using ZdravoCorp.Servieces;

namespace ZdravoCorp.Controllers
{
    public class PatientController
    {
        private PatientService patientService = new PatientService();

        public void WriteAll()
        {
            patientService.WriteAll();
        }
    }
}
