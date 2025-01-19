using System;
using System.Collections.Generic;

namespace OnlineShop.Models.Db;

public partial class Banner
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? SubTitle { get; set; }

    public string? ImageName { get; set; }

    public short? Priority { get; set; }

    public string? Link { get; set; }

    public string? Position { get; set; }
}
