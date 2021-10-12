import React, { Component } from "react";
import { Redirect } from 'react-router-dom';
import { connect } from "react-redux";

class  Profile extends Component {

  render() {

    if (!this.props.user) {
      return <Redirect to="/login" />;
    }

    var { 
      Name,
      Surname,
      Role,
      Email,
      MobileNumber,
      Occupation,
      ImageName,
      ImageFile,
      ImageSrc,
      Id
    } = this.props.user;

    return (
      <div className="container">
        <header className="jumbotron">
          <h3>
            <strong>{Name}</strong> Profile
          </h3>
        </header>
        <p>
          {/* <strong>Token:</strong> {currentUser.Token.substring(0, 412)} ...{" "}
          {currentUser.Token.substr(currentUser.Token.length - 414)} */}
        </p>
        <p>
          <strong>Id:</strong> {Id}
        </p>
        <p>
          <strong>Email:</strong> {Email}
        </p>
        <strong>Authorities:</strong> {Role}
      </div>
    );
   
  }
}

function mapStateToProps(state) {
  const { user } = state.auth;
  return {
    user
  };
}

export default connect(mapStateToProps)(Profile);