﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using ZdravoCorp.Domain;

namespace ZdravoCorp.Repositories
{
    public class UserRepository
    {
        private List<User> users;
        public List<User> Users { get => users; }
        public UserRepository()
        {
            users = LoadAll();
        }
        public List<User> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/users.json");
            var json = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<User>>(json);
        }
        public void WriteAll()
        {
            string json = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText("./../../../data/users.json", json);
        }
        public void RemoveUser(string username)
        {
            foreach (User user in Singleton.Instance.UserRepository.Users)
            {
                if (username == user.Username)
                {
                    Singleton.Instance.UserRepository.Users.Remove(user);
                    return;
                }
            }
        }
    }
}
