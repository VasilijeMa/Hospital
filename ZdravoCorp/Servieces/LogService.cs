using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ZdravoCorp.Domain;

namespace ZdravoCorp.Servieces
{
    public class LogService
    {

        public static void Count(int patientId)
        {
            foreach (var element in Singleton.Instance.Log.Elements)
            {
                if (element.Appointment.PatientId == patientId)
                {
                    if (element.Type.Equals("make"))
                    {
                        Singleton.Instance.Log.MakeCounter++;
                    }
                    else
                    {
                        Singleton.Instance.Log.UpdateCancelCounter++;
                    }
                }
            }
        }

        public static void CheckConditions(Patient patient)
        {
            if (Singleton.Instance.Log.MakeCounter > 8 || Singleton.Instance.Log.UpdateCancelCounter >= 5)
            {
                patient.IsBlocked = true;
            }
            else
            {
                patient.IsBlocked = false;
            }
        }
    }
}
