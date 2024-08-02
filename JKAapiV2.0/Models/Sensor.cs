using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using JKAapiV2._0.DTOs;

public class Sensor
{
    private static string selectAll = @"SELECT SensorId, SerialNumber, UserId
                                        FROM Sensors ORDER BY SerialNumber";

    private static string selectOne = @"SELECT SensorId, SerialNumber, UserId
                                        FROM Sensors WHERE SensorId = @SensorId";

    private static string insertOne = @"INSERT INTO Sensors (SerialNumber)
                                        VALUES (@SerialNumber)";

    private static string detailInfoBySerialNumber = @"SELECT 
                                                            s.SensorId,
                                                            s.SerialNumber,
                                                            r.ReadingId,
                                                            r.ReadingDate,
                                                            r.ReadingTime,
                                                            r.Temperature,
                                                            r.Humidity,
                                                            r.Counter
                                                        FROM 
                                                            Sensors s
                                                        INNER JOIN 
                                                            Readings r ON s.SensorId = r.SensorId
                                                        WHERE 
                                                            s.SerialNumber = @SerialNumber";
    
    private static string selectSensorsByEmail = @"SELECT 
                                                    s.SensorId,
                                                    s.SerialNumber,
                                                    u.UserId
                                                FROM 
                                                    Sensors s
                                                INNER JOIN 
                                                    Users u ON s.UserId = u.UserId
                                                WHERE 
                                                    u.Email = @Email";

    private static string topReadingBySN = @"WITH RankedReadings AS (
        SELECT 
            s.SensorId,
            s.SerialNumber,
            r.ReadingId,
            r.ReadingDate,
            r.ReadingTime,
            r.Temperature,
            r.Humidity,
            r.Counter,
            ROW_NUMBER() OVER (PARTITION BY s.SensorId ORDER BY r.ReadingDate DESC, r.ReadingTime DESC) AS RowNum
        FROM 
            Sensors s
        INNER JOIN 
            Readings r ON s.SensorId = r.SensorId
        WHERE 
            s.SerialNumber = @SerialNumber
    )
    SELECT 
        SensorId,
        SerialNumber,
        ReadingId,
        ReadingDate,
        ReadingTime,
        Temperature,
        Humidity,
        Counter
    FROM 
        RankedReadings
    WHERE 
        RowNum <= 10
    ORDER BY 
        ReadingDate ASC, 
        ReadingTime ASC";

    public int SensorId { get; set; }
    public string SerialNumber { get; set; }
    public int UserId { get; set; }

    public static List<Sensor> Get()
    {
        SqlCommand command = new SqlCommand(selectAll);
        DataTable table = SqlServerConnection.ExecuteQuery(command);
        return Mapper.ToSensorList(table);
    }

    public static Sensor Get(int sensorId)
    {
        SqlCommand command = new SqlCommand(selectOne);
        command.Parameters.AddWithValue("@SensorId", sensorId);
        DataTable table = SqlServerConnection.ExecuteQuery(command);

        if (table.Rows.Count > 0)
            return Mapper.ToSensor(table.Rows[0]);
        else
            throw new RecordNotFoundException("Sensor", sensorId.ToString());
    }

    public static bool Add(Sensor sensor)
    {
        SqlCommand command = new SqlCommand(insertOne);
        command.Parameters.AddWithValue("@SerialNumber", sensor.SerialNumber);

        return SqlServerConnection.ExecuteInsert(command);
    }

    public static List<SensorDetail> GetDetailInfoBySerialNumber(string serialNumber)
    {
        SqlCommand command = new SqlCommand(detailInfoBySerialNumber);
        command.Parameters.AddWithValue("@SerialNumber", serialNumber);
        DataTable table = SqlServerConnection.ExecuteQuery(command);

        return Mapper.ToSensorDetailList(table);
    }

    public static List<Sensor> GetSensorsByEmail(string email)
    {
        SqlCommand command = new SqlCommand(selectSensorsByEmail);
        command.Parameters.AddWithValue("@Email", email);

        DataTable table = SqlServerConnection.ExecuteQuery(command);

        return Mapper.ToSensorList(table);
    }

    public static List<Reading> GetTopReadingsBySerialNumber(string serialNumber)
    {
        SqlCommand command = new SqlCommand(topReadingBySN);
        command.Parameters.AddWithValue("@SerialNumber", serialNumber);

        DataTable table = SqlServerConnection.ExecuteQuery(command);

        return Mapper.ToReadingList(table);
    }
}
