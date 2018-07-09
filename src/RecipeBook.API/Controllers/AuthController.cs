using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RecipeBook.API.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RecipeBook.API.Controllers
{
    [Authorize]
    [Route("auth/session")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IOptions<SignCredentials> signingCred;
        public AuthController(IOptions<SignCredentials> signingCred)
        {
            this.signingCred = signingCred;
        }

        [AllowAnonymous]
        [HttpPost("authorize")]
        public async Task<IActionResult> Authorize([FromForm]SessionSignIn signIn)
        {
            await Task.CompletedTask;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: replace with database records
            if (signIn.Username == "test" && signIn.Password == "test")
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Sid, 1.ToString()),
                    new Claim(ClaimTypes.Name, "Test"),
                    new Claim(ClaimTypes.GivenName, "Test")
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingCred.Value.TokenSecret));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: signingCred.Value.TokenAuthority,
                    audience: signingCred.Value.TokenAuthority,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }

            return BadRequest("Could not verify username and password");
        }

        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromForm]SessionSignUp signUp)
        {
            await Task.CompletedTask;

            return BadRequest("Signup not implemented yet");
        }
    }
}