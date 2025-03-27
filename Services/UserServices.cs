using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Study_Buddys_Backend.Context;
using Study_Buddys_Backend.Models;
using Study_Buddys_Backend.Models.DTOS;

namespace Study_Buddys_Backend.Services
{
    public class UserServices
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _config;

        public UserServices(DataContext dataContext, IConfiguration config)
        {
            _dataContext = dataContext;
            _config = config;
        }

        public async Task<bool> RegisterUser(UserDTO user)
        {
            if(await DoseUserExist(user.Username)) return false;
            UserModels addUser = new();
            PasswordDTO encryptedPassword = HashPassword(user.Password);
            addUser.Username = user.Username;
            addUser.Hash = encryptedPassword.Hash;
            addUser.Salt = encryptedPassword.Salt;

            await _dataContext.Users.AddAsync(addUser);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<bool> DoseUserExist(string username)
        {
            return await _dataContext.Users.SingleOrDefaultAsync(x => x.Username == username) != null;
        }

        private static PasswordDTO HashPassword(string password){
            byte[] saltBytes = RandomNumberGenerator.GetBytes(64);
            string salt = Convert.ToBase64String(saltBytes);

            string hash = "";
            using (var deryveBytes = new Rfc2898DeriveBytes(password, saltBytes, 310000, HashAlgorithmName.SHA256))
            {
                byte[] hashBytes = deryveBytes.GetBytes(32);
                hash = Convert.ToBase64String(hashBytes);
            }

            return new PasswordDTO { Salt = salt, Hash = hash };
        }

        public async Task<string> LoginUser(UserDTO user){
            UserModels userToLogin = await GetByUsername(user.Username);
            if(userToLogin == null) return null;
            if(!VerifyPassword(user.Password, userToLogin.Salt, userToLogin.Hash)) return null;
            return GenerateJWTToken(new List<Claim>());
        }

        private async Task<UserModels> GetByUsername(string username){
            return await _dataContext.Users.SingleOrDefaultAsync(x => x.Username == username);
        }

        private string GenerateJWTToken (List<Claim> claims){
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken(
                issuer: "https://localhost:5001",
                audience: "https://localhost:5001",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signinCredentials
            );
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private bool VerifyPassword (string password, string salt, string hash){
            byte[] saltBytes = Convert.FromBase64String(salt);
            using (var deryveBytes = new Rfc2898DeriveBytes(password, saltBytes, 310000, HashAlgorithmName.SHA256))
            {
                return Convert.ToBase64String(deryveBytes.GetBytes(32)) == hash;
            }
        }

        public async Task<UserInfoDTO> GetUserInfoByUsername(string username){
            UserModels user = await GetByUsername(username);
            return new UserInfoDTO { Username = user.Username, Id = user.Id };
        }
    }
}