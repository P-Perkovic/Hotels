﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hotels.Commands
{
    public record HotelCommand
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; } = 0m;
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
