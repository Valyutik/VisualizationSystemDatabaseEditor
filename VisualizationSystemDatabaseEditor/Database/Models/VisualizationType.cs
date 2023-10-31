using System.Collections.Generic;

namespace VisualizationSystemDatabaseEditor.Database.Models;

public partial class VisualizationType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Visualization> Visualizations { get; set; } = new List<Visualization>();
}
