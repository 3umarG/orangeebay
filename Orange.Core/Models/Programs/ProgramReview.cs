using System.ComponentModel.DataAnnotations;
using Orange_Bay.Models.Auth;

namespace Orange_Bay.Models.Programs;

public class ProgramReview
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    [Range(0.0, 5.0)] public double RateFromFive { get; set; }
    public string? ReviewDescription { get; set; }

    public int UserId { get; set; }
    public ApplicationUser User { get; set; }

    public int ProgramId { get; set; }
    public Program Program { get; set; }
}