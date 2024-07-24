namespace BackendCode.DTOs.LoginModel
{
    public class LoginModel
    {
        //包含登录信息（如用户名和密码）的模型
        public string username { get; set; }
        public string password { get; set; }
    }
    public class RegisterModel
    {
        public string email { get; set; }
        public string password { get; set; }

        public string role { get; set; }
    }

}
