import axios from "axios";

const API_URL = "https://localhost:44395/api/auth";

class AuthService {

  login(username, password) {
    return axios
      .post(API_URL + "/login",
        {
          "email": username,
          "password": password
        })
      .then((response) => {
        response.data.forEach(element => {
          if (element.Token) {
            localStorage.setItem("user", JSON.stringify(element));
          }

        })
        return response.data;
      }).catch(function (error) {
        if (error.response) {
          console.log(error.response.data);
          console.log(error.response.status);
          console.log(error.response.headers);
        } else if (error.request) {
          console.log(error.request);
        } else {
          console.log('Error', error.message);
        }
        console.log(error);
        console.log("Data from the server is not available !!!");
      });;
  }

  logout() {
    localStorage.removeItem("user");
  }

  register(username, email, password) {
    return axios.post(API_URL + "register", {
      username,
      email,
      password,
    });
  }
}

export default new AuthService();