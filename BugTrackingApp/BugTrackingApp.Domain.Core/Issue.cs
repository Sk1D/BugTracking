using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTrackingApp.Domain.Core
{
    [Table("Issues")]
    public class Issue
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public Project Project { get; set; }

    }
}
