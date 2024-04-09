import { useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { toast } from "react-toastify";

import Uploader from "../components/Uploader";
import Editor from "../components/Editor";
import UserStore from "../stores/UserStore";

const Edit = () => {
  const location = useLocation();
  const { loginState, userDetails } = UserStore();

  const navigate = useNavigate();

  useEffect(() => {
    if (!location.state) {
      toast.error("Unauthorized Access! Login to continue.");
      navigate("/app");
    }

    if (!loginState) {
      toast.error("Unauthorized Access! Login to continue.");
      navigate("/login");
      return;
    }
  }, [location.state, loginState, navigate]);

  const { id, title, summary, content, cover } = location.state || {};

  const [editedTitle, setEditedTitle] = useState(title);
  const [editedSummary, setEditedSummary] = useState(summary);
  const [editedContent, setEditedContent] = useState(content);
  const [editedCover, setEditedCover] = useState(cover);

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!editedTitle || !editedSummary || !editedContent) {
      return toast.error("All fields are required");
    }

    const formData = new FormData();
    formData.append("title", editedTitle);
    formData.append("summary", editedSummary);
    formData.append("content", editedContent);
    if (editedCover) {
      formData.append("cover", editedCover);
    }

    const response = await fetch(
      `${import.meta.env.VITE_API_URL}/api/blog/${id}`,
      {
        method: "PUT",
        headers: {
          "x-auth-token": `${userDetails.token}`,
        },
        body: formData,
      }
    );

    const data = await response.json();

    if (response.status === 200) {
      toast.success(data.message);
      navigate("/app");
    } else {
      toast.error(data.message);
    }
  };

  return (
    <div className="container">
      <section className="create-blog">
        <h1>Edit Blog</h1>
        <form onSubmit={handleSubmit}>
          <input
            type="text"
            name="title"
            id="title"
            placeholder="Enter Blog Title"
            value={editedTitle}
            onChange={(e) => setEditedTitle(e.target.value)}
          />
          <input
            type="text"
            name="summary"
            id="summary"
            placeholder="Enter Blog Summary"
            value={editedSummary}
            onChange={(e) => setEditedSummary(e.target.value)}
          />
          <Uploader cover={editedCover} setCover={setEditedCover} />
          <Editor value={editedContent} onChange={setEditedContent} />
          <button type="submit" className="btn">
            Update Blog
          </button>
        </form>
      </section>
    </div>
  );
};

export default Edit;
