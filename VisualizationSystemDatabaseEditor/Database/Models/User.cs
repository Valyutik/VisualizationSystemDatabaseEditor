using System.Collections.Generic;

namespace VisualizationSystemDatabaseEditor.Database.Models;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int RoleId { get; set; }

    public virtual ICollection<Dashboard> Dashboards { get; set; } = new List<Dashboard>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<Visualization> Visualizations { get; set; } = new List<Visualization>();
}
