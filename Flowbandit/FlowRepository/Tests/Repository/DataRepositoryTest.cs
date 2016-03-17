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
        [TestMethod]
        public void All_Query_Test()
        {
            var testContext = this.GetDefaultContext();
            var dataRepo = new DataRepository<FlowCollectionEntities>(testContext);

            var postsList = dataRepo.All<Post>().ToList();
            postsList.ShouldAllBeEquivalentTo(testPosts);
        }

        [TestMethod]
        public void Add_Query_Test()
        {
            var testContext = this.GetDefaultContext();
            var dataRepo = new DataRepository<FlowCollectionEntities>(testContext);

            dataRepo.Add<Post>(TestPost);
            PostMock.Verify(m => m.Add(It.IsAny<Post>()), Times.Once());
        }

        [TestMethod]
        public void Remove_Query_Test()
        {
            var testContext = this.GetDefaultContext();
            var dataRepo = new DataRepository<FlowCollectionEntities>(testContext);

            dataRepo.Delete(TestPost);
            PostMock.Verify(m => m.Remove(It.IsAny<Post>()), Times.Once());
        }

        [TestMethod]
        public void SaveChanges_Test()
        {
            var testContext = this.GetDefaultContext();
            var dataRepo = new DataRepository<FlowCollectionEntities>(testContext);

            dataRepo.SaveChanges();
            MockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void AllIncluding_Test()
        {
            var testContext = this.GetDefaultContext();
            var dataRepo = new DataRepository<FlowCollectionEntities>(testContext);

            //TODO: in order to properly test this method you need to mock the include function of IQueryable<post>
            // as a hack just test with a single include to check if atleast include is being called
            var includes = new Expression<Func<Post, object>>[] { x => x.Content };

            dataRepo.AllIncluding<Post>(includes);
            PostMock.Verify(m => m.Include(It.IsAny<string>()), Times.Exactly(includes.Length));
        }

        protected Post TestPost
        {
            get
            {
                int postId = 666;
                return new Post { ContentId = postId, Id = postId, CoverPhotoUrl = "test" };
            }
        }
    }



}
