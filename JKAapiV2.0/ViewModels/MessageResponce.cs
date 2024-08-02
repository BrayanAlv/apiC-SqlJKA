//MessageResponce.cs
using Microsoft.AspNetCore.Mvc;

public class MessageResponse : JSONResponse
{
    public string Message { get; set; }

    public static MessageResponse Get(int status, string message)
    {
        MessageResponse r = new MessageResponse();
        r.Status = status;
        r.Message = message;
        return r;
    }
}