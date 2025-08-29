using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalAPI.ModelViews
{
    public struct Home
    {
        public string Message { get => "Welcome to the Vehicle Minimal API"; }
        public string Docs { get => "/swagger"; }
    }
}