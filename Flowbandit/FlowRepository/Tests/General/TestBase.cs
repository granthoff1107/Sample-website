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
        protected User testUser2 = null;
        protected List<Content> testPostContents = new List<Content>();
        protected List<Post> testPosts = new List<Post>();
        protected List<TagsToContent> testTagsToContent = new List<TagsToContent>();
        protected List<User> testUsers = new List<User>();

        TestLoremGenerator loremGenerator = new TestLoremGenerator("../../Tests/Resources/IpsumLorem.txt");

        #endregion

        #region Constructors

        public TestBase()
        {
            InitializeContext();
        }

        #endregion

        #region Data Population Methods

        protected int nextFreeId = 1;

        protected void InitializeContext()
        {
            testUser = new User { ID = 4, Username = "TestUser", Email = "TestEmail@fake.com", 
                                  Created = new DateTime(1989, 11, 4), 
                                  Contents = testPostContents.Where(x => x.UserId == 4).ToList() };

            testUser2 = new User { ID = 5, Username = "TestUser2", Email = "TestEmail2@fake.com", 
                                  Created = new DateTime(1999, 12, 5), 
                                  Contents = testPostContents.Where(x => x.UserId == 5).ToList() };

            this.testUsers.Add(testUser);
            this.testUsers.Add(testUser2);

            this.PopulateDefaultPost(ref nextFreeId, testUser, 20);
            this.PopulateDefaultPost(ref nextFreeId, testUser2, 20);
            
        }

        protected IEnumerable<Post> PopulateDefaultPost(ref int id, User user, int numberOfPost, int entryLength = 250, int titleLength = 50)
        {
            var posts = new List<Post>();
            for (int x = 0; x < numberOfPost; x++)
            {
                var isVisible = this.IsPostVisible(id);
                var entry = loremGenerator.GetLorem(entryLength);
                var title = id + loremGenerator.GetLorem(titleLength);
                var date = DateTime.Now.AddSeconds(id);
                var post = this.AddPost(id, entry, title, user, id++, isVisible, date, date);
                posts.Add(post);
            }

            return posts;
        }

        protected bool IsPostVisible(int id)
        {
            return id % 5 != 0;
        }

        protected Post AddPost(int id, string entry, string title, User user, int contentId, bool isVisible = true, DateTime? created = null, DateTime? lastModified = null)
        {
            var post = this.CreateTestPost(id, entry, title, user, contentId, isVisible, created, lastModified);

            testPosts.Add(post);
            testPostContents.Add(post.Content);
            testTagsToContent.AddRange(post.Content.TagsToContents);

            return post;
            
        }

        #endregion

        #region Mock Members

        private Mock<FlowCollectionEntities> _mockContext;
        private Mock<DbSet<Post>> _postMock;
        private Mock<DbSet<TagsToContent>> _tagsToContentMock;
        private Mock<DbSet<User>> _userMock;

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

        protected Mock<DbSet<User>> UserMock
        {
            get
            {
                if (null == _userMock)
                {
                    _userMock = this.GetMockDbSet(testUsers.AsQueryable());
                }
                return _userMock;
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

            dbSetMock.Setup(m => m.Include(It.IsAny<string>())).Returns(dbSetMock.Object);

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
