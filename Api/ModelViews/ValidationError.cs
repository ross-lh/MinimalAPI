using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalAPI.ModelViews
{
    public struct ValidationError
    {
        public List<string> Messages { get; set; }
    }
}