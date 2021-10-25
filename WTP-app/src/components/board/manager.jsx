import Login from "../auth/login";
import Register from "../auth/register";
import Home from "../board/home";
import Profile from "../auth/profile";
import BoardUser from "../board/user";
import { Switch, Route, Link } from "react-router-dom";
import { history } from "../../helpers/history";

import { logout } from "../../actions/auth";

let Manager = (props) => {
  const { Name, Surname, logout} = props.props.user;

  return (
    <>
      <li className="nav-item">
        <Link to={"/mod"} className="nav-link">
          Manager Board
        </Link>
      </li>
      <div className="navbar-nav ml-auto">
        <li className="nav-item">
          <Link to={"/profile"} className="nav-link">
            {Name}
            {Surname}
          </Link>
        </li>
        <li className="nav-item">
          <a href="/login" className="nav-link" onClick={logout}>
            LogOut
          </a>
        </li>
      </div>
    </>
  );
};

export default Manager;
