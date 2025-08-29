using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MinimalAPI.DTO;
using MinimalAPI.Entities;

namespace MinimalAPI.Interfaces
{
    public interface IVehicleService
    {
        List<Vehicle> All(int? page = 1, string? model = null, string? make = null);
        Vehicle? SearchById(int id);
        void Insert(Vehicle vehicle);
        void Update(Vehicle vehicle);
        void Delete(Vehicle vehicle);
    }
}