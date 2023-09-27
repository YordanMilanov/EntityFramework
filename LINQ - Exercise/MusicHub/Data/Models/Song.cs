using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using MusicHub.Data.Models.Enums;

namespace MusicHub.Data.Models;

public class Song
{

    public Song()
    {
        this.SongPerformers = new HashSet<SongPerformer>();
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(ValidationConstants.SongNameMaxLength)]
    public string Name { get; set; } = null!; //This is required! and cannot accept null because of the = null!, and by default string is nullable false


    //this is stored as BIGINT in the database as count of thicks(beats) 10000 thicks = 1ms
    [Required]
    public TimeSpan Duration { get; set; } //By default TimeSpan is required

    [Required]
    public DateTime CreatedOn { get; set; }

    [Required]
    public Genre Genre { get; set; } //Enumrations are storade as INT in DB

    [AllowNull]
    [ForeignKey(nameof(Album))]
    public int? AlbumId { get; set; }

    [AllowNull]
    public virtual Album? Album { get; set; }

    [Required]
    [ForeignKey(nameof(Writer))]
    public int WriterId { get; set; } // by default required

    [Required]
    public virtual Writer Writer { get; set; } = null!;

    [Required]
    public decimal Price { get; set; } //by default required

    public virtual ICollection<SongPerformer> SongPerformers { get; set; }

}
