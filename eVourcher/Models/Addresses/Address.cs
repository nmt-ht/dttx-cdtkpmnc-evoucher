using static eVoucher.Models.DataType;

namespace eVoucher.Models
{
    public class Address : Entity
    {
        public string Street { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public eAddressType Type { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
