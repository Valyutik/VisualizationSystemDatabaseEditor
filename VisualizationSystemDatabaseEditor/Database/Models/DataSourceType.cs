using System.Collections.Generic;

namespace VisualizationSystemDatabaseEditor.Database.Models;

public partial class DataSourceType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<DataSource> DataSources { get; set; } = new List<DataSource>();
}
