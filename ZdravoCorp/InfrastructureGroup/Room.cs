﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace ZdravoCorp.InfrastructureGroup
{
    public enum RoomType
    {
        [Description("Operating Theater")]
        OperatingTheater = 1,
        [Description("Examination Room")]
        ExaminationRoom = 2,
        [Description("Infirmary")]
        Infirmary = 3,
        [Description("Waiting Room")]
        WaitingRoom = 4
    }
    public class Room : Infrastructure
    {
        private RoomType TypeOfRoom { get; set; }
        public Room(int typeOfRoom, string name) : base(name)
        {
            this.TypeOfRoom = (RoomType)typeOfRoom;
        }

        public static Dictionary<string, Room> LoadAllExaminationRoom()
        {
            Dictionary<string, Room> examinationRooms = new Dictionary<string, Room>();
            Dictionary<string, Room> allRooms = RoomRepository.LoadAll();
            foreach (var room in allRooms)
            {
                if (RoomType.ExaminationRoom.Equals(room.Value.TypeOfRoom))
                {
                    examinationRooms.Add(room.Key, room.Value);
                }
            }
            return examinationRooms;
        }

        public RoomType GetTypeOfRoom() { return TypeOfRoom; }
        public string ToString()
        {
            return Name + " " + TypeOfRoom.ToString();
        }

        public static string GetTypeDescription(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            DescriptionAttribute attribute = field.GetCustomAttribute<DescriptionAttribute>();

            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static List<string> LoadAllRoomTypes()
        {
            List<string> roomTypes = new List<string>();
            foreach (RoomType roomType in Enum.GetValues(typeof(RoomType)))
            {
                roomTypes.Add(GetTypeDescription(roomType));
            }
            return roomTypes;
        }
    }
}