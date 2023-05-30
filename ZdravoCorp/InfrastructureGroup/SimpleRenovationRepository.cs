﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace ZdravoCorp.InfrastructureGroup
{
    public class SimpleRenovationRepository
    {
        private List<Renovation> _renovations;

        private List<Renovation> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/simpleRenovations.json");
            var json = reader.ReadToEnd();
            List<Renovation> allRenovations = JsonConvert.DeserializeObject<List<Renovation>>(json);

            return allRenovations;
        }

        public SimpleRenovationRepository()
        {
            _renovations = LoadAll();
        }

        private void SaveAll()
        {
            string json = JsonConvert.SerializeObject(_renovations, Formatting.Indented);
            File.WriteAllText("./../../../data/simpleRenovations.json", json);
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
                shouldExtract = (renovation.IsEligibleToStart() && onlyNotYetStarted) || (renovation.IsEligibleToFinish() && !onlyNotYetStarted);
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