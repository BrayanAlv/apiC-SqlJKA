//UserListResponse.cs
using System;
using System.Collections.Generic;

public class UserListResponse : JSONResponse
{
    public List<User> Users { get; set; }

    public static UserListResponse Get(List<User> users)
    {
        UserListResponse r = new UserListResponse();
        r.Status = 0;
        r.Users = users;
        return r;
    }
}
