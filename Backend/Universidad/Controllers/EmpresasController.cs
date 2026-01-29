using Data;
using Universidad.Core.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Universidad.Entidades;
using Entidades;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpresasController : ControllerBase
    {
        private readonly AppDbContext context;
        public EmpresasController(AppDbContext context)
        {            
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmpresas()
        {
            var empresas = await (from e in context.Empresas where e.Estado != false select e.ToReadDTO()).ToListAsync();
            return Ok(empresas);
        }

        [HttpGet("{NIT}")]
        public async Task<IActionResult> GetEmpresa(string NIT)
        {
            var empresa = await (from e in context.Empresas where e.Estado != false && e.NIT == NIT select e.ToReadDTO()).FirstOrDefaultAsync();
            if (empresa == null) return BadRequest("No se encontro");

            return Ok(empresa);
        }

        [HttpPost]
        public async Task<IActionResult> PostEmpresa(EmpresaCreateDTO empresaDto)
        {
            var existing = await (from e in context.Empresas where e.NIT == empresaDto.NIT select e.ToReadDTO()).FirstOrDefaultAsync();
            if (existing != null) return BadRequest("Ya existe la empresa");

            var empresa = new Empresa
            {
                NIT = empresaDto.NIT,
                Nombre = empresaDto.Nombre,
                RazonSocial = empresaDto.RazonSocial,
                Direccion = empresaDto.Direccion,
                Telefono = empresaDto.Telefono,
                Correo = empresaDto.Correo,
                Estado = true
            };

            await context.Empresas.AddAsync(empresa);
            await context.SaveChangesAsync();

            return Ok($"Se creo registro la empresa");
        }

        [HttpPut("{NIT}")]
        public async Task<IActionResult> PutEmpresa(string NIT, EmpresaUpdateDTO empresaDto)
        {
            var empresaExistente = await (from e in context.Empresas where e.NIT == NIT && e.Estado != false select e).FirstOrDefaultAsync();
            if (empresaExistente == null) return BadRequest("No se encontro la empresa");

            empresaExistente.Nombre = empresaDto.Nombre;
            empresaExistente.RazonSocial = empresaDto.RazonSocial;
            empresaExistente.Direccion = empresaDto.Direccion;
            empresaExistente.Telefono = empresaDto.Telefono;
            empresaExistente.Correo = empresaDto.Correo;

            await context.SaveChangesAsync();

            return Ok("Se actualizo correctamente");
        }


        [HttpDelete("{NIT}")]
        public async Task<IActionResult> DeleteEmpresa(string NIT)
        {
            var empresaExistente = await (from e in context.Empresas where e.NIT == NIT && e.Estado != false select e).FirstOrDefaultAsync();
            if (empresaExistente == null) return BadRequest("No se encontro la empresa");

            empresaExistente.Estado =  false;
            await context.SaveChangesAsync();

            return Ok("Se elimino correctamente");
        }
    }
}