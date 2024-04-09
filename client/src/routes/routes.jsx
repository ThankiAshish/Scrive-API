import Welcome from "../pages/Welcome";
import Login from "../pages/Login";
import Register from "../pages/Register";
import ForgotPassword from "../pages/ForgotPassword";
import ResetPassword from "../pages/ResetPassword";
import ResetPasswordInfo from "../pages/ResetPasswordInfo";

import Home from "../pages/Home";

const publicRoutes = [
  {
    path: "/",
    element: <Welcome />,
    name: "Welcome",
  },
  {
    path: "/login",
    element: <Login />,
    name: "Login",
  },
  {
    path: "/register",
    element: <Register />,
    name: "Register",
  },
  {
    path: "/forgot-password",
    element: <ForgotPassword />,
    name: "ForgotPassword",
  },
  {
    path: "/reset-password",
    element: <ResetPassword />,
    name: "ResetPassword",
  },
  {
    path: "/reset-password-info",
    element: <ResetPasswordInfo />,
    name: "ResetPasswordInfo",
  },
];

const privateRoutes = [
  {
    path: "/",
    element: <Home />,
    name: "Home",
  },
];

export { publicRoutes, privateRoutes };
