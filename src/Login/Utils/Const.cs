namespace Login.Utils
{
    public enum LogAction
    {
        Unknown = 0,
        Login = 1,
        Logout = 2,
        ChangePassword = 3,
        ResetPassword = 4,

        UserCreate = ResetPassword + 2,//6
        UserEdit,
        UserDelete,
    }

    public class Const
    {
    }
}
