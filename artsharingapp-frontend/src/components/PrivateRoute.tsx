import { JSX, useEffect, useState } from "react";
import { Navigate } from "react-router-dom";
import axios from "axios";
import Loading from "../pages/Loading";

function PrivateRoute({ children }: { children: JSX.Element }) {
  const [auth, setAuth] = useState<boolean | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const accessToken = localStorage.getItem("accessToken");
    if (accessToken) {
      axios
        .get("http://localhost:5125/api/auth/loggedin-user", {
          headers: {
            Authorization: `Bearer ${accessToken}`,
          },
        })
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
