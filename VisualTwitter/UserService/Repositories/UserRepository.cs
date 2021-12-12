using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using UserService.Data;
using UserService.Helpers;
using UserService.Models;

namespace UserService.Repositories
{
    public class UserRepository : IUserRepository
    {
        //private readonly DataContext _dataContext;
        private readonly IDataContext _dataContext;
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<User> _collection;
        public UserRepository(IDataContext context)
        {
            _dataContext = context;
            _database = _dataContext.getDatabaseConnection("VisualTwitter");
            _collection = _database.GetCollection<User>("Users");
        }

        public User Authenticate(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;
            var user = _collection.Find(user => user.Email == email).FirstOrDefault();
            //var user = _dataContext.Users.SingleOrDefault(x => x.Email == email);

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
            //if (_dataContext.Users.Any(x => x.Email == user.Email))
                throw new UserException("email \"" + user.Email + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            //_dataContext.Users.Add(user);
            //_dataContext.SaveChanges();
            _collection.InsertOne(user);

            return user;
        }

        public void Delete(int id)
        {
            /*var user = _collection.Find(user => user.Id == id).FirstOrDefault();
            //var user = _dataContext.Users.Find(id);
            if (user != null)
            {
                //_dataContext.Users.Remove(user);
                //_dataContext.SaveChanges();
            }*/
            _collection.FindOneAndDelete(user => user.Id == id);
        }

        public IEnumerable<User> GetAll()
        {
            //return _dataContext.Users;
            return _collection.Find(f => true).ToList();
        }

        public User GetById(int id)
        {
            //return _dataContext.Users.Find(id);
            return _collection.Find(user => user.Id == id).FirstOrDefault();
        }

        public void Update(User user, string password = null)
        {
            //var userPar = _dataContext.Users.Find(user.Id);
            var userPar= _collection.Find(u => u.Id == user.Id).FirstOrDefault(); 
            if (userPar == null)
                throw new UserException("User not found");

            // update email if it has changed
            if (!string.IsNullOrWhiteSpace(user.Email) && user.Email != userPar.Email)
            {
                // throw error if the new email is already taken
                if (_collection.Find(x => x.Email == user.Email).Any())
                    throw new UserException("email " + user.Email + " is already taken");

                userPar.Email = user.Email;
            }

            // update user properties if provided
            if (!string.IsNullOrWhiteSpace(user.FirstName))
                userPar.FirstName = user.FirstName;

            if (!string.IsNullOrWhiteSpace(user.LastName))
                userPar.LastName = user.LastName;

            // update password if provided
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                userPar.PasswordHash = passwordHash;
                userPar.PasswordSalt = passwordSalt;
            }

            //_dataContext.Users.Update(userPar);
            //_dataContext.SaveChanges();
        }


        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}
