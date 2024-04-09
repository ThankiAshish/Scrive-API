import { useState, useEffect } from "react";
import PropTypes from "prop-types";

import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCloudArrowUp, faTimes } from "@fortawesome/free-solid-svg-icons";

const Uploader = ({ cover, setCover }) => {
  const [fileName, setFileName] = useState("");
  const [preview, setPreview] = useState("");

  useEffect(() => {
    if (typeof cover === "string") {
      setPreview(
        `${import.meta.env.VITE_API_URL}/public/uploads/covers/${cover}`
      );
    } else if (cover instanceof File) {
      setPreview(URL.createObjectURL(cover));
      setFileName(cover.name);
    }
  }, [cover]);

  const clearImage = (e) => {
    e.stopPropagation();
    setCover(null);
    setFileName("");
    document.getElementById("cover").value = null;
    setPreview("");
  };

  return (
    <main
      className="uploader"
      onClick={() => document.querySelector(".cover").click()}
    >
      <input
        type="file"
        name="cover"
        id="cover"
        className="cover"
        accept=".jpg, .jpeg, .png"
        multiple={false}
        hidden
        onChange={({ target: { files } }) => {
          files[0] && setFileName(files[0].name);
          if (files) {
            setCover(files[0]);
          }
        }}
      />
      {preview ? (
        <div className="image-preview">
          <img src={preview} alt={fileName} className="file-preview" />
          <button className="clear-button" onClick={clearImage}>
            <FontAwesomeIcon icon={faTimes} className="icon" />
          </button>
        </div>
      ) : (
        <div className="default-preview">
          <FontAwesomeIcon icon={faCloudArrowUp} className="icon" />
          <p>Upload Cover Image</p>
        </div>
      )}
    </main>
  );
};

Uploader.propTypes = {
  cover: PropTypes.oneOfType([PropTypes.object, PropTypes.string]),
  setCover: PropTypes.func.isRequired,
};

export default Uploader;
