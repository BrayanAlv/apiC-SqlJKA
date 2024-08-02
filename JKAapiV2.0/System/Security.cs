public class Security
{
    public static bool ValidateToken(string userName, string token)
    {
        if(userName == "sa" && token == "ABC123")
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}