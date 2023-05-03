using System;

namespace eVoucher.Models;
public class User
{
    public Guid ID { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? EmailAddress { get; set; }
    public string? Phone { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public Guid Address_ID { get; set; }
    public bool IsActive { get; set; }
}
