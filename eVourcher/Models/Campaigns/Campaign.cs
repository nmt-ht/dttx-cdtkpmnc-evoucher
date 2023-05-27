using System;
using System.Collections.Generic;

namespace eVoucher.Models;
public class Campaign
{
    public Guid ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? StartedDate { get; set; }
    public DateTime? ExpiredDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public Guid ModifiedBy { get; set; }
    public bool IsDeleted { get; set; }
    public Guid CreatedBy { get; set; }
    public int Index { get; set; }
    public IList<Game> Games { get; set; } = new List<Game>();
}
