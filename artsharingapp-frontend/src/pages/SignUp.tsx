import { NavLink } from "react-router-dom";
import "../styles/SignUp.css";
import { IoChevronBackOutline } from "react-icons/io5";

const SignUp = () => {
  return (
    <div className="sign-up-page">
      <NavLink to="/" className="sign-up-back-link">
        <IoChevronBackOutline />
      </NavLink>

      <div className="sign-up-card">
        <h1>Sign up</h1>
        <form className="sign-up-form">
          <div className="sign-up-form-field">
            <input type="text" id="name" name="name" required placeholder=" " />
            <label htmlFor="name">Name</label>
          </div>

          <div className="sign-up-form-field">
            <input
              type="text"
              id="username"
              name="username"
              required
              placeholder=" "
            />
            <label htmlFor="username">Username</label>
          </div>

          <div className="sign-up-form-field">
            <input
              type="email"
              id="email"
              name="email"
              required
              placeholder=" "
            />
            <label htmlFor="email">Email</label>
          </div>

          <div className="sign-up-form-field">
            <input
              type="password"
              id="password"
              name="password"
              required
              placeholder=" "
            />
            <label htmlFor="password">Password</label>
          </div>

          <div className="sign-up-form-field">
            <input
              type="password"
              id="confirm-password"
              name="confirm-password"
              required
              placeholder=" "
            />
            <label htmlFor="confirm-password">Confirm Password</label>
          </div>

          <div>
            <p>
              Already have an account?
              <NavLink to="/login" className="sign-up-login-link">
                Log in
              </NavLink>
            </p>
          </div>

          <button type="submit">Sign up</button>
        </form>
      </div>
    </div>
  );
};

export default SignUp;
