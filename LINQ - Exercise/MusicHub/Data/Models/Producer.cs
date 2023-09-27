using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MusicHub.Data.Models;

public class Producer
{

    public Producer() 
    {
        this.Albums = new HashSet<Album>();
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(ValidationConstants.ProducerNameMaxLength)]
    public string Name { get; set; }

    [AllowNull]
    public string? Pseudonym { get; set; }


    [AllowNull]
    public string? PhoneNumer { get; set; }

    public virtual ICollection<Album> Albums { get; set; }
}
