//SensorListResponse.cs
using System;
using System.Collections.Generic;

public class SensorListResponse : JSONResponse
{
    public List<Sensor> Sensors { get; set; }

    public static SensorListResponse Get(List<Sensor> sensors)
    {
        return new SensorListResponse
        {
            Status = 0,
            Sensors = sensors
        };
    }
}