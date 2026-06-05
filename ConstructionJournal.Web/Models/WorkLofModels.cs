namespace ConstructionJournal.Web.Models;

public class WorkTypeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string Unit { get; set; } = "";
}

public class WorkLogDto
{
    public Guid Id { get; set; }
    public DateTime WorkDate { get; set; }
    public decimal Amount { get; set; }
    public string Unit { get; set; } = "";
    public string PerformerName { get; set; } = "";
    public Guid WorkTypeId { get; set; }
    public WorkTypeDto WorkType { get; set; } = new();
}

public class WorkLogFormModel
{
    public DateTime? WorkDate { get; set; } = DateTime.Today;
    public Guid WorkTypeId { get; set; }
    public decimal Amount { get; set; } = 1;
    public string Unit { get; set; } = "";
    public string PerformerName { get; set; } = "";
}