using System;
using System.Collections.Generic;

namespace OnlineShop.Models.Db;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool IsAdmin { get; set; }

    public DateTime? RegisterDate { get; set; }

    public int? RecoveryCode { get; set; }
}
