using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MagicVilla_VillaAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly VillaDbContext _db;
        private string secretKey;

        public UserRepository(VillaDbContext db, IConfiguration configuration)
        {
            _db = db;

            //accessing secret key from appsettings config
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }
        public bool IsUniqueUser(string username)
        {
            var usr = _db.LocalUsers.FirstOrDefault(u => u.UserName == username);

            if(usr == null)
            {
                return true;
            }

            return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequest)
        {
            var usr = _db.LocalUsers.FirstOrDefault(u=>u.UserName == loginRequest.UserName &&  u.Password == loginRequest.Password);

            if(usr == null) 
            {
                return new LoginResponseDto()
                {
                    Token = "", 
                    User = null
                }; ;
            }

            //generating JWT token
            //when we have to generate JWT, we need secret key
            //using that secret key our token will be encrypted
            //with secret key we authenticate whether token was generated by API and if that token was valid or not

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);   //get encoded secret key in byte array

            //token descriptor contains everything like claims - name of user, role...
            //also when does it expire, how to encript it

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usr.Id.ToString()),
                    new Claim(ClaimTypes.Role, usr.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            //JSON web tokens (JWTs) claims are pieces of information asserted about a subject.
            //For example, an ID token (which is always a JWT ) can contain a claim called name
            //that asserts that the name of the user authenticating is "John Doe".

            var token = tokenHandler.CreateToken(tokenDescriptor); //generating token

            LoginResponseDto loginResponse = new LoginResponseDto()
            {
                Token = tokenHandler.WriteToken(token), //generating token which we can use / serializing
                User = usr
            };

            return loginResponse;
        }

        public async Task<LocalUser> Register(RegistrationRequestDto registrationRequest)
        {
            LocalUser user = new LocalUser() { 
                Name = registrationRequest.Name,
                UserName = registrationRequest.UserName,
                Password = registrationRequest.Password,
                Role = registrationRequest.Role
            };

            _db.LocalUsers.Add(user);
           await  _db.SaveChangesAsync();

            user.Password = "";
            return user;
        }
    }
}
