using System;

namespace SystemControl.Api.DTOs
{
    public class CreateDefectDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Priority { get; set; } = 0; // DefectPriority enum
        public Guid? AssignedUserId { get; set; } // nullable Guid для клиента
        public DateTime? DueDate { get; set; }
        public Guid ProjectId { get; set; }
        public string? AttachmentUrl { get; set; }
    }
}
