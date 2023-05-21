using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using ZdravoCorp.Domain;

namespace ZdravoCorp.Repositories
{
    public class DoctorRepository
    {
        public static List<Doctor> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/doctor.json");
            var json = reader.ReadToEnd();
            List<Doctor> doctors = JsonConvert.DeserializeObject<List<Doctor>>(json);
            return doctors;
        }

    }
}
