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
        protected List<TagsToContent> testTagsToContent = new List<TagsToContent>();

        //I don't care if it not fully proper, GetHashCode should be good enough for a random seed
        protected static Random random = new Random(Guid.NewGuid().GetHashCode());

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
            var post = this.CreateTestPost(id, entry, title, user, contentId, isVisible, created, lastModified);

            testPosts.Add(post);
            testPostContents.Add(post.Content);
            testTagsToContent.AddRange(post.Content.TagsToContents);
            
        }

        #endregion

        #region Mock Members

        private Mock<FlowCollectionEntities> _mockContext;
        private Mock<DbSet<Post>> _postMock;
        private Mock<DbSet<TagsToContent>> _tagsToContentMock;

        #endregion

        #region Mock Properties

        protected Mock<DbSet<TagsToContent>> TagsToContentMock
        {
            get
            {
                if (null == _tagsToContentMock)
                {
                    _tagsToContentMock = this.GetMockDbSet(testTagsToContent.AsQueryable());
                }
                return _tagsToContentMock;
            }
        }

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

            var tagsToContentDbSet = TagsToContentMock.Object;
            defaultContext.Setup(x => x.TagsToContents).Returns(tagsToContentDbSet);
            defaultContext.Setup(x => x.Set<TagsToContent>()).Returns(tagsToContentDbSet);

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

        #region Test Helper Methods

        #region Get Methods

        protected FlowCollectionEntities GetDefaultContext()
        {
            return this.MockContext.Object;
        }

        protected static readonly string strippedTestEntry = "this is valid for now";
        protected static readonly string sanitizedTestEntry = "<div><p>" + strippedTestEntry + "</p></div>";
        protected static readonly string rawTestEntry = "<script>alert('some bs')</script>" + sanitizedTestEntry;

        protected Content GetContent()
        {
            int contentId = testPostContents.Max(x => x.Id) + 1;
            return this.CreateContent(contentId, sanitizedTestEntry, "Test Content Title", testUser, true);
        }

        protected Post GetTestPost()
        {
            int postId = testPosts.Max(x => x.Id) + 1;
            var content = this.GetContent();
            return new Post { ContentId = content.Id, Content = content, Id = postId, CoverPhotoUrl = "test" };
        }

        #endregion

        #region Create Content Methods

        protected Content CreateContent(int id, string entry, string title, User user, bool isVisible = true, DateTime? created = null, DateTime? lastModified = null)
        {
            created = created ?? DateTime.Now;
            lastModified = lastModified ?? DateTime.Now;

            var content = new Content
            {
                Id = id,
                Title = title,
                Entry = entry,
                Created = created.Value,
                LastModified = lastModified.Value,
                User = user,
                UserId = user.ID,
                Visible = isVisible
            };

            content.Posts = new List<Post>();
            content.Videos = new List<Video>();
            content.TagsToContents = new List<TagsToContent>();

            return content;
        }

        protected Post CreateTestPost(int id, string entry, string title, User user, int contentId, bool isVisible = true, DateTime? created = null, DateTime? lastModified = null)
        {
            var content = this.CreateContent(contentId, entry, title, user, isVisible, created, lastModified);

            //Hack set the tag id to be content id
            var tagToContent = CreateTagToContent(content, content.Id);
            var post = new Post { Id = id, ContentId = contentId, Content = content };

            content.Posts.Add(post);
            return post;
        }

        protected TagsToContent CreateTagToContent(Content content, int TagId)
        {
            var tagToContent = new TagsToContent { Content = content, TagId = TagId, ContentId = content.Id };
            content.TagsToContents.Add(tagToContent);

            return tagToContent;
        }

        #endregion

        #endregion
    }
}
