using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.Services
{
    public class UserService : IUserService
    {
        private List<UserModel> _users;

        public UserService()
        {
            _users = new List<UserModel>();
        }

        public List<UserModel> GetUsers()
        {
            return _users;
        }

        public UserModel GetUser(string id)
        {
            var user = _users.Where(x => x.Id == id).FirstOrDefault();
            return user;
        }
        public UserModel AddUser(UserModel user)
        {
            _users.Add(user);
            return user;
        }
        public string DeleteUser(string id)
        {
            var user = _users.Where(x => x.Id == id).FirstOrDefault();
            int index = _users.IndexOf(user);
            _users.RemoveAt(index);
            return id;
        }

        public UserModel UpdateUser(string id, UserModel updatedUser)
        {
            var user = _users.Where(x => x.Id == id).FirstOrDefault();
            int index = _users.IndexOf(user);
            _users[index] = updatedUser;
            return updatedUser;
        }
    }
}
