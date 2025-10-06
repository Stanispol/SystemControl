using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SystemControl.Api.Data;
using SystemControl.Api.Models;

namespace SystemControl.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ProjectsController(AppDbContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _db.Projects.ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var p = await _db.Projects.Include(x => x.Defects).FirstOrDefaultAsync(x => x.Id == id);
            if (p == null) return NotFound();
            return Ok(p);
        }

        [HttpPost]
        [Authorize] // require auth
        public async Task<IActionResult> Create(Project dto)
        {
            var project = new Project { Name = dto.Name, Description = dto.Description };
            _db.Projects.Add(project);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = project.Id }, project);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(Guid id, Project dto)
        {
            var p = await _db.Projects.FindAsync(id);
            if (p == null) return NotFound();
            p.Name = dto.Name;
            p.Description = dto.Description;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var p = await _db.Projects.FindAsync(id);
            if (p == null) return NotFound();
            _db.Projects.Remove(p);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
