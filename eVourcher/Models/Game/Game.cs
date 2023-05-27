using System;
using System.Collections.Generic;

namespace eVoucher.Models;
public class Game
{


    public Guid ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid ModifiedBy { get; set; }
    public bool IsDeleted { get; set; }
    public int Index { get; set; }
}
