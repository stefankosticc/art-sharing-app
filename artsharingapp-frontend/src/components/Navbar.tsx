import { NavLink } from "react-router-dom";
import "../styles/Navbar.css";

const Navbar = () => {
  const handleScroll = (e: React.MouseEvent, id: string) => {
    e.preventDefault();
    const element = document.getElementById(id);
    if (element) {
      const offset = 8 * 16;
      window.scrollTo({
        top: element.offsetTop - offset,
        behavior: "smooth",
      });
    }
  };

  return (
    <div className="navbar">
      <div className="navbar-links">
        <a href="#lp-header" onClick={(e) => handleScroll(e, "lp-header")}>
          Home
        </a>
        <a href="">About</a>
        <a href="">Services</a>
        <a href="#contact" onClick={(e) => handleScroll(e, "contact")}>
          Contact
        </a>
      </div>
      <b className="navbar-logo navbar-center">ASA</b>
      <div className="navbar-login">
        <NavLink className="navbar-btn" to="/login">
          Log in
        </NavLink>
        <NavLink className="navbar-btn" to="/sign-up">
          Sign up
        </NavLink>
      </div>
    </div>
  );
};

export default Navbar;
