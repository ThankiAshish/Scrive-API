import { Link } from "react-router-dom";
import PropTypes from "prop-types";

const Blog = ({ blog }) => {
  const blogStyle = {
    backgroundImage: `url(${
      import.meta.env.VITE_API_URL
    }/public/uploads/covers/${blog.cover})`,
  };

  return (
    <Link to="/blog" state={{ id: blog.id }}>
      <div className="blog" style={blogStyle}>
        <div className="blog-content">
          <h2>{blog.title}</h2>
          <p className="summary">{blog.summary}</p>
          <div className="blog-details">
            <img
              src={blog.author.profilePicture + blog.author.username}
              alt="avatar"
            />
            <div className="blog-author">
              <h4>{blog.author.username}</h4>
              <p>
                {new Date(blog.createdAt).getMonth() + 1}/
                {new Date(blog.createdAt).getDate()}/
                {new Date(blog.createdAt).getFullYear()}
              </p>
            </div>
          </div>
        </div>
      </div>
    </Link>
  );
};

Blog.propTypes = {
  blog: PropTypes.object.isRequired,
};

export default Blog;
