import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";

import Uploader from "../components/Uploader";
import Editor from "../components/Editor";

import UserStore from "../stores/UserStore";

const Create = () => {
  const { loginState, userDetails } = UserStore();
  const navigate = useNavigate();

  useEffect(() => {
    if (!loginState) {
      toast.error("Unauthorized Access! Login to continue.");
      navigate("/login");
    }
  }, [loginState, navigate]);

  const [title, setTitle] = useState("");
  const [summary, setSummary] = useState("");
  const [content, setContent] = useState("");
  const [cover, setCover] = useState(null);

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!title || !summary || !content || !cover) {
      return toast.error("All fields are required");
    }

    const formData = new FormData();
    formData.append("title", title);
    formData.append("summary", summary);
    formData.append("content", content);
    formData.append("cover", cover);

    const response = await fetch(`${import.meta.env.VITE_API_URL}/api/blog`, {
      method: "POST",
      headers: {
        "x-auth-token": `${userDetails.token}`,
      },
      body: formData,
    });

    const data = await response.json();

    if (response.status === 200) {
      toast.success(data.message);
      navigate("/app");
    } else {
      toast.error(data.message);
      console.log(data);
    }
  };

  return (
    <div className="container">
      <section className="create-blog">
        <h1>Create Blog</h1>
        <form onSubmit={handleSubmit}>
          <input
            type="text"
            name="title"
            id="title"
            placeholder="Enter Blog Title"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
          />
          <input
            type="text"
            name="summary"
            id="summary"
            placeholder="Enter Blog Summary"
            value={summary}
            onChange={(e) => setSummary(e.target.value)}
          />
          <Uploader cover={cover} setCover={setCover} />
          <Editor value={content} onChange={setContent} />
          <button type="submit" className="btn">
            Create Blog
          </button>
        </form>
      </section>
    </div>
  );
};

export default Create;
