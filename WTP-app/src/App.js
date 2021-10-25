import React, { Component } from "react";
import { connect } from "react-redux";
import Login from "./components/auth/login";
import Register from "./components/auth/register";
import Home from "./components/board/home";
import Profile from "./components/auth/profile";
import BoardUser from "./components/board/user";
import { Router, Switch, Route, Link } from "react-router-dom";


import "bootstrap/dist/css/bootstrap.min.css";
import "./App.css";

import { logout } from "./actions/auth";
import { clearMessage } from "./actions/message";

import { history } from './helpers/history';
import NavBar from "./components/nav/navBar";
import Manager from "./components/board/manager";

class App extends Component {
  constructor(props) {
    super(props);
    this.logOut = this.logOut.bind(this);

    this.state = {
      showModeratorBoard: false,
      showAdminBoard: false,
      currentUser: undefined,
    };

    history.listen((location) => {
      props.dispatch(clearMessage()); // clear message when changing location
    });
  }

  componentDidMount() {
    const user = this.props.user;

    if (user) {
      this.setState({
        currentUser: user.Role.includes("Employees"),
        showModeratorBoard: user.Role.includes("Manager"),
        showAdminBoard: user.Role.includes("admin"),
      });
    }
  }

  logOut() {
    this.props.dispatch(logout());
  }

  render() {
    const { currentUser, showModeratorBoard, showAdminBoard } = this.state;

    return (

      <dvi>
        {showModeratorBoard ?
          <Manager props={this.props}
            logout={this.logOut}
            state={this.state} />
          : (<NavBar />)}
        <Route path={["/", "/home"]} render={() => <Home />} />
        <Route path="/login" render={() => <Login />} />
        <Route path="/register" render={() => <Register />} />
        <Route path="/profile" render={() => <Profile />} />
        <Route path="/user" render={() => <BoardUser />} />


      </dvi>
    )
  }
}

function mapStateToProps(state) {
  const { user } = state.auth;
  return {
    user,
  };
}

export default connect(mapStateToProps)(App);
