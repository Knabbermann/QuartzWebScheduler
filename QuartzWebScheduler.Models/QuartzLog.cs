using QuartzWebScheduler.Utility;
using System.ComponentModel.DataAnnotations;

namespace QuartzWebScheduler.Models
{
    public class QuartzLog
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Type { get; set; } = StaticDetails.LogTypeInformation;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required]
        public string Message { get; set; }
    }
}
