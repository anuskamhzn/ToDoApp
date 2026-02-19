using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters")]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        public bool IsCompleted { get; set; } = false;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Foreign key to Identity user
        public string UserId { get; set; } = string.Empty;

        // Navigation property (optional but useful)
        // public ApplicationUser User { get; set; } = null!;
    }
}
