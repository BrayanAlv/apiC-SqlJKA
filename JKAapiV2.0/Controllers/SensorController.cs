using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using JKAapiV2._0.DTOs;

namespace JKAapiV2._0.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SensorController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Sensor>> GetAllSensors()
        {
            try
            {
                List<Sensor> sensors = Sensor.Get();
                if (sensors.Count > 0)
                {
                    return Ok(new { Status = 0, Message = "Sensors found", Data = sensors });
                }
                return Ok(new { Status = 1, Message = "No sensors found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = 1, Message = $"Internal server error: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Sensor> GetSensor(int id)
        {
            try
            {
                Sensor sensor = Sensor.Get(id);
                if (sensor == null)
                {
                    return NotFound(new { Status = 1, Message = "Sensor not found" });
                }
                return Ok(new { Status = 0, Message = "Sensor found", Data = sensor });
            }
            catch (RecordNotFoundException ex)
            {
                return NotFound(new { Status = 1, Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = 1, Message = $"Internal server error: {ex.Message}" });
            }
        }

        [HttpGet("serial/{serialNumber}")]
        public ActionResult<List<SensorDetail>> GetSensorDetailsBySerialNumber(string serialNumber)
        {
            if (string.IsNullOrWhiteSpace(serialNumber))
            {
                return BadRequest(new { Status = 1, Message = "Serial number is required" });
            }
            try
            {
                List<SensorDetail> sensorDetails = Sensor.GetDetailInfoBySerialNumber(serialNumber);
                if (sensorDetails.Count > 0)
                {
                    return Ok(new { Status = 0, Message = "Sensor details found", Data = sensorDetails });
                }
                return Ok(new { Status = 1, Message = "No sensor details found for the provided serial number" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = 1, Message = $"Internal server error: {ex.Message}" });
            }
        }

        [HttpGet("by-email/{email}")]
        public ActionResult<List<Sensor>> GetSensorsByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest(new { Status = 1, Message = "Email is required" });
            }
            try
            {
                List<Sensor> sensors = Sensor.GetSensorsByEmail(email);
                if (sensors.Count > 0)
                {
                    return Ok(new { Status = 0, Message = "Sensors found", Data = sensors });
                }
                return Ok(new { Status = 1, Message = "No sensors found for the provided email" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = 1, Message = $"Internal server error: {ex.Message}" });
            }
        }

        [HttpGet("top-readings/{serialNumber}")]
        public ActionResult<List<Reading>> GetTopReadingsBySerialNumber(string serialNumber)
        {
            if (string.IsNullOrWhiteSpace(serialNumber))
            {
                return BadRequest(new { Status = 1, Message = "Serial number is required" });
            }
            try
            {
                List<Reading> readings = Sensor.GetTopReadingsBySerialNumber(serialNumber);
                if (readings.Count > 0)
                {
                    return Ok(new { Status = 0, Message = "Readings found", Data = readings });
                }
                return Ok(new { Status = 1, Message = "No readings found for the provided serial number" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = 1, Message = $"Internal server error: {ex.Message}" });
            }
        }
    }
}
