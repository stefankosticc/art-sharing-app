import { JSX, useEffect, useState } from "react";
import { Navigate } from "react-router-dom";
import axios from "axios";

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
          setLoading(false);
        })
        .catch(() => {
          setAuth(false);
          setLoading(false);
        });
    } else {
      setAuth(false);
      setLoading(false);
    }
  }, []);

  if (loading) {
    return <p>Loading...</p>;
  }

  return auth ? children : <Navigate to="/login" />;
}

export default PrivateRoute;
