﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using ZdravoCorp.Core.PhysicalAssets.Repositories;
using ZdravoCorp.Core.PhysicalAssets.Repositories.Interfaces;
using MessageBox = System.Windows.Forms.MessageBox;
using ZdravoCorp.Core.PatientHealthcare.Hospitalcare.Repositories.Interfaces;
using ZdravoCorp.Core.PatientHealthcare.Hospitalcare.Model;

namespace ZdravoCorp.Core.PatientHealthcare.Hospitalcare.Services
{
    public class HospitalStayService
    {
        private IHospitalStayRepository hospitalStayRepository;
        private IRoomRepository roomRepository;

        public HospitalStayService(IHospitalStayRepository hospitalStayRepository, IRoomRepository roomRepository)
        {
            this.roomRepository = roomRepository;
            this.hospitalStayRepository = hospitalStayRepository;
        }

        public void Add(HospitalStay hospitalStay)
        {
            hospitalStayRepository.Add(hospitalStay);
        }

        public HospitalStay GetHospitalStay(int examinationId)
        {
            return hospitalStayRepository.GetHospitalStay(examinationId);
        }

        public List<HospitalStay> LoadAll()
        {
            return hospitalStayRepository.LoadAll();
        }

        public void WriteAll(List<HospitalStay> hospitalStays)
        {
            hospitalStayRepository.WriteAll(hospitalStays);
        }

        public int GetNumberOfPatientsInTheRoom(string roomId, DateOnly startDate, DateOnly endDate)
        {
            return hospitalStayRepository.GetNumberOfPatientsInTheRoom(roomId, startDate, endDate);
        }

        public List<string> FindFreeRooms(DateOnly startDate, DateOnly endDate)
        {
            List<string> freeRooms = new List<string>();
            List<string> unavailableRooms = new List<string>();
            if (LoadAll().Count == 0)
            {
                return roomRepository.LoadAllInfirmaryRooms();
            }
            foreach (string roomName in roomRepository.LoadAllInfirmaryRooms())
            {
                foreach (HospitalStay hospitalStay in LoadAll())
                {
                    if (hospitalStay.RoomId.Equals(roomName))
                    {
                        if (startDate < hospitalStay.StartDate && endDate < hospitalStay.StartDate
                            || startDate > hospitalStay.EndDate && endDate > hospitalStay.EndDate
                            || GetNumberOfPatientsInTheRoom(roomName, startDate, endDate) < 3)
                        {
                            if (!freeRooms.Contains(roomName))
                            {
                                freeRooms.Add(roomName);
                            }
                        }
                        else
                        {
                            unavailableRooms.Add(roomName);
                        }
                    }
                    if (!freeRooms.Contains(roomName) && !unavailableRooms.Contains(roomName))
                    {
                        freeRooms.Add(roomName);
                    }
                }
            }
            return freeRooms;
        }

        public void releasePatient() 
        {
            List<HospitalStay> hospitalStays = new List<HospitalStay>();
            foreach (HospitalStay hospitalStay in LoadAll()) 
            {
                if (!(hospitalStay.EndDate <= DateOnly.FromDateTime(DateTime.Now))) 
                {
                    hospitalStays.Add(hospitalStay);
                }
            }
            WriteAll(hospitalStays);
        }
    }
}
