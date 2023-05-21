using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using ZdravoCorp.Domain;

namespace ZdravoCorp.Repositories
{
    public class UserRepository
    {
        public static List<User> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/users.json");
            var json = reader.ReadToEnd();
            List<User> users = JsonConvert.DeserializeObject<List<User>>(json);
            return users;
        }
        public static void WriteAll(List<User> users)
        {
            string json = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText("./../../../data/users.json", json);
        }
        public static void RemoveUser(string username)
        {
            foreach (User user in Singleton.Instance.users)
            {
                if (username == user.Username)
                {
                    Singleton.Instance.users.Remove(user);
                    return;
                }
            }
        }
    }
}
