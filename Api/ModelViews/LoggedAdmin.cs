using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalAPI.ModelViews
{
    public record LoggedAdmin
    {
        public string UserName { get; set; } = default!;
        public string Profile { get; set; } = default!;
        public string Token { get; set; } = default!;
    }
}