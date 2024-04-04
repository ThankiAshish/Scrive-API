namespace ScriveAPI.Helpers
{
    public class UpdateBlogModel
    {
        public string? Title { get; set; }
        public string? Summary { get; set; }
        public IFormFile? Cover { get; set; }
        public string? Content { get; set; }
    }
}
