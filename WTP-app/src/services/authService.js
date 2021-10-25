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
      })
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