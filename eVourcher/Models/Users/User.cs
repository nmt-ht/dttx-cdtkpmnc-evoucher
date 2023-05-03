using System;
using System.Collections.Generic;

namespace eVoucher.Models;
public class User : Entity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; } = DateTime.MinValue;
    public string EmailAddress { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public IList<Address> Addresses { get; set; } = new List<Address>();
    public bool IsActive { get; set; }
}
