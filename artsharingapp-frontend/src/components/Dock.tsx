import { FiSearch } from "react-icons/fi";
import { FaMap, FaUser } from "react-icons/fa";
import { NavLink } from "react-router-dom";
import "../styles/Dock.css";
import { FaHouse, FaWindowMaximize } from "react-icons/fa6";
import { MdOutlineChatBubble } from "react-icons/md";
import { IoNotifications } from "react-icons/io5";

const Dock = () => {
  return (
    <div className="dock">
      <NavLink to={"/notifications"} title="Notifications">
        <IoNotifications />
      </NavLink>
      <NavLink to={"/map"} title="Map">
        <FaMap />
      </NavLink>
      <NavLink to={"/feed"} title="Feed">
        <FaWindowMaximize />
      </NavLink>
      <NavLink to={"/search"} title="Search">
        <FiSearch />
      </NavLink>
      <NavLink to={"/profile"} title="Profile">
        <FaUser />
      </NavLink>
      <NavLink to={"/home"} title="Home">
        <FaHouse />
      </NavLink>
      <NavLink to={"/chat"} title="Chat">
        <MdOutlineChatBubble />
      </NavLink>
    </div>
  );
};

export default Dock;
