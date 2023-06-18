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
        private bool isActive;
        public bool IsActive
        {
            get
            {
                return ExpiredDate.Date >= DateTime.Now.Date;
            }
            set
            {
                isActive = value;
            }
        }
        public int Index { get; set; }

        public string Status
        {
            get
            {
                return IsActive ? "Available" : "Expried";
            }
        }
    }
}
