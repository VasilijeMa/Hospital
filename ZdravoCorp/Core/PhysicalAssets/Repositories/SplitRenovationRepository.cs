using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using ZdravoCorp.Core.PhysicalAssets.Model;
using ZdravoCorp.Core.PhysicalAssets.Repositories.Interfaces;

namespace ZdravoCorp.Core.PhysicalAssets.Repositories
{
    public class SplitRenovationRepository : IRenovationRepository<SplitRenovation>
    {
        private List<SplitRenovation> _renovations;

        public List<SplitRenovation> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../../ZdravoCorp/data/splitRenovations.json");
            var json = reader.ReadToEnd();
            List<SplitRenovation> allRenovations = JsonConvert.DeserializeObject<List<SplitRenovation>>(json);

            return allRenovations;
        }

        public SplitRenovationRepository()
        {
            _renovations = LoadAll();
        }

        public void SaveAll()
        {
            string json = JsonConvert.SerializeObject(_renovations, Formatting.Indented);
            File.WriteAllText("./../../../../ZdravoCorp/data/splitRenovations.json", json);
        }

        public void Add(SplitRenovation renovation)
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

        public List<SplitRenovation> ExtractNotYetExecutedRenovations(bool onlyNotYetStarted)
        {
            bool shouldExtract;
            List<SplitRenovation> extractedRenovations = new List<SplitRenovation>();
            List<SplitRenovation> remainingRenovations = new List<SplitRenovation>();
            foreach (SplitRenovation renovation in _renovations)
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