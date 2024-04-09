import { create } from "zustand";

const UserStore = create((set) => ({
  userDetails: JSON.parse(localStorage.getItem("userDetails")) || null,
  loginData: (userDetails) => {
    set({ userDetails }),
      localStorage.setItem("userDetails", JSON.stringify(userDetails));
  },
  loginState: JSON.parse(localStorage.getItem("userDetails")) ? true : false,
  setLoginState: (state) => set({ loginState: state }),
  logout: () => {
    set({ userDetails: null, loginState: false }),
      localStorage.removeItem("userDetails");
  },
}));

export default UserStore;
