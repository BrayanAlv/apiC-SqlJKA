//SensorResponse.cs
using System;
using System.Collections.Generic;
public class SensorResponse : JSONResponse
{
    public Sensor Sensor { get; set; }

    public static SensorResponse Get(Sensor sensor)
    {
        return new SensorResponse
        {
            Status = 0,
            Sensor = sensor
        };
    }
}