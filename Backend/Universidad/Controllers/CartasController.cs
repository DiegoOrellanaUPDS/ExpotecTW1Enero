using Data;
using Universidad.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Universidad.Core.DTOs;
using Entidades;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartasController : ControllerBase
    {
        private readonly AppDbContext context;
        public CartasController(AppDbContext context)
        {            
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCartas()
        {
            var cartas = await (from c in context.Cartas where c.Estado != "Borrado" select c.ToReadDTO()).ToListAsync();
            return Ok(cartas);
        }

        [HttpGet("{codigo}")]
        public async Task<IActionResult> GetCarta(string codigo)
        {
            var carta = await (from c in context.Cartas where c.Estado != "Borrado" && c.Codigo == codigo select c.ToReadDTO()).FirstOrDefaultAsync();
            if (carta == null) return BadRequest("No se encontro");

            return Ok(carta);
        }

        [HttpPost]
        public async Task<IActionResult> PostCarta(CartaCreateDTO cartaDto)
        {
            var existing = await (from c in context.Cartas where c.FolioCarta == cartaDto.FolioCarta select c.ToReadDTO()).FirstOrDefaultAsync();
            if (existing != null) return BadRequest("Ya existe la carta");

            var carta = new Carta
            {
                Codigo = await GenerarCodigoCarta(),
                EstudianteCI = cartaDto.EstudianteCI,
                NITEmpresa = cartaDto.NITEmpresa,
                FolioCarta = cartaDto.FolioCarta,
                Url = cartaDto.Url,
                Estado = "Pendiente"
            };

            await context.Cartas.AddAsync(carta);
            await context.SaveChangesAsync();

            return Ok($"Se creo correctamente");
        }

        [HttpPut("{codigo}")]
        public async Task<IActionResult> PutCarta(string codigo, CartaUpdateDto cartaDto)
        {
            var cartaExistente = await (from c in context.Cartas where c.Codigo == codigo && c.Estado != "Borrado" select c).FirstOrDefaultAsync();
            if (cartaExistente == null) return BadRequest("No se encontro la carta");

            cartaExistente.EstudianteCI = cartaDto.EstudianteCI;
            cartaExistente.NITEmpresa = cartaDto.NITEmpresa;

            await context.SaveChangesAsync();

            return Ok("Se actualizo correctamente");
        }

        [HttpDelete("{codigo}")]
        public async Task<IActionResult> DeleteCarta(string codigo)
        {
            var cartaExistente = await (from c in context.Cartas where c.Codigo == codigo && c.Estado != "Borrado" select c).FirstOrDefaultAsync();
            if (cartaExistente == null) return BadRequest("No se encontro la carta");

            cartaExistente.Estado = "Borrado";
            await context.SaveChangesAsync();

            return Ok("Se elimino correctamente");
        }


        private async Task<string> GenerarCodigoCarta()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var year = today.Year;
            var month = today.Month;

            var lastCarta = await context.Cartas
                .Where(c => c.FechaGeneracion.Year == year && c.FechaGeneracion.Month == month)
                .OrderByDescending(c => c.Codigo)
                .Select(c => c.Codigo)
                .FirstOrDefaultAsync();

            int sequenceNumber = 1;

            if (lastCarta != null)
            {
                var parts = lastCarta.Split('-');
                
                if (parts.Length == 3 && int.TryParse(parts[2], out int lastSequence))
                {
                    sequenceNumber = lastSequence + 1;
                }
            }
            return $"CAR-{year}{month:D2}-{sequenceNumber:D3}";
        }
    }
}