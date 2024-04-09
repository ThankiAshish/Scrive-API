import { Link } from "react-router-dom";

const ResetPasswordInfo = () => {
  return (
    <main className="auth-showcase reset-password-info">
      <div className="container">
        <h1 className="heading">
          <span className="highlight-text">Lifeline</span> on the Way!
        </h1>
        <p>
          We have sent you an email with instructions to reset your password.
        </p>
        <Link to="/login" className="link-text">
          Go back to Login
        </Link>
      </div>
      <h1 className="logo">Scrive</h1>
    </main>
  );
};

export default ResetPasswordInfo;
