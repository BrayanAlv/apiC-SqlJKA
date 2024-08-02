//ReadingResponse.cs
using System;
using System.Collections.Generic;
public class ReadingResponse : JSONResponse
{
    public Reading Reading { get; set; }

    public static ReadingResponse Get(Reading reading)
    {
        return new ReadingResponse
        {
            Status = 0,
            Reading = reading
        };
    }
}