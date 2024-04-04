using MongoDB.Driver;
using ScriveAPI.Data;
using ScriveAPI.Helpers;
using ScriveAPI.Models;

namespace ScriveAPI.Services
{
    public class BlogServices
    {
        private readonly BlogContext _dbContext;
        private readonly UserContext _userContext;
        private readonly IMongoCollection<Blog> _blogs;
        private readonly IMongoCollection<User> _users;
        private readonly IConfiguration _configuration;
        private readonly TokenValidator _tokenValidator;
        public BlogServices(BlogContext dbContext, UserContext userContext, IMongoCollection<Blog> blogs, IMongoCollection<User> users, IConfiguration configuration, TokenValidator tokenValidator)
        {
            _dbContext = dbContext;
            _userContext = userContext;
            _blogs = blogs;
            _users = users;
            _configuration = configuration;
            _tokenValidator = tokenValidator;
        }
        public async Task<IEnumerable<Blog>> GetAll()
        {
            try
            {
                var blogs = await _dbContext.Blogs
                    .Find(_ => true)
                    .SortByDescending(b => b.CreatedAt)
                    .ToListAsync();

                return blogs;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch blogs", ex);
            }
        }
        public async Task<Blog> GetById(string id)
        {
            try
            {
                var blog = await _dbContext.Blogs
                  .Find(b => b.Id == id)
                  .FirstOrDefaultAsync();

                if (blog == null)
                {
                    throw new Exception("Blog not found");
                }

                return blog;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Blog> Create(string token, string title, string summary, IFormFile cover, string content)
        {
            try
            {
                var userId = _tokenValidator.ExtractUserIdFromToken(token);

                if(userId == null)
                {
                    throw new Exception("Invalid Token");
                }

                var user = await _users.FindAsync(u => u.Id == userId).Result.FirstOrDefaultAsync();

                if(user == null)
                {
                    throw new Exception("No user found!");
                }

                user.Password = String.Empty;

                var blog = new Blog
                {
                    Title = title,
                    Summary = summary,
                    Cover = String.Empty,
                    Content = content,
                    Author = user,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };

                if(cover != null && cover.Length > 0)
                {
                    var fileName = Path.GetFileName(cover.FileName);
                    var filePath = Path.Combine("wwwroot/uploads/covers", fileName);
                    using(var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await cover.CopyToAsync(stream);
                    }

                    blog.Cover = fileName;
                }

                await _blogs.InsertOneAsync(blog);

                return blog;
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Blog> Update(string id, string token, string title, string summary, IFormFile cover, string content)
        {
            try
            {
                var userId = _tokenValidator.ExtractUserIdFromToken(token);

                if (userId == null)
                {
                    throw new Exception("Invalid Token");
                }

                var user = await _users.FindAsync(u => u.Id == userId).Result.FirstOrDefaultAsync();

                if (user == null)
                {
                    throw new Exception("No user found!");
                }

                var blog = await _blogs.FindOneAndUpdateAsync(
                    b => b.Id == id,
                    Builders<Blog>.Update
                        .Set(b => b.Title, title)
                        .Set(b => b.Summary, summary)
                        .Set(b => b.Content, content)
                );

                if (cover != null && cover.Length > 0)
                {
                    var fileName = Path.GetFileName(cover.FileName);
                    var filePath = Path.Combine("wwwroot/uploads/covers", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await cover.CopyToAsync(stream);
                    }

                    await _blogs.FindOneAndUpdateAsync(
                        b => b.Id == id,
                        Builders<Blog>.Update
                            .Set(b => b.Cover, fileName)
                    );
                }

                return blog;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Blog> Delete(string id, string token)
        {
            try
            {
                var userId = _tokenValidator.ExtractUserIdFromToken(token);

                if (userId == null)
                {
                    throw new Exception("Invalid Token");
                }

                var user = await _users.FindAsync(u => u.Id == userId).Result.FirstOrDefaultAsync();

                if (user == null)
                {
                    throw new Exception("No user found!");
                }

                var blog = await _blogs.FindOneAndDeleteAsync(b => b.Id == id);

                if (blog == null)
                {
                    throw new Exception("Blog not found");
                }

                return blog;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
