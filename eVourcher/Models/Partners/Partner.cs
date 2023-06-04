using System;
using System.Collections.Generic;
using static eVoucher.Models.DataType;

namespace eVoucher.Models;
public class Partner    
{
    public Guid Id { get; set; }
    public string CompanyName { get; set; }
    public string Description { get; set; }
    public string CompanyEmailAddress { get; set; }
    public string CompanyPhone { get; set; }
    public IList<Address> CompanyAddess { get; set; }
    public byte[] Image { get; set; }
    public DateTime? JoinDate { get; set; } = DateTime.Now;
    public ePartnerType Type { get; set; }
    public bool IsActive { get; set; }
    public int Index { get; set; }
    public Guid User_ID_FK { get; set; }
    public IList<PartnerCampaign> PartnerCampaigns { get; set; } = new List<PartnerCampaign>();

    public string JoinDateText
    {
        get
        {
            return JoinDate == null ? string.Empty : JoinDate.Value.ToShortDateString();
        }
    }
}
