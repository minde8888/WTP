import axios from 'axios';
import authHeader from './header';

const API_URL = 'http://localhost:8080/v1/api/';

class UserService {
  getPublicContent() {
    return axios.get(API_URL + 'all');
  }

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