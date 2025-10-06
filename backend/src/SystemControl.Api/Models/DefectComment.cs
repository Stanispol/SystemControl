using System;

namespace SystemControl.Api.Models
{
    public class DefectComment
    {
        public Guid Id { get; set; }

        // ✅ FK тип string
        public string AuthorId { get; set; } = string.Empty;
        public ApplicationUser? Author { get; set; }

        public string Text { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Guid DefectId { get; set; }
        public Defect? Defect { get; set; }
    }
}
