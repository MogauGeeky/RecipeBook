using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RecipeBook.API.Models;
using RecipeBook.Data.Manager;
using RecipeBook.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RecipeBook.API.Controllers
{
    [Authorize]
    [Route("auth/session")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IRecipeBookDataManager recipeBookDataManager;
        private readonly IOptions<SignCredentials> signingCred;
        public AuthController(IRecipeBookDataManager recipeBookDataManager, IOptions<SignCredentials> signingCred)
        {
            this.recipeBookDataManager = recipeBookDataManager;
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

            // check if user exists
            var user = (await recipeBookDataManager.Users.GetItemsAsync(c => c.Username == signIn.Username)).FirstOrDefault();
            
            if(user == null)
            {
                return BadRequest("Invalid username and password");
            }

            // verify user password
            if(!IsPasswordValid(user, signIn.Password))
            {
                return BadRequest("Invalid username and password");
            }

            // if all is well, generate a token
            var token = GenerateToken(user);

            return Ok(new
            {
                token
            });
        }

        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromForm]SessionSignUp signUp)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // check if username exists
            if ((await recipeBookDataManager.Users.GetItemsAsync(c => c.Username == signUp.Username)).Any())
            {
                return BadRequest("Username already exists");
            }

            var passwordHash = HashPassword(signUp.Password);

            var newUser = new RecipeUser
            {
                FullName = signUp.Fullname,
                Username = signUp.Username,
                PasswordSecret = passwordHash.Item1,
                PasswordHash = passwordHash.Item2
            };

            var newUserId = await recipeBookDataManager.Users.CreateItemAsync(newUser);
            newUser.Id = newUserId;

            var token = GenerateToken(newUser);

            return Ok(new
            {
                token
            });
        }

        private Tuple<string, string> HashPassword(string passwordString)
        {
            // generate a 128-bit salt using a secure PRNG
            byte[] saltBytes = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }

            var passwordSecret = Convert.ToBase64String(saltBytes);

            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string passwordHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: passwordString,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return new Tuple<string, string>(passwordSecret, passwordHash);
        }

        private bool IsPasswordValid(RecipeUser recipeUser, string password)
        {
            var userSalt = Convert.FromBase64String(recipeUser.PasswordSecret);

            string passwordHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: userSalt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return passwordHash == recipeUser.PasswordHash;
        }

        private string GenerateToken(RecipeUser recipeUser)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Sid, recipeUser.Id),
                new Claim(ClaimTypes.Name, recipeUser.Username),
                new Claim(ClaimTypes.GivenName, recipeUser.FullName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingCred.Value.TokenSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: signingCred.Value.TokenAuthority,
                audience: signingCred.Value.TokenAuthority,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}