using System.ComponentModel.DataAnnotations;

namespace MusicHub.Data.Models;

public class Performer
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(ValidationConstants.PerformerFirstNameMaxLength)]
    public string FirstName { get; set; } = null!;

    [Required]
    [MaxLength(ValidationConstants.PerformerLastNameMaxLength)]
    public string LastName { get; set; } = null!;

    [Required]
    public int Age { get; set; }

    [Required]
    public decimal NetWorth { get; set; }
}
