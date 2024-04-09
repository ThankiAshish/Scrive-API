import { useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { toast } from "react-toastify";

const ResetPassword = () => {
  const navigate = useNavigate();
  const { token } = useParams();

  const [formData, setFormData] = useState({
    password: "",
    confirmPassword: "",
    token: token,
  });

  const handleInputChange = (e) => {
    setFormData({ ...formData, [e.target.id]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (formData.password.length < 8) {
      return toast.error("Password must be at least 8 characters long!");
    }

    if (formData.password !== formData.confirmPassword) {
      return toast.error("Passwords do not match!");
    }

    const toastId = toast("Resetting Password...", { autoClose: false });

    const response = await fetch(
      `${import.meta.env.VITE_API_URL}/api/auth/reset-password`,
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
      toast.dismiss(toastId);
      toast.success(data.message);
      navigate("/login");
    } else {
      toast.dismiss(toastId);
      toast.error(data.message);
    }
  };

  return (
    <main className="auth-showcase">
      <div className="container">
        <div className="auth-box">
          <h1 className="heading">
            <span className="highlight-text">Claim </span>
            the Lifeline!
          </h1>
          <p>Reset your Password!</p>
          <form onSubmit={handleSubmit}>
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
                Reset
              </button>
            </div>
          </form>
        </div>
      </div>
      <h1 className="logo">Scrive</h1>
    </main>
  );
};

export default ResetPassword;
