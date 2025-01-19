using System;
using System.Collections.Generic;

namespace OnlineShop.Models.Db;

public partial class Setting
{
    public int Id { get; set; }

    public decimal? Shipping { get; set; }

    public string? Title { get; set; }

    public string? Address { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Copyright { get; set; }

    public string? Instagram { get; set; }

    public string? Facebook { get; set; }

    public string? GooglePlus { get; set; }

    public string? Youtube { get; set; }

    public string? Twitter { get; set; }

    public string? Logo { get; set; }
}
