using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Universidad.Services
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService()
        {
            // mis credenciales directas (wilson yucra)
            _cloudinary = new Cloudinary(new Account(
                "dzk9as6ep", 
                "167194994214489", 
                "4oCyFKB3P-x_4e22_AGwpvnpnMY"));
        }

        public async Task<string> SubirArchivoAsync(IFormFile archivo)
        {
            if (archivo == null) return null;

            using var stream = archivo.OpenReadStream();
            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(archivo.FileName, stream)
            };

            var result = await _cloudinary.UploadAsync(uploadParams);
            
            return result.SecureUrl.ToString();
        }
    }
}