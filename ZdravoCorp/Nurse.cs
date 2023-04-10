using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp
{
    class Nurse:User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Nurse() : base() { }
        public Nurse(int id, string firstName, string lastName, string username, string password, string type):base(username, password, type)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
        }
        public static List<Nurse> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/nurse.json");
            var json = reader.ReadToEnd();
            List<Nurse> nurses = JsonConvert.DeserializeObject<List<Nurse>>(json);
            return nurses;
        }

        public override string ToString()
        {
            return "Id: " + Id + ", FirstName: " + FirstName + ", LastName: " + LastName;
        }
    }
}
