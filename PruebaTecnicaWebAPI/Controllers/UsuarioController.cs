using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using PruebaTecnicaWebAPI.Models;
using PruebaTecnicaWebAPI.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace PruebaTecnicaWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IMongoCollection<Usuario> _usuarios;

        private readonly IConfiguration _configuration;

        public UsuarioController(IConfiguration configuration, MongoDBService mongoDBService)
        {
            _configuration = configuration;
            _usuarios = mongoDBService.Database?.GetCollection<Usuario>("Usuarios"); // Reemplaza "Usuarios" con el nombre de tu colección
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginDto model)
        {
            // 1. Autenticar al usuario contra la base de datos de MongoDB
            var usuario = await _usuarios.Find(u => u.Nombre == model.Nombre).FirstOrDefaultAsync();

            if (usuario == null || !VerifyPassword(model.Contraseña, usuario.Contraseña)) // Implementa VerifyPassword
            {
                return Unauthorized();
            }

            var issuer = _configuration["JwtConfig:Issuer"];
            var audience = _configuration["JwtConfig:Audience"];
            var key = _configuration["JwtConfig:Key"];
            var tokenValidityMins = _configuration.GetValue<int>("JwtConfig:TokenValidityMins");
            var tokenExpirityTimeStamp = DateTime.UtcNow.AddMinutes(tokenValidityMins);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Name, model.Nombre)
                }),
                Expires = tokenExpirityTimeStamp,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    SecurityAlgorithms.HmacSha512Signature),
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor); 
            var accessToken = tokenHandler.WriteToken(securityToken);

            return Ok(new { Token = tokenHandler.WriteToken(securityToken) });
        }

        // Ejemplo de cómo verificar la contraseña (deberías usar un hashing seguro como bcrypt)
        private bool VerifyPassword(string providedPassword, string storedHash)
        {
            // ¡Implementación de verificación de contraseña segura!
            // Esto es solo un ejemplo inseguro y NO debe usarse en producción.
            return providedPassword == storedHash;
        }
    }

    public class UsuarioLoginDto
    {
        public string Nombre { get; set; }
        public string Contraseña { get; set; }
    }

    // Asegúrate de tener tu modelo de usuario (Usuario) con las propiedades necesarias (Id, Nombre, Password, etc.)
   
}
