import { useState } from "react";
import { Link } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faBars, faXmark } from "@fortawesome/free-solid-svg-icons";

const GuestHeader = () => {
  const [isOpen, setIsOpen] = useState(false);

  return (
    <div className="container">
      <nav className="nav guest-nav">
        <Link to="/app">
          <h1 className="logo">Scrive</h1>
        </Link>
        <ul className={`nav-links ${isOpen ? "active" : ""}`}>
          {/* <li>
            <Link to="/about" className="nav-link">
              About
            </Link>
          </li>
          <li>
            <Link to="/contact" className="nav-link">
              Contact
            </Link>
          </li> */}
          <div className="btn-container">
            <li>
              <Link to="/login">
                <button className="btn btn-outline">Login</button>
              </Link>
            </li>
            <li>
              <Link to="/register">
                <button className="btn">Register</button>
              </Link>
            </li>
          </div>
        </ul>
        <FontAwesomeIcon
          onClick={() => setIsOpen(!isOpen)}
          icon={isOpen ? faXmark : faBars}
          className="menu-icon"
        />
      </nav>
    </div>
  );
};

export default GuestHeader;
