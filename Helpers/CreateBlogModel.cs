namespace ScriveAPI.Helpers
{
    public class CreateBlogModel
    {
        public string Title { get; set; }
        public string Summary { get; set; }
        public IFormFile Cover { get; set; }
        public string Content { get; set; }
    }
}
