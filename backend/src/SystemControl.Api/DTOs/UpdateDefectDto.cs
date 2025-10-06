using System;

namespace SystemControl.Api.DTOs
{
    public class UpdateDefectDto
    {
        public string? Title { get; set; } // nullable, чтобы не менять, если не указан
        public string? Description { get; set; } 
        public int Priority { get; set; } = 0; // DefectPriority enum
        public int Status { get; set; } = 0;   // DefectStatus enum
        public Guid? AssignedUserId { get; set; } // nullable Guid для клиента
        public DateTime? DueDate { get; set; }
        public string? AttachmentUrl { get; set; }
    }
}
