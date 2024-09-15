using System.ComponentModel.DataAnnotations;

namespace QuartzWebScheduler.Models
{
    public class QuartzJobConfig
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public string JobName { get; set; }

        [Required]
        public string GroupName { get; set; } = "None";

        [Required]
        public string CronExpression { get; set; }

        [Required]
        public string Url { get; set; }
    }
}
