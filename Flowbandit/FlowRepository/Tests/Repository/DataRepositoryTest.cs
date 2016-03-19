using FlowRepository.Repositories.Models.Base;
using FlowRepository.Tests.General;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using System.Linq.Expressions;

namespace FlowRepository.Tests.Repository
{
    [TestClass]
    public class DataRepositoryTest : TestBase
    {
        #region Test Methods 

        [TestMethod]
        public void All_Query_Test()
        {
            var dataRepo = this.GetDataRepository();

            var postsList = dataRepo.All<Post>().ToList();
            postsList.ShouldAllBeEquivalentTo(testPosts);
        }

        [TestMethod]
        public void Add_Query_Test()
        {
            var dataRepo = this.GetDataRepository();
            dataRepo.Add<Post>(this.GetTestPost());
            PostMock.Verify(m => m.Add(It.IsAny<Post>()), Times.Once());
        }

        [TestMethod]
        public void Remove_Query_Test()
        {
            var dataRepo = this.GetDataRepository();
            dataRepo.Delete(this.GetTestPost());
            PostMock.Verify(m => m.Remove(It.IsAny<Post>()), Times.Once());
        }

        [TestMethod]
        public void SaveChanges_Test()
        {
            var dataRepo = this.GetDataRepository();
            dataRepo.SaveChanges();
            MockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void AllIncluding_Test()
        {
            var dataRepo = this.GetDataRepository();

            //TODO: in order to properly test this method you need to mock the include function of IQueryable<post>
            // as a hack just test with a single include to check if atleast include is being called
            var includes = new Expression<Func<Post, object>>[] { x => x.Content };

            dataRepo.AllIncluding<Post>(includes);
            PostMock.Verify(m => m.Include(It.IsAny<string>()), Times.Exactly(includes.Length));
        } 

        #endregion

        #region Test Helper Methods

        protected DataRepository<FlowCollectionEntities> GetDataRepository()
        {
            var testContext = this.GetDefaultContext();
            return new DataRepository<FlowCollectionEntities>(testContext);
        }
 
        #endregion
    }
}
