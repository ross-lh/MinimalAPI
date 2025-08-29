using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MinimalAPI.Data;
using MinimalAPI.Entities;
using MinimalAPI.Interfaces;

namespace MinimalAPI.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly DatabaseContext _context;
        public VehicleService(DatabaseContext context)
        {
            _context = context;
        }
        public List<Vehicle> All(int? page = 1, string? model = null, string? make = null)
        {
            var query = _context.Vehicles.AsQueryable();
            if (!string.IsNullOrEmpty(model))
            {
                query = query.Where(v => EF.Functions.Like(v.Model.ToLower(), $"%{model.ToLower()}%"));
            }

            int ItensPerPage = 10;

            if (page != null)
                query = query.Skip(((int)page - 1) * ItensPerPage).Take(ItensPerPage);

            return query.ToList();
        }

        public Vehicle? SearchById(int id)
        {
            return _context.Vehicles.Where(v => v.Id == id).FirstOrDefault();
        }

        public void Insert(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
            _context.SaveChanges();
        }

        public void Update(Vehicle vehicle)
        {
            _context.Vehicles.Update(vehicle);
            _context.SaveChanges();
        }

        public void Delete(Vehicle vehicle)
        {
            _context.Vehicles.Remove(vehicle);
            _context.SaveChanges();
        }
    }
}