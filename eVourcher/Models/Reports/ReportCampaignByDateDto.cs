using System;

namespace eVoucher.Models
{
    public class ReportCampaignByDateDto
    {
        public DateTime CreatedDate { get; set; }
        public int TotalCampaign { get; set; }
    }

    public class ReportCampaignRequest
    {
        public DateTime? CreatedDateFrom { get; set; }
        public DateTime? CreatedDateTo { get; set; }
    }
}
