using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Data;
using UserService.Models;

namespace UserService.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;

        public UserRepository(DataContext context)
        {
            _dataContext = context;
        }

        public User Authenticate(string email, string password)
        {
            throw new NotImplementedException();
        }

        public User Create(User user, string password)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public User GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(User user, string password = null)
        {
            throw new NotImplementedException();
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            throw new NotImplementedException();
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            throw new NotImplementedException();
        }
    }
}
