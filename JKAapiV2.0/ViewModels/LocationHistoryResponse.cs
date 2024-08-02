//LocationHistoryResponse.cs
using System;
using System.Collections.Generic;
public class LocationHistoryResponse : JSONResponse
{
    public LocationHistory LocationHistory { get; set; }

    public static LocationHistoryResponse Get(LocationHistory locationHistory)
    {
        return new LocationHistoryResponse
        {
            Status = 0,
            LocationHistory = locationHistory
        };
    }
}

