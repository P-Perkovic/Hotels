using System;
using System.Collections.Generic;
using System.Text;

namespace Hotels.Domain.Queries
{
    public class LocationQuery : PageQuery
    {
        public float Distance { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        // Longitude is between -180 and 180
        // Latitude is between -90 and 90
        public bool Validate()
        {
            return Longitude > -180 && Longitude < 180 && 
                Latitude > -90 && Latitude < 90 && 
                Distance > 0 && base.Validate();
        }
    }
}
