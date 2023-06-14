using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.PhysicalAssets.Model;
using ZdravoCorp.Core.VacationRequest.Model;
using ZdravoCorp.Core.VacationRequest.Repositories.Interfaces;

namespace ZdravoCorp.Core.VacationRequest.Repositories
{
    public class ProcessedVacationRequestRepository : IProcessedVacationRequestRepository
    {
        private List<ProcessedVacationRequest> _processedVacationRequests;
        public List<ProcessedVacationRequest> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../../ZdravoCorp/data/processedVacationRequests.json");
            var json = reader.ReadToEnd();
            List<ProcessedVacationRequest> allRequests = JsonConvert.DeserializeObject<List<ProcessedVacationRequest>>(json);

            return allRequests;
        }
        public ProcessedVacationRequestRepository()
        {
            _processedVacationRequests = LoadAll();
        }
        public void SaveAll()
        {
            string json = JsonConvert.SerializeObject(_processedVacationRequests, Formatting.Indented);
            File.WriteAllText("./../../../../ZdravoCorp/data/processedVacationRequests.json", json);
        }

        public void Add(ProcessedVacationRequest request)
        {
            _processedVacationRequests.Add(request);
            SaveAll();
        }


    }
}
