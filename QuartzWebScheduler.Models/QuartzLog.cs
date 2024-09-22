using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using QuartzWebScheduler.Utility;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuartzWebScheduler.Models
{
    public class QuartzLog
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Type { get; set; } = StaticDetails.LogTypeInformation;

        public DateTime Date { get; set; } = DateTime.Now;

        [Required]
        public int Statuscode { get; set; }

        [Required]
        public string Message { get; set; } = "";

        public string QuartzJobConfigId { get; set; }

        [ForeignKey("QuartzJobConfigId")]
        [ValidateNever]
        public QuartzJobConfig QuartzJobConfig { get; set; }

        [Required]
        public string CronExpression { get; set; }

        [Required]
        public string RequestType { get; set; }

        [Required]
        public string RequestUrl { get; set; }

        public string? RequestBody { get; set; }
    }
}
