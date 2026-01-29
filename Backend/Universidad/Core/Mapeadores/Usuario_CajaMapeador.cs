using Entidades;
using Universidad.Core.DTOs;
using Universidad.Entidades;
namespace Universidad.Core.Mapedores
{
    public static class Usuario_CajaMapeador
    {
        public static Usuario_CajaDTO toUsuario_CajaDTO(this Usuario_Caja usuario)
        {
            return new Usuario_CajaDTO()
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Rol = usuario.Rol,
                Token = usuario.Token
            };
        }
    }
}
