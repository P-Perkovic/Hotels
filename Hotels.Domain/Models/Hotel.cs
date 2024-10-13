using System;
using System.Collections.Generic;
using System.Text;
using NetTopologySuite.Geometries;

namespace Hotels.Domain.Models
{
    public class Hotel : Entity
    {
        public string Name { get; set; }
        public Point Location { get; set; }
    }
}
