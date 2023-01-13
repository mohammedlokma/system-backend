using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using system_backend.Helpers;
using system_backend.Models;
using system_backend.Models.Dtos;

namespace system_backend.Services
{
    public class AuthServices : IAuthServices
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;
        private readonly IMapper _mapper;

        public AuthServices(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, IOptions<JWT> jwt)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
            _mapper = mapper;
        }

        public async Task<AuthModel>RegisterAsync(RegisterModel model)
        {
            if (await _userManager.FindByNameAsync(model.UserName) is not null)
                return new AuthModel { Message = "اسم المستخدم موجود بالفعل!" };
            if(await _userManager.Users.FirstOrDefaultAsync(x => x.UserDisplayName == model.UserDisplayName) is not null)
                return new AuthModel { Message = "UserDisplayName is already registered " };

            var user = _mapper.Map<ApplicationUser>(model);

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Empty;

                foreach (var error in result.Errors)
                    errors += $"{error.Description},";

                return new AuthModel { Message = errors };
            }

            await _userManager.AddToRoleAsync(user, model.Role);

            var jwtSecurityToken = await CreateJwtToken(user);

            return new AuthModel
            {
                UserId = user.Id,
                Username = user.UserName,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Role = new List<string>{model.Role},
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
               
            };
        }
        public async Task<AuthModel> LoginAsync(LoginModel model)
        {
            var authModel = new AuthModel();
            

            var user = await _userManager.FindByNameAsync(model.userName);

            if (user is null || !await _userManager.CheckPasswordAsync(user, model.password))
            {
                authModel.Message = "البيانات التي أدخلتها غير صحيحة";
                return authModel;
            }

            var jwtSecurityToken = await CreateJwtToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);

            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.Username = user.UserName;
            authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authModel.Role = rolesList.ToList();

            return authModel;
        }

       

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));
            var iat= new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString();
            var claims = new[]
            {
               new Claim("userName", user.UserName),
               new Claim("userId", user.Id),
               new Claim(JwtRegisteredClaimNames.Iat, iat)

            }
            .Union(userClaims)
            .Union(roleClaims);
            

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
        }
}
