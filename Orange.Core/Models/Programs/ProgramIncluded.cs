using System.Text.Json.Serialization;

namespace Orange_Bay.Models.Programs;

public class ProgramIncluded
{
    public int Id { get; set; }
    public string Description { get; set; }

    public int ProgramId { get; set; }
    [JsonIgnore] public Program Program { get; set; }
}