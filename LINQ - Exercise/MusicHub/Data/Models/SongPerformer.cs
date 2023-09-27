namespace MusicHub.Data.Models;

public class SongPerformer
{

    //SongId and PerfomerId has to be composite PK (their combination should be unique for the table) we do this in the MusicHubDbContext onModelCreating method
    public int SongId { get; set; }
    public int PerformerId { get; set; }
}
