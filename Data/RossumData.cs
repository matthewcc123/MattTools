using System;
namespace MattTools.Data;

public class RossumData
{

    #region Login
    public class LoginCache
    {
        public string username;
        public string key;
    }

    public class LoginForm
    {
        public string username { get; set; }
        public string password { get; set; }
        public string key { get; set; } = null;
    }

    public class LoginData
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class LoginRespone
    {
        public string key { get; set; } = null;
        public string domain { get; set; } = null;
        public string error { get; set; } = null;
    }

    public class LoginBadRequest
    {
        public List<string> non_field_errors { get; set; }
    }
    #endregion

    #region Logout

    public class LogoutRespone
    {
        public bool loggedOut { get; set; }
        public string detail { get; set; }
        public string code { get; set; }
    }

    #endregion

}

