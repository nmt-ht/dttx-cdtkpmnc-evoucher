using static eVoucher.Models.DataType;

namespace eVoucher.Models
{
    public class Address : Entity
    {
        public string Street { get; set; }
        public string District { get; set; } 
        public string City { get; set; }
        public string Country { get; set; }
        public eAddressType Type { get; set; }
        public bool IsDeleted { get; set; }
    }
}
