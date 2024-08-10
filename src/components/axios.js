import axios from 'axios';


const axiosInstance = axios.create({
  baseURL: 'https://localhost:7262/api', // 后端服务器的URL，根据实际情况修改
  withCredentials: true, // 允许跨域请求发送 Cookie
  timeout: 30000, // 请求超时时间
  headers: {
    'Content-Type': 'application/json',
  },

}); 

export default axiosInstance;