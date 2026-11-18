using System.ComponentModel.DataAnnotations;

namespace backend.Dtos;

public sealed class ChangeRetakeRequestStatusRequest : IValidatableObject
{
    [Required]
    [RegularExpression("^(New|InProgress|Closed|Cancelled)$")]
    public string Status { get; init; } = string.Empty;

    public DateOnly? RetakeDate { get; init; }
    public TimeOnly? RetakeTime { get; init; }
    public string? Audience { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Status == "InProgress")
        {
            if (RetakeDate is null)
            {
                yield return new ValidationResult(
                    "Для статуса InProgress укажите дату пересдачи.",
                    [nameof(RetakeDate)]);
            }

            if (RetakeTime is null)
            {
                yield return new ValidationResult(
                    "Для статуса InProgress укажите время пересдачи.",
                    [nameof(RetakeTime)]);
            }

            if (string.IsNullOrWhiteSpace(Audience))
            {
                yield return new ValidationResult(
                    "Для статуса InProgress укажите аудиторию.",
                    [nameof(Audience)]);
            }
        }
    }
}
