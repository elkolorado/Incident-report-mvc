using System.ComponentModel.DataAnnotations;

namespace ErrorReports.Models
{
    public class ErrorReport
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime DateReported { get; set; }

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
