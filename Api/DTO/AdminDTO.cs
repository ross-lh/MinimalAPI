using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MinimalAPI.Enums;

namespace MinimalAPI.DTO
{
    public class AdminDTO
    {
        public string UserName { get; set; } = default!;
        public string Password { get; set; } = default!;
        public Profile? Profile { get; set; } = default!;
    }
}