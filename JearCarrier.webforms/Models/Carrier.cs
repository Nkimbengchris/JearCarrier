using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JearCarrier.webforms.Models
{
    public class Carrier
    {
        public int Id { get; set; }
        public string CarrierName { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }   // SC, NC etc.
        public string Zip { get; set; }
        public string Contact { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
    }
}