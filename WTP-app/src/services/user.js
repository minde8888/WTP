import axios from 'axios';
import authHeader from './header';

const API_URL = 'https://localhost:44395/v1/api/';

class UserService {
  // getPublicContent() {
  //   return axios.get(API_URL);
  // }

  getUserBoard() {
    return axios.get(API_URL + 'employee', { headers: authHeader() });
  }

  getModeratorBoard() {
    return axios.get(API_URL + 'manager', { headers: authHeader() });
  }

  getAdminBoard() {
    return axios.get(API_URL + 'admin', { headers: authHeader() });
  }
}

export default new UserService();