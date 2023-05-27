using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eVoucher.Models;
public class User
{
    public Guid ID { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; } = DateTime.MinValue;
    public string EmailAddress { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    [Required(ErrorMessage = "Password is required")]
    [StringLength(8, ErrorMessage = "Must be between 5 and 8 characters", MinimumLength = 5)]

    public string Password { get; set; } = string.Empty;
    public IList<Address> Addresses { get; set; } = new List<Address>();
    public bool IsActive { get; set; }
    public int Index { get; set; }
}
