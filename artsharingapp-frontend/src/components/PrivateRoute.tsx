import { JSX, useEffect, useState } from "react";
import { Navigate } from "react-router-dom";
import axios from "axios";
import Loading from "../pages/Loading";
import authAxios from "../services/authAxios";

function PrivateRoute({ children }: { children: JSX.Element }) {
  const [auth, setAuth] = useState<boolean | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const accessToken = localStorage.getItem("accessToken");
    if (accessToken) {
      authAxios
        .get("/auth/loggedin-user")
        .then(() => {
          setAuth(true);
          // TODO: DELETE DELAYS LATER
          setTimeout(() => setLoading(false), 1500);
        })
        .catch(() => {
          setAuth(false);
          setTimeout(() => setLoading(false), 1500);
        });
    } else {
      setAuth(false);
      setTimeout(() => setLoading(false), 1500);
    }
  }, []);

  if (loading) {
    return <Loading />;
  }

  return auth ? children : <Navigate to="/login" />;
}

export default PrivateRoute;
