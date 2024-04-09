import { useState, useEffect } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import { toast } from "react-toastify";

import Blog from "../components/Blog";

import NotFound from "../assets/images/not-found.svg";

const Search = () => {
  const location = useLocation();
  const navigate = useNavigate();

  useEffect(() => {
    if (!location.state) {
      toast.error("An Error Occurred. Please try again.");
      navigate("/");
    }
  }, [location, navigate]);

  const { blogs } = location.state ? location.state : { blogs: [] };
  const [search, setSearch] = useState("");
  const [searchResults, setSearchResults] = useState([]);

  const handleSearchChange = (e) => {
    setSearch(e.target.value);

    if (e.target.value.trim() === "") {
      setSearchResults([]);
    }

    const results = blogs.filter(
      (blog) =>
        blog.title.toLowerCase().includes(search.toLowerCase()) ||
        blog.summary.toLowerCase().includes(search.toLowerCase()) ||
        blog.content.toLowerCase().includes(search.toLowerCase()) ||
        blog.author.username.toLowerCase().includes(search.toLowerCase())
    );

    setSearchResults(results);
  };

  const renderBlogs = (blogs) => (
    <div className={"search-results"}>
      {blogs.map((blog) => (
        <Blog key={blog.id} blog={blog} />
      ))}
    </div>
  );

  return (
    <div className="container">
      <section className="search">
        <h1>Search Blogs</h1>
        <input
          type="search"
          name="search"
          id="search"
          placeholder="Search Blogs by Author, Title, or Keywords"
          value={search}
          onChange={handleSearchChange}
        />
        {searchResults && searchResults.length > 0 ? (
          renderBlogs(searchResults)
        ) : search !== "" ? (
          <main className="default">
            <img src={NotFound} alt="Not Found" />
            <h1>No Blogs Found!</h1>
          </main>
        ) : blogs && blogs.length > 0 ? (
          renderBlogs(blogs)
        ) : (
          <main className="default">
            <img src={NotFound} alt="Not Found" />
            <h1>No Blogs Found!</h1>
          </main>
        )}
      </section>
    </div>
  );
};

export default Search;
