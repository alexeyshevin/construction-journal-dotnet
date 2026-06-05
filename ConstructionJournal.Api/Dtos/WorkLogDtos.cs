using System.ComponentModel.DataAnnotations;

namespace ConstructionJournal.Api.Dtos;

public record WorkTypeDto(Guid Id, string Name, string Unit);

public record WorkLogDto(
    Guid Id,
    DateTime WorkDate,
    decimal Amount,
    string Unit,
    string PerformerName,
    Guid WorkTypeId,
    WorkTypeDto WorkType,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public class CreateWorkLogDto
{
    [Required]
    public DateTime WorkDate { get; set; }

    [Required]
    public Guid WorkTypeId { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Объем должен быть больше 0")]
    public decimal Amount { get; set; }

    [Required]
    public string Unit { get; set; } = "";

    [Required]
    [MinLength(2)]
    public string PerformerName { get; set; } = "";
}

public class UpdateWorkLogDto : CreateWorkLogDto;