using Microsoft.AspNetCore.Mvc;
using Universidad.Data;
using Entidades;
using Microsoft.EntityFrameworkCore;

namespace Universidad.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PacientesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PacientesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Pacientes.AsNoTracking().ToList());
        }

        [HttpPost]
        public IActionResult Post([FromBody] Paciente paciente)
        {
            _context.Pacientes.Add(paciente);
            _context.SaveChanges();
            return Ok(paciente);
        }
    }
}
