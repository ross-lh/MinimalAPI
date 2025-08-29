using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MinimalAPI.Enums;

namespace MinimalAPI.ModelViews
{
    public record AdminModelView
    {
        public int Id { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Profile { get; set; } = default!;
    }
}