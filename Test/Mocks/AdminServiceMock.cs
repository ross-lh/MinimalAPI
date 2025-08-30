using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MinimalAPI.DTO;
using MinimalAPI.Entities;
using MinimalAPI.Interfaces;

namespace Test.Mocks
{
    public class AdminServiceMock : IAdminService
    {
        private static List<Admin> admins = new List<Admin>()
        {
            new Admin {
                Id = 1,
                UserName = "admin1",
                Password = "password1",
                Profile = "Adm"
            },
            new Admin {
                Id = 2,
                UserName = "editor1",
                Password = "password2",
                Profile = "Editor"
            },
        };
        public List<Admin> All(int? page)
        {
            return admins;
        }

        public Admin Insert(Admin admin)
        {
            admin.Id = admins.Count + 1;

            admins.Add(admin);
            return admin;
        }

        public Admin? Login(LoginDTO loginDTO)
        {
            return admins.Find(a => a.UserName == loginDTO.UserName && a.Password == loginDTO.Password);
        }

        public Admin? SearchById(int id)
        {
            return admins.Find(a => a.Id == id);
        }
    }
}