using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hotels.Dtos
{
    public record LocationDto
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
