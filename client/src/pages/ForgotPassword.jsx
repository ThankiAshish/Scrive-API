import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";

const ForgotPassword = () => {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({ email: "" });

  const handleInputChange = (e) => {
    setFormData({ ...formData, [e.target.id]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!formData.email) {
      return toast.error("Email is required!");
    }

    const toastId = toast("Sending Password Reset Email...", {
      autoClose: false,
    });

    try {
      const response = await fetch(
        `${import.meta.env.VITE_API_URL}/api/auth/forgot-password`,
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(formData),
        }
      );

      const data = await response.json();

      toast.dismiss(toastId);

      if (response.status === 200) {
        toast.success(data.message);
        navigate("/reset-password-info");
      } else {
        toast.error(data.message);
      }
    } catch (error) {
      toast.dismiss(toastId);
      toast.error("An error occurred, please try again");
    }
  };

  return (
    <main className="auth-showcase">
      <div className="container">
        <div className="auth-box">
          <h1 className="heading">
            Login <span className="highlight-text">Lifeline!</span>
          </h1>
          <p>Let&apos; get back to Scriving!</p>
          <form onSubmit={handleSubmit}>
            <input
              type="email"
              id="email"
              placeholder="Enter Email Address"
              value={formData.email}
              onChange={handleInputChange}
            />
            <div className="btn-container">
              <button type="submit" className="btn">
                Submit
              </button>
            </div>
          </form>
        </div>
      </div>
      <h1 className="logo">Scrive</h1>
    </main>
  );
};

export default ForgotPassword;
