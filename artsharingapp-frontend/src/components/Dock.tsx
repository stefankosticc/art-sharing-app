import { FiSearch } from "react-icons/fi";
import { FaMap, FaUser } from "react-icons/fa";
import { NavLink } from "react-router-dom";
import "../styles/Dock.css";
import { FaHouse, FaWindowMaximize } from "react-icons/fa6";

const Dock = () => {
  return (
    <div className="dock">
      <NavLink to={"/home"} title="Home">
        <FaHouse />
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
      <NavLink to={"/map"} title="Map">
        <FaMap />
      </NavLink>
    </div>
  );
};

export default Dock;
