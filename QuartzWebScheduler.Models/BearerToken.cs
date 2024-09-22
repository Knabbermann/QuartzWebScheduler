using System.ComponentModel.DataAnnotations;

namespace QuartzWebScheduler.Models
{
    public class BearerToken
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string? Token { get; set; }
    }
}
