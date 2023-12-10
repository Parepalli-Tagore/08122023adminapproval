using System;
using System.Collections.Generic;

namespace _08122023adminapproval.Models;

public partial class Registration
{
    public int Id { get; set; }

    public string? Firstname { get; set; }

    public string? Lastname { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Password { get; set; }

    public int? IsActive { get; set; }

    public int? Roleid { get; set; }

    public virtual Role? Role { get; set; }
}
