using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApiAutores1.DTOs;
using WebApiAutores1.Servicios;

namespace WebApiAutores1.Controllers.V1
{
    [ApiController]
    [Route("api/v1/cuentas")]
    public class CuentasController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly HashService hashService;
        private readonly IDataProtector dataProtector;

        public CuentasController(UserManager<IdentityUser> userManager,
            IConfiguration configuration,
            SignInManager<IdentityUser> signInManager,
            IDataProtectionProvider dataProtectionProvider,
            HashService hashService)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
            this.hashService = hashService;
            dataProtector = dataProtectionProvider.CreateProtector("valor_unico_y_quiza_secreto");
        }

        //[HttpGet("hash/{textoPlano}")]
        //public ActionResult RealizarHash(string textoPlano)
        //{
        //    var resultado1 = hashService.Hash(textoPlano);
        //    var resultado2 = hashService.Hash(textoPlano);
        //    return Ok(new
        //    {
        //        textoPlano = textoPlano,
        //        resultado1 = resultado1,
        //        resultado2 = resultado2
        //    });
        //}

        //[HttpGet("Encriptar")]
        //public ActionResult Encriptar()
        //{
        //    var textoPlano = "Felipe Gavilán";
        //    var textoCifrado = dataProtector.Protect(textoPlano);
        //    var textoDescencriptado = dataProtector.Unprotect(textoCifrado);

        //    return Ok(new {
        //        textoPlano = textoPlano,
        //        textoCifrado = textoCifrado,
        //        textoDescencriptado = textoDescencriptado              
        //    });
        //}

        //[HttpGet("EncriptarPorTiempo")]
        //public ActionResult EncriptarPorTiempo()
        //{
        //    var protectorLimitadoPortiempo = dataProtector.ToTimeLimitedDataProtector();

        //    var textoPlano = "Felipe Gavilán";
        //    var textoCifrado = protectorLimitadoPortiempo.Protect(textoPlano, lifetime: TimeSpan.FromSeconds(5));
        //    Thread.Sleep(6000);
        //    var textoDescencriptado = protectorLimitadoPortiempo.Unprotect(textoCifrado);

        //    return Ok(new
        //    {
        //        textoPlano = textoPlano,
        //        textoCifrado = textoCifrado,
        //        textoDescencriptado = textoDescencriptado
        //    });
        //}


        [HttpPost("registrar", Name = "registrarUsuario")]
        public async Task<ActionResult<RespuestaAutenticacion>> Registrar(CredencialUsuarios credencialUsuarios)
        {
            var usuario = new IdentityUser
            {
                UserName = credencialUsuarios.Email,
                Email = credencialUsuarios.Email
            };
            var resultado = await userManager.CreateAsync(usuario, credencialUsuarios.Password);

            if (resultado.Succeeded)
            {
                return await ConstruirToken(credencialUsuarios);
            }
            else
            {
                return BadRequest(resultado.Errors);
            }
        }

        [HttpPost("login", Name = "loginUsuario")]
        public async Task<ActionResult<RespuestaAutenticacion>> login(CredencialUsuarios credencialUsuarios)
        {
            var resultado = await signInManager.PasswordSignInAsync(credencialUsuarios.Email,
                credencialUsuarios.Password, isPersistent: false, lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                return await ConstruirToken(credencialUsuarios);
            }
            else
            {
                return BadRequest(credencialUsuarios);
            }
        }

        [HttpGet("RenovarToken", Name = "renovarToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<RespuestaAutenticacion>> Renovar()
        {
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var credencialUsuario = new CredencialUsuarios()
            {
                Email = email
            };

            return await ConstruirToken(credencialUsuario);
        }

        private async Task<RespuestaAutenticacion> ConstruirToken(CredencialUsuarios credencialUsuarios)
        {
            var claims = new List<Claim>()
            {
                new Claim("email", credencialUsuarios.Email),
                new Claim("lo que sea", "cualquier cosa")
            };

            var usuario = await userManager.FindByEmailAsync(credencialUsuarios.Email);
            var claimsDB = await userManager.GetClaimsAsync(usuario);

            claims.AddRange(claimsDB);

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["llavejwt"]));
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddMinutes(30);

            var securityToken = new JwtSecurityToken(issuer: null, audience: null,
                claims: claims, expires: expiracion, signingCredentials: creds);

            return new RespuestaAutenticacion()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiracion = expiracion
            };
        }

        [HttpPost("HacerAdmin", Name = "hacerAdmin")]
        public async Task<ActionResult> HacerAdmin(EditarAdminDTO editarAdminDTO)
        {
            var usuario = await userManager.FindByEmailAsync(editarAdminDTO.Email);
            await userManager.AddClaimAsync(usuario, new Claim("esAdmin", "1"));
            return NoContent();
        }

        [HttpPost("RemoverAdmin", Name = "removerAdmin")]
        public async Task<ActionResult> RemoverAdmin(EditarAdminDTO editarAdminDTO)
        {
            var usuario = await userManager.FindByEmailAsync(editarAdminDTO.Email);
            await userManager.RemoveClaimAsync(usuario, new Claim("esAdmin", "1"));
            return NoContent();
        }
    }
}
