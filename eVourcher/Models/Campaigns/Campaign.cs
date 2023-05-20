using System;
using System.Collections.Generic;

namespace eVoucher.Models;
public class Campaign : Entity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime StartedDate { get; set; }
    public DateTime ExpiredDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public User ModifiedBy { get; set; }
    public bool IsDeleted { get; set; }
    public User CreatedBy { get; set; }
}
