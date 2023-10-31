using System.Collections.Generic;

namespace VisualizationSystemDatabaseEditor.Database.Models;

public partial class DataSource
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int TypeId { get; set; }

    public string Location { get; set; } = null!;

    public string SecurityCredentials { get; set; } = null!;

    public int? FrequencyOfUse { get; set; }

    public virtual DataSourceType Type { get; set; } = null!;

    public virtual ICollection<Visualization> Visualizations { get; set; } = new List<Visualization>();
}
