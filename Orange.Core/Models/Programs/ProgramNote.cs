using System.Text.Json.Serialization;

namespace Orange_Bay.Models.Programs;

public class ProgramNote
{
    public int Id { get; set; }
    public string Note { get; set; }

    public int ProgramId { get; set; }
    [JsonIgnore] public Program Program { get; set; }
}