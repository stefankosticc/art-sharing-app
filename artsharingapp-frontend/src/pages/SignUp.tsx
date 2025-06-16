import { NavLink, useNavigate } from "react-router-dom";
import "../styles/SignUp.css";
import { IoChevronBackOutline } from "react-icons/io5";
import { useState } from "react";
import { signUp, SignUpRequest } from "../services/auth";

const SignUp = () => {
  const [name, setName] = useState("");
  const [userName, setUserName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");

  type SignUpErrors = {
    confirmPassword?: string;
    general?: string;
  };

  const [errors, setErrors] = useState<SignUpErrors>({});
  const navigate = useNavigate();

  const handleRegister = async (e: React.FormEvent) => {
    e.preventDefault();
    if (password !== confirmPassword) {
      setErrors({ confirmPassword: "Passwords do not match" });
      return;
    }

    const requestData: SignUpRequest = {
      name,
      email,
      userName,
      password,
    };

    try {
      await signUp(requestData);
      navigate("/login");
    } catch (error: any) {
      console.error("Registration error:", error.response.data.error);
      const errorMessage: string =
        error?.response?.data?.error || "Registration failed";
      setErrors({ general: errorMessage });
    }
  };

  return (
    <div className="sign-up-page">
      <NavLink to="/" className="sign-up-back-link">
        <IoChevronBackOutline />
      </NavLink>

      <div className="sign-up-card" onSubmit={handleRegister}>
        <h1>Sign up</h1>

        <form className="sign-up-form">
          <div className="sign-up-form-field">
            <input
              type="text"
              id="name"
              name="name"
              required
              placeholder=" "
              value={name}
              onChange={(e) => setName(e.target.value)}
            />
            <label htmlFor="name">Name</label>
          </div>

          <div className="sign-up-form-field">
            <input
              type="text"
              id="username"
              name="username"
              required
              placeholder=" "
              value={userName}
              onChange={(e) => setUserName(e.target.value)}
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
              value={email}
              onChange={(e) => setEmail(e.target.value)}
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
              value={password}
              onChange={(e) => setPassword(e.target.value)}
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
              value={confirmPassword}
              onChange={(e) => setConfirmPassword(e.target.value)}
            />
            <label htmlFor="confirm-password">Confirm Password</label>
            {errors.confirmPassword && (
              <p className="sign-up-error">{errors.confirmPassword}</p>
            )}
          </div>

          <div>
            <p>
              Already have an account?
              <NavLink to="/login" className="sign-up-login-link">
                Log in
              </NavLink>
            </p>
          </div>

          {errors.general && <p className="sign-up-error">{errors.general}</p>}

          <button type="submit">Sign up</button>
        </form>
      </div>
    </div>
  );
};

export default SignUp;
