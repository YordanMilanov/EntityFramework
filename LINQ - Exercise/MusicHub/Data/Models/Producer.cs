using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MusicHub.Data.Models;

internal class Producer
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(ValidationConstants.ProducerNameMaxLength)]
    public string Name { get; set; }

    [AllowNull]
    public string? Pseudonym { get; set; }


    [AllowNull]
    public string? PhoneNumer { get; set; }
}
