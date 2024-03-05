namespace Orange_Bay.Models.Programs;

public class Program
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? InternalNotes { get; set; }
    public string? SpecialRequirements { get; set; }
    public int MaxCapacity { get; set; }
    public string? Location { get; set; }
    public int DurationInHours { get; set; }

    public int? DaysBeforeCancellation { get; set; }

    public virtual ICollection<ProgramPlan>? ProgramPlans { get; set; } = new HashSet<ProgramPlan>();
    public virtual ICollection<ProgramImage>? ProgramImages { get; set; } = new HashSet<ProgramImage>();
    public virtual ICollection<ProgramReview>? ProgramReviews { get; set; } = new HashSet<ProgramReview>();
    public virtual ICollection<ProgramPrice>? ProgramPrices { get; set; } = new HashSet<ProgramPrice>();
    public virtual ICollection<ProgramIncluded>? ProgramIncludedDetails { get; set; } = new HashSet<ProgramIncluded>();
    public virtual ICollection<ProgramExcluded>? ProgramExcludedDetails { get; set; } = new HashSet<ProgramExcluded>();
    public virtual ICollection<ProgramNote>? ProgramNotes { get; set; } = new HashSet<ProgramNote>();
}