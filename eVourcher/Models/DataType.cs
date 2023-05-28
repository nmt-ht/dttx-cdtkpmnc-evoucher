using System.ComponentModel;

namespace eVoucher.Models
{
    public class DataType
    {
        public enum eAddressType
        {
            [Description("Ship To")]
            ShipTo = 0,
            [Description("Bill To")]
            BillTo = 1,
            [Description("Ship To & Bill To")]
            BillToShipTo = 2,
            [Description("Company")]
            Company = 3,
        }

        public enum ePartnerType
        {
            Food = 0,
            Drink = 1,
            Technology = 2
        }

        public enum eVoucherType
        {
            Ten = 0,
            Twenty = 1,
            Thirty = 2,
            Fifty = 3,
            OneHundered = 4
        }

        public enum eAction
        {
            Add = 0,
            Edit = 1,
            Delete = 2
        }
        public enum eAdminComponent
        {
            Campaign = 0,
            Partner = 1,
            User = 2,
            Dasboard = 3
        }
    }
}
