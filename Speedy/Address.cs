using System;
using System.Collections.Generic;
using System.Text;

namespace Speedy
{
    public class Address
    {        
        public string FullAddress { get; set; }
        public string SpeedyType { get; set; }
        public bool IsOffice { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Neighborhood { get; set; }
        public string PostalCode { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string BlockNumber { get; set; }
        public string Municipality { get; set; }
        public string Region { get; set; }
        public string PhoneCode { get; set; }
        public string FullRegion { get; set; }
        public string SiteType { get; internal set; }
    }
}
