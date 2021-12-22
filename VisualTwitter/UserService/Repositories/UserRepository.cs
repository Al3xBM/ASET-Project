using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using UserService.Data;
using UserService.Helpers;
using UserService.Models;

namespace UserService.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDataContext _dataContext;
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<User> _collection;
        private readonly AppSettings _appSettings;

        public UserRepository(IDataContext context, IOptions<AppSettings> appSettings)
        {
            _dataContext = context;
            _database = _dataContext.getDatabaseConnection("VisualTwitter");
            _collection = _database.GetCollection<User>("Users");
            _appSettings = appSettings.Value;
        }

        public User Authenticate(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

            var user = _collection.Find(user => user.Email == email).FirstOrDefault();

            if (user == null)
                return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        public User Create(User user, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new UserException("Password is required");

            if(_collection.Find(u => u.Email == user.Email).Any())
                throw new UserException("email \"" + user.Email + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _collection.InsertOne(user);

            return user;
        }

        public void Delete(int id) => _collection.FindOneAndDelete(user => user.Id == id);

        public IEnumerable<User> GetAll() => _collection.Find(f => true).ToList();

        public User GetById(int id) => _collection.Find(user => user.Id == id).FirstOrDefault();

        public void Update(User user, string password = null)
        {
            var userPar = _collection.Find(u => u.Id == user.Id).FirstOrDefault(); 
            if (userPar == null)
                throw new UserException("User not found");

            if (!string.IsNullOrWhiteSpace(user.Email) && user.Email != userPar.Email)
            {
                if (_collection.Find(x => x.Email == user.Email).Any())
                    throw new UserException("email " + user.Email + " is already taken");

                userPar.Email = user.Email;
            }

            if (!string.IsNullOrWhiteSpace(user.FirstName))
                userPar.FirstName = user.FirstName;

            if (!string.IsNullOrWhiteSpace(user.LastName))
                userPar.LastName = user.LastName;

            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                userPar.PasswordHash = passwordHash;
                userPar.PasswordSalt = passwordSalt;
            }
        }


        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) 
                throw new ArgumentNullException("password");

            if (string.IsNullOrWhiteSpace(password)) 
                throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) 
                throw new ArgumentNullException("password");

            if (string.IsNullOrWhiteSpace(password)) 
                throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            if (storedHash.Length != 64)
                throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");

            if (storedSalt.Length != 128)
                throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                    if (computedHash[i] != storedHash[i]) 
                        return false;
            }

            return true;
        }

        public string GetJWTToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
