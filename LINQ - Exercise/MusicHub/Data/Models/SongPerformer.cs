using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicHub.Data.Models;

public class SongPerformer
{

    //SongId and PerfomerId has to be composite PK (their combination should be unique for the table) we do this in the MusicHubDbContext onModelCreating method
    [ForeignKey(nameof(Song))]
    public int SongId { get; set; }

    [Required]
    public virtual Song Song { get; set; } = null!;
   
    
    [ForeignKey(nameof(Performer))]
    public int PerformerId { get; set; }

    [Required]
    public virtual Performer Performer { get; set; } = null!;
}
