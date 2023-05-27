using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.EquipmentGroup;

namespace ZdravoCorp.InfrastructureGroup
{
    public class RenovationRepository
    {
        private List<Renovation> _renovations;

        private List<Renovation> LoadAll() //different for Extended
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/simpleRenovations.json");
            var json = reader.ReadToEnd();
            List<Renovation> allRenovations = JsonConvert.DeserializeObject<List<Renovation>>(json);

            return allRenovations;
        }

        public RenovationRepository()
        {
            _renovations = LoadAll();
        }

        private void SaveAll() //different for Extended
        {
            string json = JsonConvert.SerializeObject(_renovations, Formatting.Indented);
            File.WriteAllText("./../../../data/simpleRenovations.json", json);
        }

        public void Add(Renovation renovation)
        {
            _renovations.Add(renovation);
            SaveAll();
        }

        public void AddAll(List<Renovation> renovations)
        {
            if (renovations.Count == 0) { return; }
            foreach (var renovation in renovations)
            {
                _renovations.Add(renovation);
            }
            SaveAll();
        }

        public bool IsRoomScheduledForRenovation(string roomName)
        {
            foreach (var renovation in _renovations)
            {
                if (renovation.RoomName == roomName) //extra condition for Extended
                {
                    return true;
                }
            }
            return false;
        }

        public List<Renovation> ExtractNotYetExecutedRenovations(bool onlyNotYetStarted)
        {
            bool shouldExtract;
            List<Renovation> extractedRenovations = new List<Renovation>();
            List<Renovation> remainingRenovations = new List<Renovation>();
            foreach (Renovation renovation in _renovations)
            {
                shouldExtract = (renovation.IsEligibleToStart() && onlyNotYetStarted) || (renovation.IsEligibleToFinish() && !onlyNotYetStarted);
                if (shouldExtract)
                {
                    extractedRenovations.Add(renovation);
                }
                else
                {
                    remainingRenovations.Add(renovation);
                }
            }
            _renovations = remainingRenovations;
            return extractedRenovations;
        }

        //public List<Renovation> ExtractNotYetFinishedRenovations()
        //{
        //    List<Renovation> extractedRenovations = new List<Renovation>();
        //    List<Renovation> remainingRenovations = new List<Renovation>();
        //    foreach (Renovation renovation in _renovations)
        //    {
        //        if (renovation.IsEligibleToFinish())
        //        {
        //            extractedRenovations.Add(renovation);
        //        }
        //        else
        //        {
        //            remainingRenovations.Add(renovation);
        //        }
        //    }
        //    _renovations = remainingRenovations;
        //    return extractedRenovations;
        //}



    }
}
