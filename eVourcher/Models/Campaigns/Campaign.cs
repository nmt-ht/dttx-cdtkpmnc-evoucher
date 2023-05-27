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
    public IList<Game> Games { get; set; } = new List<Game>();
}
