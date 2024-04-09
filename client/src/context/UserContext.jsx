import { createContext, useContext, useState, useEffect } from "react";
import PropTypes from "prop-types";
import UserStore from "../stores/UserStore";

const UserContext = createContext({});

const UserProvider = ({ children }) => {
  const { userDetails } = UserStore();
  const [user, setUser] = useState({});
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchUser = async () => {
      if (userDetails) {
        const response = await fetch(
          `${import.meta.env.VITE_API_URL}/api/auth/user`,
          {
            headers: {
              "Content-Type": "application/json",
              "x-auth-token": `${userDetails.token}`,
            },
          }
        );

        if (response.status === 200) {
          const data = await response.json();
          setUser(data.user);
        } else {
          setUser({});
        }
      } else {
        setUser({});
      }
    };

    setLoading(true);
    fetchUser();
    setLoading(false);
  }, [userDetails]);

  return (
    <UserContext.Provider value={{ user, setUser, loading }}>
      {children}
    </UserContext.Provider>
  );
};

UserProvider.propTypes = {
  children: PropTypes.node.isRequired,
};

export const UserState = () => {
  return useContext(UserContext);
};

export default UserProvider;
