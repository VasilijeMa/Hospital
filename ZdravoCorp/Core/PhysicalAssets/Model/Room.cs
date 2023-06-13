using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace ZdravoCorp.Core.PhysicalAssets.Model
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
        [JsonProperty("typeOfRoom")]
        private RoomType TypeOfRoom { get; set; }
        [JsonConstructor]
        public Room(int typeOfRoom, string name) : base(name)
        {
            TypeOfRoom = (RoomType)typeOfRoom;
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