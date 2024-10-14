﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Hotels.Domain.Queries
{
    public class LocationQuery : PageQuery
    {
        public float Distance { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
