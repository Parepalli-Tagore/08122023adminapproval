using System;
using System.Collections.Generic;

namespace _08122023adminapproval.Models;

public partial class Role
{
    public int Roleid { get; set; }

    public string? Rolename { get; set; }

    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();
}
