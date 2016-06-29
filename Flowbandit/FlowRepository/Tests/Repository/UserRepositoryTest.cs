using FlowRepository.Repositories.Models.FlowRepository;
using FlowRepository.Tests.General;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository.Tests.Repository
{
    public class UserRepositoryTest : TestBase
    {
        #region Test Methods

        [TestMethod]
        public void AddUser()
        {

        }


        #endregion

        #region Test Helper Methods

        protected UserRepository GetRepository()
        {
            var testContext = this.GetDefaultContext();
            return new UserRepository(testContext);
        }

        #endregion
    }
}
