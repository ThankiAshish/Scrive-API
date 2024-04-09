import { useEffect } from "react";

import Blog from "../components/Blog";

import { BlogState } from "../context/BlogContext";

import NotFound from "../assets/images/not-found.svg";

const Home = () => {
  const { blogs, setBlogs } = BlogState();

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
  }, [setBlogs]);

  return (
    <div className="container">
      {blogs.length > 0 ? (
        <main className="home">
          {blogs.map((blog) => (
            <Blog key={blog.id} blog={blog} />
          ))}
        </main>
      ) : (
        <main className="default">
          <img src={NotFound} alt="Not Found" />
          <h1>No Blogs Found!</h1>
        </main>
      )}
    </div>
  );
};

export default Home;
