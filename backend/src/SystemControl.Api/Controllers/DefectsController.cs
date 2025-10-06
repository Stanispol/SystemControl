using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using SystemControl.Api.Data;
using SystemControl.Api.DTOs;
using SystemControl.Api.Models;

namespace SystemControl.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DefectsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DefectsController(AppDbContext context)
        {
            _context = context;
        }

        // ----------------------
        // СОЗДАНИЕ ДЕФЕКТА
        // ----------------------
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDefectDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Проверим, существует ли проект
            var project = await _context.Projects
                .Include(p => p.Defects)
                .FirstOrDefaultAsync(p => p.Id == dto.ProjectId);

            if (project == null)
                return BadRequest(new { message = "Указанный проект не найден." });

            // Проверим, если указан пользователь — существует ли он
            if (dto.AssignedUserId.HasValue)
            {
                var userExists = await _context.Users
                    .AnyAsync(u => u.Id == dto.AssignedUserId.Value.ToString());

                if (!userExists)
                    return BadRequest(new { message = "Назначенный пользователь не найден." });
            }

            var defect = new Defect
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                Priority = (DefectPriority)dto.Priority,
                Status = DefectStatus.Open,
                AssignedUserId = dto.AssignedUserId?.ToString(), // безопасная конверсия
                DueDate = dto.DueDate ?? DateTime.UtcNow,
                ProjectId = dto.ProjectId,
                AttachmentUrl = dto.AttachmentUrl
            };

            _context.Defects.Add(defect);
            await _context.SaveChangesAsync();

            // Подгружаем связи
            await _context.Entry(defect).Reference(d => d.AssignedUser).LoadAsync();
            await _context.Entry(defect).Reference(d => d.Project).LoadAsync();

            // Подгружаем проект с обновлённым списком дефектов
            defect.Project = await _context.Projects
                .Include(p => p.Defects)
                .FirstOrDefaultAsync(p => p.Id == defect.ProjectId);

            return Ok(defect);
        }

        // ----------------------
        // ОБНОВЛЕНИЕ ДЕФЕКТА
        // ----------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDefectDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var defect = await _context.Defects.FindAsync(id);
            if (defect == null)
                return NotFound(new { message = "Дефект не найден." });

            defect.Title = dto.Title ?? defect.Title;
            defect.Description = dto.Description ?? defect.Description;
            defect.Priority = (DefectPriority)dto.Priority;
            defect.Status = (DefectStatus)dto.Status;
            defect.AssignedUserId = dto.AssignedUserId?.ToString() ?? defect.AssignedUserId; // безопасная конверсия
            defect.DueDate = dto.DueDate ?? defect.DueDate;
            defect.AttachmentUrl = dto.AttachmentUrl ?? defect.AttachmentUrl;

            // Проверка назначенного пользователя, если указано
            if (dto.AssignedUserId.HasValue)
            {
                var userExists = await _context.Users
                    .AnyAsync(u => u.Id == dto.AssignedUserId.Value.ToString());
                if (!userExists)
                    return BadRequest(new { message = "Назначенный пользователь не найден." });
            }

            await _context.SaveChangesAsync();

            // Загружаем актуальные данные
            await _context.Entry(defect).Reference(d => d.AssignedUser).LoadAsync();
            await _context.Entry(defect).Reference(d => d.Project).LoadAsync();

            // Обновляем проект с актуальным списком дефектов
            defect.Project = await _context.Projects
                .Include(p => p.Defects)
                .FirstOrDefaultAsync(p => p.Id == defect.ProjectId);

            return Ok(defect);
        }

        // ----------------------
        // ПОЛУЧЕНИЕ ДЕФЕКТА ПО ID
        // ----------------------
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var defect = await _context.Defects
                .Include(d => d.AssignedUser)
                .Include(d => d.Project)
                .Include(d => d.Comments)
                    .ThenInclude(c => c.Author)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (defect == null)
                return NotFound(new { message = "Дефект не найден." });

            return Ok(defect);
        }
    }
}
