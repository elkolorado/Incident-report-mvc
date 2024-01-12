using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErrorReports.Models
{
    public class ErrorReport
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(150)")]
        public string Title { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(200)")]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "Datetime")]
        public DateTime DateReported { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        public string ReporterName { get; set; }


        public ErrorStatus Status { get; set; }

        public ErrorPriority Priority { get; set; }
    }

    public enum ErrorStatus
    {
        Open,
        InProgress,
        Closed
    }

    public enum ErrorPriority
    {
        Low,
        Medium,
        High
    }

    public static class StatusExtensions
    {
        public static string ToText(this ErrorStatus status) =>
            status switch
            {
                ErrorStatus.Open => "Open",
                ErrorStatus.InProgress => "In Progress",
                ErrorStatus.Closed => "Closed",
                _ => throw new NotSupportedException()
            };
    }
}
