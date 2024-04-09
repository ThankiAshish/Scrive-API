import { Outlet } from "react-router-dom";

import GuestHeader from "../components/GuestHeader";
import UserHeader from "../components/UserHeader";

import UserStore from "../stores/UserStore";

const MainLayout = () => {
  const { loginState } = UserStore();

  return (
    <div>
      {loginState ? <UserHeader /> : <GuestHeader />}
      <Outlet />
    </div>
  );
};

export default MainLayout;
