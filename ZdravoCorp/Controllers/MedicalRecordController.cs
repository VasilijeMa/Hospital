using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using ZdravoCorp.Domain;
using ZdravoCorp.Servieces;

namespace ZdravoCorp.Controllers
{
    public class MedicalRecordController
    {
        MedicalRecordService medicalRecordService = new MedicalRecordService();

        public MedicalRecord GetMedicalRecord(int medicalRecordId)
        {
            return medicalRecordService.GeMedicalRecord(medicalRecordId);
        }

        public void WriteAll(List<MedicalRecord> medicalRecords)
        {
            medicalRecordService.WriteAll(medicalRecords);
        }
    }
}
