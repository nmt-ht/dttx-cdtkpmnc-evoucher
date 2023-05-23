﻿using System;
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
    public DateTime? JoinDate { get; set; }
    public ePartnerType Type { get; set; }
    public bool IsActive { get; set; }
}
