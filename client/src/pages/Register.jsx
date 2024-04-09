import { useState, useEffect } from "react";
import { Link, useNavigate } from "react-router-dom";
import { toast } from "react-toastify";

import UserStore from "../stores/UserStore";

const Register = () => {
  const navigate = useNavigate();
  const { loginState } = UserStore();

  useEffect(() => {
    if (loginState) {
      navigate("/app");
    }
  }, [loginState, navigate]);

  const [formData, setFormData] = useState({
    username: "",
    email: "",
    password: "",
    confirmPassword: "",
    profilePicture:
      "https://api.dicebear.com/7.x/adventurer/svg?scale=75&backgroundType=gradientLinear&earringsProbability=50&featuresProbability=50&glassesProbability=50&backgroundColor=b6e3f4,c0aede,d1d4f9&seed=",
  });

  const handleInputChange = (e) => {
    setFormData({ ...formData, [e.target.id]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      if (formData.password.length < 8) {
        return toast.error("Password must be at least 8 characters long");
      }

      if (formData.password !== formData.confirmPassword) {
        return toast.error("Passwords do not match");
      }

      const response = await fetch(
        `${import.meta.env.VITE_API_URL}/api/auth/register`,
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(formData),
        }
      );

      const data = await response.json();

      if (response.status === 200) {
        toast.success(data.message);
        setFormData({
          username: "",
          email: "",
          password: "",
          confirmPassword: "",
        });
        navigate("/login");
      } else {
        toast.error(data.message);
      }
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <main className="auth-showcase">
      <div className="container">
        <div className="auth-box">
          <h1 className="heading">
            Ready to <span className="highlight-text">Scrive?</span>
          </h1>
          <p>
            Already a Scriver?{" "}
            <Link to="/login" className="link-text">
              Ahoy, Mate!
            </Link>
          </p>
          <form onSubmit={handleSubmit}>
            <input
              type="text"
              id="username"
              placeholder="Enter Username"
              value={formData.username}
              onChange={handleInputChange}
            />
            <input
              type="email"
              id="email"
              placeholder="Enter Email Address"
              value={formData.email}
              onChange={handleInputChange}
            />
            <input
              type="password"
              id="password"
              placeholder="Enter Password"
              value={formData.password}
              onChange={handleInputChange}
            />
            <input
              type="password"
              id="confirmPassword"
              placeholder="Confirm Password"
              value={formData.confirmPassword}
              onChange={handleInputChange}
            />
            <div className="btn-container">
              <button type="submit" className="btn">
                Register
              </button>
            </div>
          </form>
        </div>
      </div>
      <h1 className="logo">Scrive</h1>
    </main>
  );
};

export default Register;
