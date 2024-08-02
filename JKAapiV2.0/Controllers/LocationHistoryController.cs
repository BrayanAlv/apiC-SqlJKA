using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/locationhistory")]
[ApiController]
public class LocationHistoryController : ControllerBase
{
    // GET api/locationhistory
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            List<LocationHistory> locationHistories = await Task.Run(() => LocationHistory.Get());
            return Ok(LocationHistoryListResponse.Get(locationHistories));
        }
        catch (Exception ex)
        {
            // Log the exception
            return StatusCode(StatusCodes.Status500InternalServerError, MessageResponse.Get(1, ex.Message));
        }
    }

    // GET api/locationhistory/{locationHistoryId}
    [HttpGet("{locationHistoryId}")]
    public async Task<IActionResult> Get(int locationHistoryId)
    {
        try
        {
            LocationHistory locationHistory = await Task.Run(() => LocationHistory.Get(locationHistoryId));
            return Ok(LocationHistoryResponse.Get(locationHistory));
        }
        catch (RecordNotFoundException ex)
        {
            return NotFound(MessageResponse.Get(404, ex.Message));
        }
        catch (Exception ex)
        {
            // Log the exception
            return StatusCode(StatusCodes.Status500InternalServerError, MessageResponse.Get(1, ex.Message));
        }
    }

    // POST api/locationhistory
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] LocationHistoryDto locationHistoryDto)
    {
        if (locationHistoryDto == null)
        {
            return BadRequest(MessageResponse.Get(400, "Invalid Input"));
        }

        try
        {
            LocationHistory locationHistory = new LocationHistory
            {
                SensorId = locationHistoryDto.SensorId,
                Longitude = locationHistoryDto.Longitude,
                Latitude = locationHistoryDto.Latitude,
                Timestamp = locationHistoryDto.Timestamp
            };

            bool result = await Task.Run(() => LocationHistory.Add(locationHistory));
            if (result)
            {
                return CreatedAtAction(nameof(Get), new { locationHistoryId = locationHistory.LocationHistoryId }, LocationHistoryResponse.Get(locationHistory));
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageResponse.Get(1, "Failed to add location history"));
            }
        }
        catch (Exception ex)
        {
            // Log the exception
            return StatusCode(StatusCodes.Status500InternalServerError, MessageResponse.Get(1, ex.Message));
        }
    }
    
    [HttpGet("bySN/{serialNumber}")]
    public async Task<IActionResult> GetBySerialNumber(string serialNumber)
    {
        if (string.IsNullOrEmpty(serialNumber))
        {
            return BadRequest(MessageResponse.Get(400, "SerialNumber cannot be null or empty"));
        }

        try
        {
            List<LocationHistory> locationHistories = await Task.Run(() => LocationHistory.GetBySerialNumber(serialNumber));
            return Ok(LocationHistoryListResponse.Get(locationHistories));
        }
        catch (Exception ex)
        {
            // Log the exception
            return StatusCode(StatusCodes.Status500InternalServerError, MessageResponse.Get(1, ex.Message));
        }
    }
}