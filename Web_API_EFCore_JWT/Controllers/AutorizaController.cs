
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using APICatalogo_Seguranca.DTOs;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace APICatalogo_Seguranca.Controllers {

    // api/Autoriza
    [Route("api/[Controller]")]
    [ApiController]
    public class AutorizaController : Controller {


        private readonly UserManager<IdentityUser> _userManager; // Gerenciador de usuários 
        private readonly SignInManager<IdentityUser> _signInManager;// gerenciador de login
        private readonly IConfiguration _configuration; // Para pode ler as informações do appsettings.json


        public AutorizaController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager, IConfiguration configuration) {

            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }


        // Maneira de verificar se a API esta atendendo
        [HttpGet]
        public ActionResult<string> Get() {

            return "AutorizaController ::  Acessado em  : "
               + DateTime.Now.ToLongDateString();
        }



        // Registrar o Usuário
        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody] UsuarioDTO model) {

            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
            //}

            // Registrando usuario com informações passadas no corpo da requisição
            var user = new IdentityUser {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true// Ja confirmando para não precisar enviar solicitação
            };

            var result = await _userManager.CreateAsync(user, model.Password);// Criando o Usuario

            if (!result.Succeeded) {
                return BadRequest(result.Errors);
            }

            await _signInManager.SignInAsync(user, false);
            return Ok(GeraToken(model));
        }


        // Logar usuario
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UsuarioDTO userInfo) {

            //verifica se o modelo é válido
            if (!ModelState.IsValid) {
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
            }

            //verifica as credenciais do usuário e retorna um valor
            
            var result = await _signInManager.PasswordSignInAsync(userInfo.Email,
                userInfo.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded) {

                return Ok(GeraToken(userInfo));

            } else {

                ModelState.AddModelError(string.Empty, "Login Inválido....");
                return BadRequest(ModelState);
            }
        }


        // Geração do Token
        private UsuarioToken GeraToken(UsuarioDTO userInfo) {

            //define declarações do usuário(Não obrigatório) para se tornar mais seguro
            var claims = new[]
            {
                 new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
                 new Claim("meuPet", "pipoca"),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
             };

            //gera uma chave com base em um algoritmo simétrico
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));
            //gera a assinatura digital do token usando o algoritmo Hmac e a chave privada
            var credenciais = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //Tempo de expiracão do token.
            var expiracao = _configuration["TokenConfiguration:ExpireHours"];
            var expiration = DateTime.UtcNow.AddHours(double.Parse(expiracao));

            // classe que representa um token JWT e gera o token
            JwtSecurityToken token = new JwtSecurityToken(
              issuer: _configuration["TokenConfiguration:Issuer"],
              audience: _configuration["TokenConfiguration:Audience"],
              claims: claims,
              expires: expiration,
              signingCredentials: credenciais);

            //retorna os dados com o token e informacoes
            return new UsuarioToken() {
                Authenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
                Message = "Token JWT OK"
            };
        }


    }
}