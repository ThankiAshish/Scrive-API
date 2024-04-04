using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScriveAPI.Helpers;
using ScriveAPI.Services;

namespace ScriveAPI.Controllers
{
    [Route("api/blog")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly BlogServices _blogServices;
        public BlogController(BlogServices blogServices)
        {
            _blogServices = blogServices;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var blogs = await _blogServices.GetAll();
                return Ok(blogs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            try
            {
                var blog = _blogServices.GetById(id);
                return Ok(blog);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateBlog([FromForm] CreateBlogModel model)
        {
            try
            {
                var token = Request.Headers["x-auth-token"].ToString();

                var blog = await _blogServices.Create(token, model.Title, model.Summary, model.Cover, model.Content);
                return Ok(blog);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromForm] UpdateBlogModel model)
        {
            try
            {
                var token = Request.Headers["x-auth-token"].ToString();

                var blog = await _blogServices.Update(id, token, model.Title, model.Summary, model.Cover, model.Content);
                return Ok(blog);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var token = Request.Headers["x-auth-token"].ToString();

                var blog = await _blogServices.Delete(id, token);

                return Ok(blog);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
