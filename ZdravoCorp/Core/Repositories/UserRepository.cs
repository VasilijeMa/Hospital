using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Repositories.Interfaces;

namespace ZdravoCorp.Core.Repositories
{
    public class UserRepository : IUserRepository
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
            foreach (User user in users)
            {
                if (username == user.Username)
                {
                    users.Remove(user);
                    return;
                }
            }
        }

        public List<User> GetUsers()
        {
            return Users;
        }

        public void AddUser(User user)
        {
            users.Add(user);
        }
    }
}
