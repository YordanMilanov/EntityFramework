using System.ComponentModel.DataAnnotations;
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

    [AllowNull]
    public int? ProducerId { get; set; }

    public virtual ICollection<Song> Songs { get; set; }
}
