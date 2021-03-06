﻿using FlowRepository.Data.Contracts;
using FlowRepository.Data.Rules;
using FlowRepository.Models.UserRepository;
using FlowRepository.Repositories.Contracts.FlowRepository;
using FlowRepository.Repositories.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using FlowRepository.Models.Const;

namespace FlowRepository.Repositories.Models.FlowRepository
{
    //TODO Repository Needs to be refactored into ISearchable,  Posts and Videos Needs to be refactoring into IContent with abstract methods to return a url based on the content type
    public class UserRepository : DataRepository<FlowCollectionEntities>, IUserRepository
    {
        #region Constructors

        public UserRepository()
            : base()
        {
        }

        public UserRepository(FlowCollectionEntities context)
            : base(context)
        {
        } 

        #endregion

        #region IUserRepository Methods

        public User CreateUser(NewUserDTO newUser, int PriviledgeLevelID)
        {
            //TODO throw validation exception
            if (false == IsValidateNewUser(newUser))
            {
                return null;
            }

            IHash hashRule = new HashRule();
            var passwordHash = hashRule.CreateHash(newUser.Password);

            var user = new User
            {
                Username = newUser.Username,
                PasswordHash = passwordHash,
                Email = newUser.Email,
                FK_PrivilegelevelID = PriviledgeLevelID
            };

            AddVerificationToUser(user, FlowCollectionConsts.VERIFICATION_TYPE_EMAIL);

            Add(user);
            SaveChanges();

            return user;
        }

        //TODO Consider adding a time limit to now allow verfications for forgot password after a few days
        public int VerifyUser(string username, Guid guid, string verificationName)
        {
            var user = GetUserByUsernameQuery(username).Include(x => x.UserVerifications).FirstOrDefault();

            if (user == null)
            {
                return 0;
            }
            //Note once were using enumerations we won't need to load this data
            var verificationType = _context.VerificationTypes.FirstOrDefault(x => x.Name == verificationName);
            var userVerification = user.UserVerifications.Where(x => x.FK_VerficationId == verificationType.ID).OrderByDescending(x => x.Timestamp).FirstOrDefault();

            //don't allow verfication to happen twice check the is verfied 
            if (userVerification.VerifiedGuid == guid && userVerification.isVerified == false)
            {
                userVerification.isVerified = true;
                SaveChanges();
                return user.ID;
            }

            return 0;

        }

        public User GetUserByUsername(string username)
        {
            return GetUserByUsernameQuery(username).FirstOrDefault();
        }

        public List<User> SearchUsers(string[] searchTerms, int pageNumber, int resultsPerPage, int currentUserId = 0)
        {
            return this.GetBaseSearchUserQueryable(searchTerms)
                            .OrderBy(x => x.Created)
                            .ThenBy(x => x.ID)
                            .Skip(pageNumber * resultsPerPage)
                            .Take(resultsPerPage)
                            .ToList();
        }

        #endregion

        #region Protected Methods

        protected IQueryable<User> GetBaseSearchUserQueryable(string[] searchTerms)
        {
            var baseQuery = All<User>();
            foreach (var term in searchTerms)
            {
                baseQuery = baseQuery.Where(x => x.Username.Contains(term));
            }

            return baseQuery;
        }

        protected IQueryable<User> GetUserByUsernameQuery(string username)
        {
            return All<User>().Where(u => u.Username == username);
        }

        protected void AddVerificationToUser(User user, string verificationName)
        {
            var verificationType = this.GetUserVerificationByName(verificationName);
            var userVerification = this.CreateUserVerfication(user, verificationType);
            user.UserVerifications.Add(userVerification);
        }

        protected VerificationType GetUserVerificationByName(string name)
        {
            return _context.VerificationTypes.FirstOrDefault(x => x.Name == name);
        }

        protected UserVerification CreateUserVerfication(User user, VerificationType verificationType)
        {
            var userVerification = new UserVerification
            {
                FK_VerficationId = verificationType.ID,
                Timestamp = DateTime.Now,
                VerifiedGuid = Guid.NewGuid(),
                FK_UserId = user.ID
            };

            return userVerification;
        }

        protected bool IsValidateNewUser(NewUserDTO user)
        {
            return (false == _context.Users.Any(u => u.Username == user.Username || u.Email == user.Email));
        }

        #endregion
    }
}
