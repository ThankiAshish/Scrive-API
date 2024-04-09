import { BrowserRouter as Router, Routes, Route } from "react-router-dom";

import UserProvider from "./context/UserContext";
import BlogProvider from "./context/BlogContext";

import MainLayout from "./layouts/MainLayout";

import Welcome from "./pages/Welcome";
import About from "./pages/About";
import Contact from "./pages/Contact";
import Login from "./pages/Login";
import Register from "./pages/Register";
import ForgotPassword from "./pages/ForgotPassword";
import ResetPassword from "./pages/ResetPassword";
import ResetPasswordInfo from "./pages/ResetPasswordInfo";
import Home from "./pages/Home";
import Blog from "./pages/Blog";
import Profile from "./pages/Profile";
import Create from "./pages/Create";
import Edit from "./pages/Edit";
import Search from "./pages/Search";

const App = () => {
  return (
    <UserProvider>
      <BlogProvider>
        <Router>
          <Routes>
            <Route path="/about" element={<About />} />
            <Route path="/contact" element={<Contact />} />
            <Route path="/login" element={<Login />} />
            <Route path="/register" element={<Register />} />
            <Route path="/forgot-password" element={<ForgotPassword />} />
            <Route path="/reset-password/:token" element={<ResetPassword />} />
            <Route
              path="/reset-password-info"
              element={<ResetPasswordInfo />}
            />
            <Route path="/" element={<MainLayout />}>
              <Route index element={<Welcome />} />
              <Route path="/app" element={<Home />} />
              <Route path="/blog" element={<Blog />} />
              <Route path="/profile" element={<Profile />} />
              <Route path="/create" element={<Create />} />
              <Route path="/edit" element={<Edit />} />
              <Route path="/search" element={<Search />} />
            </Route>
          </Routes>
        </Router>
      </BlogProvider>
    </UserProvider>
  );
};

export default App;
