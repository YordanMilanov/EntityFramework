using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Library.Data.Models
{
    public class IdentityUserBooks
    {
        public string CollectorId { get; set; } = null!;
        [ForeignKey(nameof(CollectorId))]
        public virtual IdentityUser Collector { get; set; } = null!;
        public int BookId {get; set; }

        [ForeignKey(nameof(BookId))]
        public virtual Book Book { get; set; } = null!;
    }
}