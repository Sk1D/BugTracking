using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTrackingApp.Domain.Core
{
    [Table("Projects")]
    public class Project
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
