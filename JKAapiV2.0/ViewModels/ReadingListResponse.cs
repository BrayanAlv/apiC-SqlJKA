//ReadingListResponse.cs
using System;
using System.Collections.Generic;

public class ReadingListResponse : JSONResponse
{
    public List<Reading> Readings { get; set; }

    public static ReadingListResponse Get(List<Reading> readings)
    {
        return new ReadingListResponse
        {
            Status = 0,
            Readings = readings
        };
    }
}