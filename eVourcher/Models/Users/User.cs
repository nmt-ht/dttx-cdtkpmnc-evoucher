using System;
using System.Collections.Generic;

namespace eVoucher.Models;
public class User : Entity
{
    public string FirstName { get; set; } 
    public string LastName { get; set; }
    public DateTime? DateOfBirth { get; set; } 
    public string EmailAddress { get; set; }
    public string Phone { get; set; } 
    public string UserName { get; set; }
    public string Password { get; set; }
    public IList<Address> Addresses { get; set; } = new List<Address>();
    public bool IsActive { get; set; }
    public int Index { get; set; }
}
