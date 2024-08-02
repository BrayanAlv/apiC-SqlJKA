//LocationHistoryListResponse.cs
using System;
using System.Collections.Generic;

public class LocationHistoryListResponse : JSONResponse
{
    public List<LocationHistory> LocationHistories { get; set; }

    public static LocationHistoryListResponse Get(List<LocationHistory> locationHistories)
    {
        return new LocationHistoryListResponse
        {
            Status = 0,
            LocationHistories = locationHistories
        };
    }
}