import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import Swal from "sweetalert2";
import PuffLoader from "react-spinners/PuffLoader";

import UserStore from "../stores/UserStore";
import { UserState } from "../context/UserContext";

const override = {
  display: "block",
  margin: "0 auto",
  borderColor: "red",
};

const Profile = () => {
  const { loginState, userDetails, logout } = UserStore();
  const { user, setUser, loading } = UserState();
  const [formData, setFormData] = useState();
  const [edit, setEdit] = useState(false);

  const navigate = useNavigate();

  useEffect(() => {
    setFormData(user);

    if (!loginState) {
      toast.error("Unauthorized Access! Login to continue.");
      navigate("/login");
    }
  }, [loginState, navigate, setFormData, user]);

  const handleInputChange = (e) => {
    setFormData({ ...formData, [e.target.id]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const response = await fetch(
      `${import.meta.env.VITE_API_URL}/api/auth/user`,
      {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
          "x-auth-token": `${userDetails.token}`,
        },
        body: JSON.stringify(formData),
      }
    );

    if (response.status === 200) {
      const data = await response.json();
      setUser(data.user);
      setEdit(false);
      toast.success(data.message);
    }
  };

  const handleLogout = async (e) => {
    e.preventDefault();
    Swal.fire({
      title: "Logout",
      text: "Are you sure you want to logout?",
      icon: "warning",
      showCancelButton: true,
      confirmButtonText: "Yes",
      cancelButtonText: "No",
      customClass: {
        popup: "swan-popup",
        icon: "swan-icon",
        confirmButton: "btn btn-primary",
        cancelButton: "btn btn-outline btn-delete",
      },
    }).then((result) => {
      if (result.isConfirmed) {
        logout();
        toast.success("Logout Successful!");
        navigate("/login");
      }
    });
  };

  return (
    <div className="container">
      <section className="user-profile">
        {loading ? (
          <div className="loader">
            <PuffLoader
              color="#f76c6c"
              loading={loading}
              css={override}
              size={150}
            />
          </div>
        ) : (
          formData &&
          (edit ? (
            <form onSubmit={handleSubmit}>
              <h1>Edit Your Details</h1>
              <input
                type="text"
                id="username"
                value={formData.username}
                onChange={handleInputChange}
              />
              <input
                type="email"
                id="email"
                value={formData.email}
                onChange={handleInputChange}
              />
              <div className="btn-container">
                <button className="btn btn-primary" type="submit">
                  Save
                </button>
                <button
                  className="btn btn-outline"
                  onClick={() => setEdit(false)}
                >
                  Cancel
                </button>
              </div>
            </form>
          ) : (
            <>
              <img
                src={
                  formData && formData.profilePicture
                    ? formData.profilePicture + formData.username
                    : ""
                }
                alt={`${formData && formData.username} Profile Picture`}
              />
              <div className="user-details">
                <h2>{formData.username}</h2>
                <p>{formData.email}</p>

                <div className="btn-container">
                  <button
                    className="btn btn-primary"
                    onClick={() => setEdit(true)}
                  >
                    Edit Profile
                  </button>
                  <button
                    className="btn btn-outline btn-delete"
                    onClick={handleLogout}
                  >
                    Logout
                  </button>
                </div>
              </div>
            </>
          ))
        )}
      </section>
    </div>
  );
};

export default Profile;
