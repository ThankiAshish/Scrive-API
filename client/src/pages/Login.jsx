import { useState, useEffect } from "react";
import { Link, useNavigate } from "react-router-dom";
import { toast } from "react-toastify";

import UserStore from "../stores/UserStore";
import { UserState } from "../context/UserContext";

const Login = () => {
  const navigate = useNavigate();
  const { loginData, loginState, setLoginState } = UserStore();
  const { setUser } = UserState();

  useEffect(() => {
    if (loginState) {
      navigate("/app");
    }
  }, [loginState, navigate]);

  const [formData, setFormData] = useState({ email: "", password: "" });

  const handleInputChange = (e) => {
    setFormData({ ...formData, [e.target.id]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (formData.email === "" || formData.password === "") {
      return toast.error("All fields are required");
    }

    if (formData.password.length < 8) {
      return toast.error("Password must be at least 8 characters long");
    }

    try {
      const response = await fetch(
        `${import.meta.env.VITE_API_URL}/api/auth/login`,
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
        loginData(data.response);
        setLoginState(true);
        setUser(data.response);
        toast.success(data.message);
        navigate("/app");
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
            <span className="highlight-text">Welcome </span>
            Back!
          </h1>
          <p>
            Not already a Scriver?{" "}
            <Link to="/register" className="link-text">
              Become one
            </Link>
          </p>
          <form onSubmit={handleSubmit}>
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
            <div className="btn-container">
              <button type="submit" className="btn">
                Login
              </button>
              <Link to="/forgot-password">Forgot Password?</Link>
            </div>
          </form>
        </div>
      </div>
      <h1 className="logo">Scrive</h1>
    </main>
  );
};

export default Login;
