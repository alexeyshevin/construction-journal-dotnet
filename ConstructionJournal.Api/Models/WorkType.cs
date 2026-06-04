namespace ConstructionJournal.Api.Models;

public class WorkType
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Unit { get; set; }
    public List<WorkLog>? WorkLogs { get; set; }

}