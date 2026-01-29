using Data;
using Universidad.Core.DTOs;
using Universidad.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Entidades;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PracticasProfesionalesController : ControllerBase
    {
        private readonly AppDbContext context;
        public PracticasProfesionalesController(AppDbContext context)
        {            
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetPracticas()
        {
            var practicas = await (from pp in context.PracticasProfesionales where pp.Estado != "Borrado" select pp.ToReadDTO()).ToListAsync();
            return Ok(practicas);
        }

        [HttpGet("{Codigo}")]
        public async Task<IActionResult> GetPractica(string codigo)
        {
            var practica = await (from pp in context.PracticasProfesionales where pp.Estado != "Borrado" && pp.Codigo == codigo select pp.ToReadDTO()).FirstOrDefaultAsync();

            if(practica == null) return BadRequest("No se encontro la practica");

            return Ok(practica);
        }

        [HttpPost]
        public async Task<IActionResult> PostPractica(PracticaProfesionalCreateDTO practicaDto)
        {
            var tieneProcesoActivo = await context.PracticasProfesionales
                .AnyAsync(pp => pp.EstudianteCI == practicaDto.EstudianteCI && 
                        (pp.Estado == "Pendiente" || pp.Estado == "En Curso"));

            if (tieneProcesoActivo) 
                return BadRequest("El estudiante ya tiene una práctica pendiente o en curso.");

            if (practicaDto.FechaInicio >= practicaDto.FechaFin)
                return BadRequest("La fecha de inicio no puede ser mayor o igual a la fecha de fin.");

            if (practicaDto.HorasRequeridas <= 0)
                return BadRequest("Las horas requeridas deben ser mayores a 0.");

            var nuevaPractica = new PracticaProfesional
            {
                Codigo = await GenerarCodigoPractica(),
                EstudianteCI = practicaDto.EstudianteCI,
                NITEmpresa = practicaDto.NITEmpresa,
                FechaInicio = practicaDto.FechaInicio,
                FechaFin = practicaDto.FechaFin,
                HorasRequeridas = practicaDto.HorasRequeridas,
                AreaDepartamento = practicaDto.AreaDepartamento,
                Estado = "Pendiente",
            };

            await context.PracticasProfesionales.AddAsync(nuevaPractica);
            await context.SaveChangesAsync();

            return Ok("Se registro exitosamente");
        }

        [HttpPut]
        public async Task<IActionResult> PutPractica(string codigo, PracticaProfesionalUpdateDTO practicaDto)
        {
            var practicaExistente = await (from pp in context.PracticasProfesionales where pp.Codigo == codigo && pp.Estado != "Borrado" select pp).FirstOrDefaultAsync();
            if (practicaExistente == null) return BadRequest("No se encontro la Practica");

            practicaExistente.EstudianteCI = practicaDto.EstudianteCI;
            practicaExistente.NITEmpresa = practicaDto.NITEmpresa;
            practicaExistente.FechaInicio = practicaDto.FechaInicio;
            practicaExistente.FechaFin = practicaDto.FechaFin;
            practicaExistente.HorasRequeridas = practicaDto.HorasRequeridas;
            practicaExistente.AreaDepartamento = practicaDto.AreaDepartamento;

            await context.SaveChangesAsync();

            return Ok("Actualizado correctamente");
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePractica(string codigo)
        {
            var practicaExistente = await (from pp in context.PracticasProfesionales where pp.Codigo == codigo && pp.Estado != "Borrado" select pp).FirstOrDefaultAsync();
            if (practicaExistente == null) return BadRequest("No se encontro la Practica");

            practicaExistente.Estado = "Borrado";
            
            await context.SaveChangesAsync();

            return Ok("Borrado correctamente");
        }

        private async Task<string> GenerarCodigoPractica()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var year = today.Year;
            var month = today.Month;

            var lastPractica = await context.PracticasProfesionales
                .Where(p => p.FechaRegistro.Year == year && p.FechaRegistro.Month == month)
                .OrderByDescending(p => p.Codigo)
                .Select(p => p.Codigo)
                .FirstOrDefaultAsync();

            int sequenceNumber = 1;

            if (lastPractica != null)
            {
                var parts = lastPractica.Split('-');
                
                if (parts.Length == 3 && int.TryParse(parts[2], out int lastSequence))
                {
                    sequenceNumber = lastSequence + 1;
                }
            }

            // Retorna algo como: PRC-202405-001
            return $"PRC-{year}{month:D2}-{sequenceNumber:D3}";
        }
    }
}