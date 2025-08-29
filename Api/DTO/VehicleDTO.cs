using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalAPI.DTO
{
    public record VehicleDTO
    {
        public string Model { get; set; } = default!;
        public string Make { get; set; } = default!;
        public int Year { get; set; } = default!;
    }
}