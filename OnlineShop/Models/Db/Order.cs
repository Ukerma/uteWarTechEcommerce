using System;
using System.Collections.Generic;

namespace OnlineShop.Models.Db;

public partial class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? CompanyName { get; set; }

    public string Country { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? Comment { get; set; }

    public string? CouponCode { get; set; }

    public decimal? CouponDiscount { get; set; }

    public decimal? Shipping { get; set; }

    public decimal? SubTotal { get; set; }

    public decimal? Total { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? TransId { get; set; }

    public string? Status { get; set; }
}
