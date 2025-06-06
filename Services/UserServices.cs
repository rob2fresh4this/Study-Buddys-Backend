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

        public async Task<List<UserInfoDto>> GetAllUsersAsync()
        {
            return await _dataContext.Users
                .Select(user => new UserInfoDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    OwnedCommunitys = user.OwnedCommunitys ?? new List<int>(),
                    JoinedCommunitys = user.JoinedCommunitys ?? new List<int>(),
                    CommunityRequests = user.CommunityRequests ?? new List<int>()
                })
                .ToListAsync();
        }

        public async Task<bool> RegisterUser(UserDTO user)
        {
            if (await DoseUserExist(user.Username)) return false;
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

        private static PasswordDTO HashPassword(string password)
        {
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

        public async Task<string> LoginUser(UserDTO user)
        {
            UserModels userToLogin = await GetByUsername(user.Username);
            if (userToLogin == null) return null;
            if (!VerifyPassword(user.Password, userToLogin.Salt, userToLogin.Hash)) return null;
            return GenerateJWTToken(new List<Claim>());
        }

        private async Task<UserModels> GetByUsername(string username)
        {
            return await _dataContext.Users.SingleOrDefaultAsync(x => x.Username == username);
        }

        public string serverUrl = "https://study-buddys-backend.azurewebsites.net";
        // public string serverUrl = "https://localhost:5233/"; // Localhost URL for testing

        private string GenerateJWTToken(List<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken(
                issuer: serverUrl,
                audience: serverUrl,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signinCredentials
            );
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }



        private bool VerifyPassword(string password, string salt, string hash)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);
            using (var deryveBytes = new Rfc2898DeriveBytes(password, saltBytes, 310000, HashAlgorithmName.SHA256))
            {
                return Convert.ToBase64String(deryveBytes.GetBytes(32)) == hash;
            }
        }

        public async Task<UserModels> GetUserByIdAsync(int id)
        {
            return await _dataContext.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> EditUserCommunitiesAsync(int userId, List<int>? owned, List<int>? joined, List<int>? requests)
        {
            // Retrieve the user by ID
            UserModels user = await GetUserByIdAsync(userId);
            if (user == null) return false;

            // Update the user's communities only if the lists are provided
            if (owned != null)
                user.OwnedCommunitys = owned;
            if (joined != null)
                user.JoinedCommunitys = joined;
            if (requests != null)
                user.CommunityRequests = requests;

            // Mark the user entity as modified and save changes
            _dataContext.Users.Update(user);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<bool> AddCommunityToUserAsync(int userId, List<int>? owned, List<int>? joined, List<int>? requests)
        {
            // Ensure the user exists
            UserModels user = await GetUserByIdAsync(userId);
            if (user == null) return false;

            // Add the communities to the user if they are not null
            if (owned != null && owned.Any())
            {
                user.OwnedCommunitys.AddRange(owned);
            }
            if (joined != null && joined.Any())
            {
                user.JoinedCommunitys.AddRange(joined);
            }
            if (requests != null && requests.Any())
            {
                user.CommunityRequests.AddRange(requests);
            }

            // Update the user and save changes to the database
            _dataContext.Users.Update(user);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<bool> RemoveCommunityFromUserAsync(int userId, int communityId)
        {
            // Retrieve the user by ID
            var user = await GetUserByIdAsync(userId);

            if (user == null) return false; // If user doesn't exist, return false

            // Initialize the lists if they are null
            user.OwnedCommunitys ??= new List<int>();
            user.JoinedCommunitys ??= new List<int>();
            user.CommunityRequests ??= new List<int>();

            // Remove the community from the user's lists (if it exists)
            user.OwnedCommunitys.Remove(communityId);
            user.JoinedCommunitys.Remove(communityId);
            user.CommunityRequests.Remove(communityId);

            // Save changes to the database
            _dataContext.Users.Update(user);

            // Return true if changes were saved successfully, otherwise false
            return await _dataContext.SaveChangesAsync() > 0;
        }


        public async Task<UserInfoDto> GetAllUserInfoAsync(int userId)
        {
            var user = await _dataContext.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            if (user == null)
                return null;  // User not found

            // Map the user data to the UserInfoDto
            return new UserInfoDto
            {
                Id = user.Id,
                Username = user.Username,
                OwnedCommunitys = user.OwnedCommunitys ?? new List<int>(),
                JoinedCommunitys = user.JoinedCommunitys ?? new List<int>(),
                CommunityRequests = user.CommunityRequests ?? new List<int>()
            };
        }



    }
}