// File: JearCarrier/Model/Carrier.cs  (API project)
using System.ComponentModel.DataAnnotations;

namespace JearCarrier.Model
{
    public class Carrier
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string CarrierName { get; set; }

        [Required, MaxLength(200)]
        public string Address { get; set; }

        [MaxLength(200)]
        public string Address2 { get; set; }

        [Required, MaxLength(100)]
        public string City { get; set; }

        [Required, StringLength(2, MinimumLength = 2)]
        public string State { get; set; }

        [Required, MaxLength(15)]
        public string Zip { get; set; }

        [Required, MaxLength(120)]
        public string Contact { get; set; }

        [Required, MaxLength(40)]
        public string Phone { get; set; }

        [MaxLength(40)]
        public string Fax { get; set; }

        [Required, EmailAddress, MaxLength(200)]
        public string Email { get; set; }

        [Timestamp] public byte[] RowVersion { get; set; }
    }
}




