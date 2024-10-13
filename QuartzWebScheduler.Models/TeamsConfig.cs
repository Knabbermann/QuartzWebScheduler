using System.ComponentModel.DataAnnotations;

namespace QuartzWebScheduler.Models
{
    public class TeamsConfig
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public string TenantId { get; set; }

        [Required]
        public string ClientId { get; set; }             

        [Required]
        public string ClientSecret { get; set; }         

        [Required]
        public string GraphApiEndpoint { get; set; }     

        public string TeamId { get; set; }               

        public string ChannelId { get; set; }            
    }
}
