namespace JKAapiV2._0.DTOs;

public class SensorDetail
{
    public int SensorId { get; set; }
    public string SerialNumber { get; set; }
    public List<Reading> Readings { get; set; } // Incluye una lista de lecturas asociadas
}