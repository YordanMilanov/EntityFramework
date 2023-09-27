using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MusicHub.Data.Models;

public class Album
{
    public Album()
    {
        this.Songs = new HashSet<Song>();
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(ValidationConstants.AlbumNameMaxLength)]
    public string Name { get; set; } = null!;

    [Required]
    public DateTime ReleaseDate { get; set; }

    [NotMapped] //this is not going in the database it is only for the code in EF, Excludes it from DB
    public decimal Price => this.Songs.Sum(s => s.Price);

    [AllowNull]
    [ForeignKey(nameof(Producer))]
    public int? ProducerId { get; set; }

    public virtual Producer? Producer { get; set; }

    public virtual ICollection<Song> Songs { get; set; }
}
