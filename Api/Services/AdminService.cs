using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MinimalAPI.Data;
using MinimalAPI.DTO;
using MinimalAPI.Entities;
using MinimalAPI.Interfaces;

namespace MinimalAPI.Services
{
    public class AdminService : IAdminService
    {
        private readonly DatabaseContext _context;
        public AdminService(DatabaseContext context) => _context = context;

        public List<Admin> All(int? page)
        {
            var query = _context.Admins.AsQueryable();

            int ItensPerPage = 10;

            if (page != null)
                query = query.Skip(((int)page - 1) * ItensPerPage).Take(ItensPerPage);

            return query.ToList();
        }

        public Admin? SearchById(int id)
        {
            return _context.Admins.Where(v => v.Id == id).FirstOrDefault();
        }

        public Admin? Login(LoginDTO loginDTO)
        {
            var adm = _context.Admins.Where(a => a.UserName == loginDTO.UserName && a.Password == loginDTO.Password).FirstOrDefault();
            return adm;
        }

        public Admin Insert(Admin admin)
        {
            _context.Admins.Add(admin);
            _context.SaveChanges();

            return admin;
        }
    }
}