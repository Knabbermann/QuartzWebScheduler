using System.ComponentModel.DataAnnotations;

namespace QuartzWebScheduler.Models
{
    public class SmtpConfig
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string SmtpServer { get; set; }         

        [Required]
        public int Port { get; set; }                  

        [Required]
        public bool EnableSsl { get; set; }            

        [Required]
        public string Username { get; set; }           

        [Required]
        public string Password { get; set; }           

        [Required]
        public string FromEmail { get; set; }           
    }
}
