using FlowRepository.Data.Contracts;
using FlowRepository.Data.Rules;
using FlowRepository.Models.UserRepository;
using FlowRepository.Repositories.Contracts.FlowRepository;
using FlowRepository.Repositories.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository.Repositories.Models.FlowRepository
{
    public class UserRepository : DataRepository<FlowCollectionEntities>, IUserRepository
    {
        public User GetUserByUsername(string username)
        {
            return All<User>().FirstOrDefault(u => u.Username.ToLower() == username.ToLower());
        }

        public User CreateUser(NewUserDTO newUser)
        {
            if(false == IsValidateNewUser(newUser))
            {
                return null;
            }

            IHash hashRule = new HashRule();
            var passwordHash = hashRule.CreateHash(newUser.Password);

            var user = new User
            {
                Username = newUser.Username,
                PasswordHash = passwordHash,
                Email = newUser.Email
            };

            Add(user);
            SaveChanges();

            return user;
        }

        protected bool IsValidateNewUser(NewUserDTO user)
        {
            return (false == _context.Users.Any(u => u.Username == user.Username || u.Email == u.Email));
        }
    }
}
