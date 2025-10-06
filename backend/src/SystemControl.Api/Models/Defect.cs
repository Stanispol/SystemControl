using System;
using System.Collections.Generic;

namespace SystemControl.Api.Models
{
    public class Defect
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DefectPriority Priority { get; set; } = DefectPriority.Low;
        public DefectStatus Status { get; set; } = DefectStatus.Open;

        // ✅ FK тип string, чтобы совпадал с ApplicationUser.Id
        public string? AssignedUserId { get; set; }
        public ApplicationUser? AssignedUser { get; set; }

        public DateTime DueDate { get; set; } = DateTime.UtcNow;

        public Guid ProjectId { get; set; }
        public Project? Project { get; set; }

        public string? AttachmentUrl { get; set; }

        public ICollection<DefectComment> Comments { get; set; } = new List<DefectComment>();
    }

    public enum DefectPriority
    {
        Low = 0,
        Medium = 1,
        High = 2
    }

    public enum DefectStatus
    {
        Open = 0,
        InProgress = 1,
        Closed = 2
    }
}
