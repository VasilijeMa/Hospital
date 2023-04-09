using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp
{
    class Singleton
    {
        private static Singleton instance;
        public Schedule Schedule { get; set; }
        public List<Patient> patients;
        public static Singleton Instance
        {
            get 
            {
                if (instance == null)
                {
                    return instance = new Singleton();
                }
                return instance;
            }
        }
        private Singleton()
        {
            Schedule = new Schedule();
            patients = Patient.LoadAll();
        }
    }
}
