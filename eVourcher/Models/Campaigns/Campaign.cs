using System;
using System.Collections.Generic;

namespace eVoucher.Models;
public class Campaign
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime? CreatedDate { get; set; } = DateTime.MinValue;
    public DateTime? StartedDate { get; set; } = DateTime.MinValue;
    public DateTime? ExpiredDate { get; set; } = DateTime.MinValue;
    public DateTime? ModifiedDate { get; set; } = DateTime.MinValue;
    public Guid ModifiedBy { get; set; }
    public bool IsDeleted { get; set; }
    public Guid CreatedBy { get; set; }
    public int Index { get; set; }
    public byte[] Image { get; set; }
    public IList<CampaignGame> CampaignGames { get; set; } = new List<CampaignGame>();

    public string CreatedDateText => CreatedDate == null ? string.Empty : CreatedDate.Value.ToShortDateString();
    public string StartedDateText => StartedDate == null ? string.Empty : StartedDate.Value.ToShortDateString();
    public string ExpiredDateText => ExpiredDate == null ? string.Empty : ExpiredDate.Value.ToShortDateString();
    public string ImageUrl
    {
        get
        {
            if (Image is not null && Image.Length > 0)
            {
                var imageBase64 = Convert.ToBase64String(Image);
                var imageDataUrl = $"data:image/png;base64,{imageBase64}";
                return imageDataUrl;
            }
            return string.Empty;
        }
    }

    public string UserName { get; set; }
    public IList<Partner> Partners { get; set; }
    public IList<Game> Games { get; set; }
}
