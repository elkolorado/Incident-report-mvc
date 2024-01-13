using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ErrorReports.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
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

        public ICollection<ErrorComment> Comments { get; set; }
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

    public class ErrorComment
    {
        [Key]
        public int CommentId { get; set; }

        [Required]
        [ForeignKey("ReportComment")]
        public int ErrorReportId { get; set; }
        public ErrorReport ErrorReport { get; set; }

        [Required]
        [ForeignKey("UserComment")]
        public string UserId { get; set; }
        public AppUser User { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(500)")]
        public string Content { get; set; }

        [Required]
        [Column(TypeName = "Datetime")]
        public DateTime CommentDate { get; set; }
    }

    public class HelpDeskAssignment
    {
        [Key]
        public int AssignmentId { get; set; }

        [Required]
        [ForeignKey("AssignedReport")]
        public int ErrorReportId { get; set; }
        public ErrorReport ErrorReport { get; set; }

        [Required]
        [ForeignKey("AssignedUser")]
        public string HelpDeskUserId { get; set; }
        public AppUser HelpDeskUser { get; set; }

        [Column(TypeName = "Datetime")]
        public DateTime AssignedDate { get; set; }
    }
}
