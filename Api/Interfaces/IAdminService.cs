using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MinimalAPI.DTO;
using MinimalAPI.Entities;

namespace MinimalAPI.Interfaces
{
    public interface IAdminService
    {
        List<Admin> All(int? page);
        Admin? SearchById(int id);
        Admin? Login(LoginDTO loginDTO);
        Admin Insert(Admin admin);
    }
}