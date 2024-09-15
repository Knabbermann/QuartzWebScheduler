using System.ComponentModel.DataAnnotations;

namespace QuartzWebScheduler.Models
{
    public class QuartzGroup
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string GroupName { get; set; }
    }
}
