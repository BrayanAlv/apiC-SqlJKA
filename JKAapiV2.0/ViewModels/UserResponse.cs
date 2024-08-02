//userResponce.cs
using System;
using System.Collections.Generic;

public class UserResponse : JSONResponse
{
    public User User { get; set; }

    public static UserResponse Get(User user)
    {
        UserResponse r = new UserResponse();
        r.Status = 0;
        r.User = user;
        return r;
    }
}