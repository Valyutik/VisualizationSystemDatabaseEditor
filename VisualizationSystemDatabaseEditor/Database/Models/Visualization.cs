using System.Collections.Generic;

namespace VisualizationSystemDatabaseEditor.Database.Models;

public partial class Visualization
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int TypeId { get; set; }

    public int AnalystId { get; set; }

    public int DataSourceId { get; set; }

    public string? Parameters { get; set; }

    public string? VisualizationSettings { get; set; }

    public virtual User Analyst { get; set; } = null!;

    public virtual DataSource DataSource { get; set; } = null!;

    public virtual VisualizationType Type { get; set; } = null!;

    public virtual ICollection<Dashboard> Dashboards { get; set; } = new List<Dashboard>();
}
