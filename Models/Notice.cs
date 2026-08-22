using System.ComponentModel.DataAnnotations;

namespace CampusConnect.Models
{
    public class Notice
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Category { get; set; } = string.Empty;

        public string TargetDepartment { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        public string PostedBy { get; set; } = "Admin";

        public DateTime PostedDate { get; set; } = DateTime.Now;

        public string? FilePath { get; set; }
    }
}