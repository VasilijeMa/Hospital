using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using ZdravoCorp.Core.PhysicalAssets.Model;
using ZdravoCorp.Core.PhysicalAssets.Repositories.Interfaces;

namespace ZdravoCorp.Core.PhysicalAssets.Repositories
{
    public class SimpleRenovationRepository : IRenovationRepository<Renovation>
    {
        private List<Renovation> _renovations;

        public List<Renovation> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../../ZdravoCorp/data/simpleRenovations.json");
            var json = reader.ReadToEnd();
            List<Renovation> allRenovations = JsonConvert.DeserializeObject<List<Renovation>>(json);

            return allRenovations;
        }

        public SimpleRenovationRepository()
        {
            _renovations = LoadAll();
        }

        public void SaveAll()
        {
            string json = JsonConvert.SerializeObject(_renovations, Formatting.Indented);
            File.WriteAllText("./../../../../ZdravoCorp/data/simpleRenovations.json", json);
        }

        public void Add(Renovation renovation)
        {
            _renovations.Add(renovation);
            SaveAll();
        }

        public bool IsRoomScheduledForRenovation(string roomName)
        {
            foreach (var renovation in _renovations)
            {
                if (renovation.IsFinished)
                {
                    continue;
                }
                if (renovation.RoomName == roomName)
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
                shouldExtract = renovation.IsEligibleToStart() && onlyNotYetStarted || renovation.IsEligibleToFinish() && !onlyNotYetStarted;
                if (shouldExtract)
                {
                    extractedRenovations.Add(renovation);
                    continue;
                }
                remainingRenovations.Add(renovation);
            }
            _renovations = remainingRenovations;
            return extractedRenovations;
        }
    }
}