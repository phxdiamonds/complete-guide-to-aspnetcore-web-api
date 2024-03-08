using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using my_books.Data;
using my_books.Data.Models;
using my_books.Data.ViewModel.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace my_books.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        // inject the services that we are going to use

        private readonly UserManager<ApplicationUser> _userManager; // <> this is where i store the user information in database

        private readonly RoleManager<IdentityRole> _roleManager; // <> this is where i store the roles information in database

        private readonly AppDbContext _context;

        private readonly IConfiguration _configuration;

        //refresh tokens

        private readonly TokenValidationParameters _tokenValidationParameters;

        public AuthenticationController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext context, IConfiguration configuration, TokenValidationParameters tokenValidationParameters)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _configuration = configuration;
            _tokenValidationParameters = tokenValidationParameters;
        }

        [HttpPost("register-user")]
        public async Task<IActionResult> Register([FromBody] RegisterVM payload)
        {
            //to check whether the user provided username and password
            if (!ModelState.IsValid)
            {
                return BadRequest("Please Provide all the required fields");
            }

            //check user is exist or not

            var userExist = await _userManager.FindByEmailAsync(payload.Email);

            if ((userExist != null))
            {
                return BadRequest($"User {payload.Email} already exist");
            }

            //create user
            var user = new ApplicationUser()
            {
                Email = payload.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = payload.UserName
            };
            
           //generate password and store in database

            var result = await _userManager.CreateAsync(user, payload.Password);

            if (!result.Succeeded)
            {
                return BadRequest("user could not be created");
            }

            //Add user to the role
            switch (payload.Role)
            {  
                case"Admin":
                    await _userManager.AddToRoleAsync(user, UserRoles.Admin);
                    break;
                case "Publisher":
                    await _userManager.AddToRoleAsync (user, UserRoles.Publisher);
                    break;
                case "Author":
                    await _userManager.AddToRoleAsync(user, UserRoles.Author);
                    break;
                default:
                    await _userManager.AddToRoleAsync(user, UserRoles.User);
                    break;



            }

            return Created(nameof(Register), $"User {payload.Email} Created");
        }

        private async Task<AuthResultVM> GenerateJwtTokenAsync(ApplicationUser user, string existingRefreshToken)
        {
            //adding authentication claims which are properties of application user
            //You can add public private or register claims
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            //Add user roles and claim types associated to

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach(var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            //Getting auth signin key

            var authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));

            //Create the token
            var token = new JwtSecurityToken(

                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.UtcNow.AddMinutes(10), //here we kept 1 min, but typical time is 5-10 mins
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            //form these token generate jwt token

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token); // here the writetoken is used to jwt into a compact serialization format

            //Generate the refresh tokem
            //Need to define the jwt id

            var refreshToken = new RefreshToken();

            if(string.IsNullOrEmpty(existingRefreshToken))
            {
                 refreshToken = new RefreshToken()
                {
                    JwtId = token.Id,
                    IsRovoked = false,
                    UserId = user.Id,
                    DateAdded = DateTime.UtcNow,
                    DateExpire = DateTime.UtcNow.AddMonths(6),
                    Token = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString() //Need to generate a secure string
                };
                await _context.RefreshTokens.AddAsync(refreshToken); // adding to database
                await _context.SaveChangesAsync();
            }



            //construct the response as well

            var response = new AuthResultVM()
            {
                Token = jwtToken,
                RefreshToken = String.IsNullOrEmpty(existingRefreshToken) ? refreshToken.Token : existingRefreshToken,
                ExpiresAt = token.ValidTo //valid to comes from the expiers of the token
            };

            return response;
        }

           [HttpPost("login-user")]
        public async Task<IActionResult> Login([FromBody] LoginVM payload)
        {
            //to check whethr the user provided username and password

            if (!ModelState.IsValid)
            {
                return BadRequest("Please Provide all the required fields");
            }

            //check if user exist
            var loginUser = await _userManager.FindByEmailAsync(payload.Email);

            //check password
            if ((loginUser != null && await _userManager.CheckPasswordAsync(loginUser, payload.Password)))
            {
                var tokenValue = await GenerateJwtTokenAsync(loginUser, "");
                return Ok(tokenValue);
            }
            return Unauthorized();
            
            
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequestVM payload)
        {
            try
            {
                //here before we need to generate a new token  we need to verify that the existing token is generated by our api
                var result = await VerifyAndGenerateTokenAsync(payload);

                if ((result == null)) return BadRequest("Invalid Tokens");

                return Ok(result);
               
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        private async Task<AuthResultVM> VerifyAndGenerateTokenAsync(TokenRequestVM payload)
        {
            //need to have some validations  and before that we need to verify the jwt security token handlers

            var jwtTokenHandler = new JwtSecurityTokenHandler();

            try
            {

                //validations

                // 1, check the jwt token format

                var tokenInVerification = jwtTokenHandler.ValidateToken(payload.Token, _tokenValidationParameters, out var validatedToken);

                //2 check the encryption algorith

                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                    if ((result == false)) return null;

                }

                //check 3 validate expiry date

                var utcExpiryDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);

                if (expiryDate > DateTime.UtcNow)
                {
                    throw new Exception("Token has not yet expired");
                }


                //check 4 refresh token exists in database

                var dbRefreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(n => n.Token == payload.RefreshToken);

                if (dbRefreshToken == null)
                {
                    throw new Exception("Refresh token does not exist");
                }
                else
                {
                    //check 5 validate Id
                    var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                    if (dbRefreshToken.JwtId != jti) throw new Exception("Refresh token does not match");

                    //check 6 refresh token expiration
                    if (dbRefreshToken.DateExpire <= DateTime.UtcNow)
                    {
                        throw new Exception(" Your Refresh token has expired, Please re-authenticate");
                    }

                    //check 7 refresh token is revoked
                    if (dbRefreshToken.IsRovoked) throw new Exception("Refresh token has been revoked");

                    //Generate new token (with existing refresh token)

                    var dbUserData = await _userManager.FindByIdAsync(dbRefreshToken.UserId);


                    var newTokenResponse = GenerateJwtTokenAsync(dbUserData, payload.RefreshToken);

                    return await newTokenResponse;

                }
            }
            catch (SecurityTokenExpiredException ex)
            {


                var dbRefreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(n => n.Token == payload.RefreshToken);

                //Generate new token (with existing refresh token)

                var dbUserData = await _userManager.FindByIdAsync(dbRefreshToken.UserId);


                var newTokenResponse = GenerateJwtTokenAsync(dbUserData, payload.RefreshToken);

                return await newTokenResponse;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }




        }

        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp);

            return dateTimeVal;
        }


    }
}
