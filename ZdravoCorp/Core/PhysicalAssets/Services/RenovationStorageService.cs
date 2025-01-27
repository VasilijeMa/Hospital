﻿using System.Collections.Generic;
using ZdravoCorp.Core.PhysicalAssets.Model;
using ZdravoCorp.Core.PhysicalAssets.Repositories;
using ZdravoCorp.Core.PhysicalAssets.Repositories.Interfaces;

namespace ZdravoCorp.Core.PhysicalAssets.Services
{
    public class RenovationStorageService
    {
        private IRenovationRepository<Renovation> _simpleRenovationRepository;
        private IRenovationRepository<MergeRenovation> _mergeRenovationRepository;
        private IRenovationRepository<SplitRenovation> _splitRenovationRepository;

        public RenovationStorageService()
        {
            _simpleRenovationRepository = new SimpleRenovationRepository();
            _mergeRenovationRepository = new MergeRenovationRepository();
            _splitRenovationRepository = new SplitRenovationRepository();
        }

        public void SaveRenovation(Renovation renovation)
        {
            if (renovation.GetType() == typeof(MergeRenovation))
            {
                _mergeRenovationRepository.Add((MergeRenovation)renovation);
                return;
            }
            if (renovation.GetType() == typeof(SplitRenovation))
            {
                _splitRenovationRepository.Add((SplitRenovation)renovation);
                return;
            }
            _simpleRenovationRepository.Add(renovation);
        }

        public bool IsRoomScheduledForRenovation(string roomName)
        {
            if (_mergeRenovationRepository.IsRoomScheduledForRenovation(roomName))
            {
                return true;
            }
            if (_simpleRenovationRepository.IsRoomScheduledForRenovation(roomName))
            {
                return true;
            }
            return _splitRenovationRepository.IsRoomScheduledForRenovation(roomName);
        }
        public List<Renovation> ExtractNotYetExecutedRenovations(bool onlyNotYetStarted)
        {
            List<Renovation> extractedRenovations = _simpleRenovationRepository.ExtractNotYetExecutedRenovations(onlyNotYetStarted);
            List<MergeRenovation> extractedMergeRenovations = _mergeRenovationRepository.ExtractNotYetExecutedRenovations(onlyNotYetStarted);
            List<SplitRenovation> extractedSplitRenovations = _splitRenovationRepository.ExtractNotYetExecutedRenovations(onlyNotYetStarted);
            foreach (MergeRenovation mergeRenovation in extractedMergeRenovations)
            {
                extractedRenovations.Add(mergeRenovation);
            }
            foreach (SplitRenovation splitRenovation in extractedSplitRenovations)
            {
                extractedRenovations.Add(splitRenovation);
            }
            return extractedRenovations;
        }

        public void AddAll(List<Renovation> newRenovations)
        {
            foreach (var renovation in newRenovations)
            {
                if (renovation.GetType() == typeof(MergeRenovation))
                {
                    _mergeRenovationRepository.Add((MergeRenovation)renovation);
                    continue;
                }
                if (renovation.GetType() == typeof(SplitRenovation))
                {
                    _splitRenovationRepository.Add((SplitRenovation)renovation);
                    continue;
                }
                _simpleRenovationRepository.Add(renovation);
            }
        }
    }
}
