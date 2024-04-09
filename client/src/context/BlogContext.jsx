import { createContext, useContext, useState, useEffect } from "react";
import PropTypes from "prop-types";

const BlogContext = createContext({});

const BlogProvider = ({ children }) => {
  const [blogs, setBlogs] = useState([]);

  useEffect(() => {
    const fetchBlogs = async () => {
      const response = await fetch(`${import.meta.env.VITE_API_URL}/api/blog`);

      if (response.status === 200) {
        const data = await response.json();
        setBlogs(data.blogs);
      } else {
        setBlogs([]);
      }
    };

    fetchBlogs();
  }, []);

  return (
    <BlogContext.Provider value={{ blogs, setBlogs }}>
      {children}
    </BlogContext.Provider>
  );
};

BlogProvider.propTypes = {
  children: PropTypes.node.isRequired,
};

export const BlogState = () => {
  return useContext(BlogContext);
};

export default BlogProvider;
