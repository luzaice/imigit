using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.Services
{
    public interface IUserService
    {
        public List<UserModel> GetUsers();
        public UserModel GetUser(string Id);
        public UserModel AddUser(UserModel NewUser);
        public UserModel UpdateUser(string Id, UserModel updatedUser);
        public string DeleteUser(string Id);
    }
}
