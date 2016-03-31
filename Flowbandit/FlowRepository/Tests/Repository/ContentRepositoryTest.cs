using FlowRepository.Repositories.Models.FlowRepository;
using FlowRepository.Tests.General;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using System.Data.Entity;

namespace FlowRepository.Tests.Repository
{
    [TestClass]
    public class ContentRepositoryTest : TestBase
    {
        #region Test Methods

        [TestMethod]
        public void Add_Content_Test()
        {
            var dataRepo = this.GetRepository();
            
            var post = this.GetTestPost();
            post.Content.Entry = rawTestEntry;
            post.Content.LastModified = DateTime.MinValue;
            post.Content.Created = DateTime.MinValue;

            dataRepo.Add<Post>(post);

            post.Content.LastModified.Should().BeCloseTo(DateTime.Now, 1000);
            post.Content.Created.Should().BeCloseTo(DateTime.Now, 1000);
            post.Content.Entry.ShouldBeEquivalentTo(sanitizedTestEntry);
            
            PostMock.Verify(m => m.Add(It.IsAny<Post>()), Times.Once());
        }

        [TestMethod]
        public void Edit_Content_Test()
        {
            var dataRepo = this.GetRepository();

            var post = this.GetTestPost();
            post.Content.Entry = rawTestEntry;
            post.Content.LastModified = DateTime.MinValue;

            Assert.IsTrue(dataRepo.Context.Entry(post).State == EntityState.Detached);
            Assert.IsTrue(dataRepo.Context.Entry(post.Content).State == EntityState.Detached);

            var tags = new List<int>() { 7, 9, 1};

            dataRepo.Edit(post, tags);

            Assert.IsTrue(dataRepo.Context.Entry(post).State == EntityState.Modified);
            Assert.IsTrue(dataRepo.Context.Entry(post.Content).State == EntityState.Modified);

            post.Content.LastModified.Should().BeCloseTo(DateTime.Now, 1000);
            post.Content.Entry.ShouldBeEquivalentTo(sanitizedTestEntry);

            post.Content.TagsToContents.Select(x => x.TagId).ShouldBeEquivalentTo(tags);
        }

        [TestMethod]
        public void GetMostRecentVisibleContent_Test()
        {
            var dataRepo = this.GetRepository();

            //two post should be hidden, by default we hide any post divisible by 5
            var addedPosts = this.PopulateDefaultPost(ref nextFreeId, testUser2, 12);
            var posts = dataRepo.GetMostRecentVisibleContent(dataRepo.All<Post>(), 0, 10, testUser.ID);

            addedPosts.Where(x => x.Content.Visible).ShouldAllBeEquivalentTo(posts);
        }
        
        [TestMethod]
        public void GetVisibleContentById_Test()
        {
            var dataRepo = this.GetRepository();
            var addedPosts = this.PopulateDefaultPost(ref nextFreeId, testUser2, 1);

            var expectedPost = addedPosts.Single();
            var actualPost = dataRepo.GetVisibleContentByIdWithCommentsTagsUsers<Post>(expectedPost.Id, expectedPost.Content.UserId);

            PostMock.Verify(m => m.Include(It.IsAny<string>()), Times.Exactly(3));

            expectedPost.ShouldBeEquivalentTo(actualPost);
        }

        #endregion

        #region Test Helper Methods

        protected ContentRepository GetRepository()
        {
            var testContext = this.GetDefaultContext();
            return new ContentRepository(testContext);
        } 

        #endregion
    }
}
