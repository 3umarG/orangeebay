using System.Text.Json.Serialization;

namespace Orange_Bay.Models.Programs;

public class ProgramImage
{
    public int Id { get; set; }
    public string PhotoUrl { get; set; }
    [JsonIgnore] public int ProgramId { get; set; }
    [JsonIgnore] public Program Program { get; set; }
}