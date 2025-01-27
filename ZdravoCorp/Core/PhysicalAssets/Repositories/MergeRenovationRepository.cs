﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using ZdravoCorp.Core.PhysicalAssets.Model;
using ZdravoCorp.Core.PhysicalAssets.Repositories.Interfaces;

namespace ZdravoCorp.Core.PhysicalAssets.Repositories
{
    public class MergeRenovationRepository : IRenovationRepository<MergeRenovation>
    {
        private List<MergeRenovation> _renovations;

        public List<MergeRenovation> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../../ZdravoCorp/data/mergeRenovations.json");
            var json = reader.ReadToEnd();
            List<MergeRenovation> allRenovations = JsonConvert.DeserializeObject<List<MergeRenovation>>(json);

            return allRenovations;
        }

        public MergeRenovationRepository()
        {
            _renovations = LoadAll();
        }

        public void SaveAll()
        {
            string json = JsonConvert.SerializeObject(_renovations, Formatting.Indented);
            File.WriteAllText("./../../../../ZdravoCorp/data/mergeRenovations.json", json);
        }

        public void Add(MergeRenovation renovation)
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
                if (renovation.RoomName == roomName || renovation.SecondRoomName == roomName)
                {
                    return true;
                }
            }
            return false;
        }

        public List<MergeRenovation> ExtractNotYetExecutedRenovations(bool onlyNotYetStarted)
        {
            bool shouldExtract;
            List<MergeRenovation> extractedRenovations = new List<MergeRenovation>();
            List<MergeRenovation> remainingRenovations = new List<MergeRenovation>();
            foreach (MergeRenovation renovation in _renovations)
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