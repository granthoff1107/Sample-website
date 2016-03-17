using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository.Tests.General
{
    [TestClass]
    public class TestBase
    {
        #region Members

        protected User testUser = null;
        protected List<Content> testPostContents = new List<Content>();
        protected List<Post> testPosts = new List<Post>();

        #endregion

        #region Constructors

        public TestBase()
        {
            InitializeContext();
        }

        #endregion

        #region Data Population Methods

        protected void InitializeContext()
        {
            testUser = new User { ID = 4, Username = "TestUser", Email = "TestEmail@fake.com", Created = new DateTime(1989, 11, 4), Contents = testPostContents };

            this.AddPost(1, "Some Really basic Post", "Basic Post", testUser, 1);
            this.AddPost(2, "Another Some Really basic Post", "2 Basic Post", testUser, 2);
        }

        protected void AddPost(int id, string entry, string title, User user, int contentId, bool isVisible = true, DateTime? created = null, DateTime? lastModified = null)
        {
            created = created ?? DateTime.Now;
            lastModified = lastModified ?? DateTime.Now;

            var content = new Content
            {
                Id = contentId,
                Title = title,
                Entry = entry,
                Created = created.Value,
                LastModified = lastModified.Value,
                User = user,
                UserId = user.ID,
                Visible = isVisible
            };
            var post = new Post { Id = id, ContentId = contentId, Content = content };

            content.Posts = new List<Post> { post };

            testPosts.Add(post);
            testPostContents.Add(content);
        }

        #endregion

        #region Mock Members

        private Mock<FlowCollectionEntities> _mockContext;
        private Mock<DbSet<Post>> _postMock;

        #endregion

        #region Mock Properties

        protected Mock<DbSet<Post>> PostMock
        {
            get
            {
                if (null == _postMock)
                {
                    _postMock = this.GetMockDbSet(testPosts.AsQueryable());
                }
                return _postMock;
            }
        }

        protected Mock<FlowCollectionEntities> MockContext
        {
            get
            {
                if (null == _mockContext)
                {
                    _mockContext = this.CreateMockContext();
                }
                return _mockContext;
            }
        }

        #endregion

        #region Private Mock Methods

        private Mock<FlowCollectionEntities> CreateMockContext()
        {
            testPosts.AsQueryable();

            var defaultContext = new Mock<FlowCollectionEntities>();

            var postDbSet = PostMock.Object;
            defaultContext.Setup(x => x.Posts).Returns(postDbSet);
            defaultContext.Setup(m => m.Set<Post>()).Returns(postDbSet);

            return defaultContext;
        }

        private Mock<DbSet<T>> GetMockDbSet<T>(IQueryable<T> entityQueryable)
            where T : class
        {
            var dbSetMock = new Mock<DbSet<T>>();
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(entityQueryable.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(entityQueryable.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(entityQueryable.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(entityQueryable.GetEnumerator());

            return dbSetMock;
        }

        #endregion

        #region Test Methods

        protected FlowCollectionEntities GetDefaultContext()
        {
            return this.MockContext.Object;
        }

        #endregion

    }
}
