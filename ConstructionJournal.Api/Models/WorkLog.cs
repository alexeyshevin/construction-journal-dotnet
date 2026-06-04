namespace ConstructionJournal.Api.Models;

public class WorkLog
{
    public Guid Id { get; set; }
    public DateTime WorkDate { get; set; }
    public Guid WorkTypeId { get; set; }
    public WorkType? WorkType { get; set; }
    public decimal Amount { get; set; }
    public string? Unit { get; set; }
    public string? PerformerName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}