using System;

namespace eVoucher.Models
{
    public class UserVoucher
    {
        public string VoucherCode { get; set; }
        public string ReceivedDate { get; set; }
        public DateTime ExpiredDate { get; set;}
        public string GameName { get; set; }
        public string CampaignName { get; set; }
        public bool IsActive { get; set; }
        public int Index { get; set; }
    }
}
