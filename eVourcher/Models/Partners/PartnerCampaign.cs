using System;
using System.Collections.Generic;

namespace eVoucher.Models;
public class PartnerCampaign
{
    public Guid ID { get; set; }
    public Campaign Campaign { get; set; }
    public int Index { get; set; }
    public string Name => Campaign != null ? Campaign.Name : string.Empty;
    public string Description => Campaign != null ? Campaign.Description : string.Empty;
    public string StartedDate => Campaign != null ? Campaign.StartedDateText : string.Empty;
    public string ExpiredDate => Campaign != null ? Campaign.ExpiredDateText : string.Empty;
}
