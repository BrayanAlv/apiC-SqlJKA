using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace JKAapiV2._0.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet("GetAllUsers")]
        public ActionResult GetAllUsers()
        {
            try
            {
                List<User> users = global::User.Get();
                if (users.Count > 0)
                {
                    return Ok(new { Status = 0, Message = "Users found", Data = users });
                }
                return Ok(new { Status = 1, Message = "No users found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = 1, Message = $"Internal server error: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public ActionResult GetUserById(int id)
        {
            try
            {
                User user = global::User.Get(id);
                return Ok(new { Status = 0, Message = "User found", Data = user });
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

        [HttpPost("newUser")]
        public ActionResult CreateUser([FromBody] PostUser newUser)
        {
            try
            {
                bool success = global::User.Add(newUser);
                if (success)
                {
                    return CreatedAtAction(nameof(GetUserById), new { id = newUser }, new { Status = 0, Message = "User created successfully", Data = newUser });
                }
                return BadRequest(new { Status = 1, Message = "User could not be added." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = 1, Message = $"Internal server error: {ex.Message}" });
            }
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] PostLogin login)
        {
            try
            {
                User user = global::User.GetByEmailPass(login);
                return Ok(new { Status = 0, Message = "Login successful", Data = user });
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

        [HttpGet("detailsByEmail")]
        public ActionResult GetUserDetailsByEmail([FromQuery] string email)
        {
            try
            {
                DetailedUser userDetails = global::User.GetDetailedUserByEmail(email);
                return Ok(new { Status = 0, Message = "User details found", Data = userDetails });
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

        [HttpGet("GetAllUsersWithSensors")]
        public ActionResult GetAllUsersWithSensors()
        {
            try
            {
                List<User> users = global::User.GetAllWithSensors();
                foreach (var user in users)
                {
                    if (user.Sensors == null || user.Sensors.Count == 0)
                    {
                        user.Sensors = new List<Sensor> { new Sensor { SerialNumber = "No data found" } };
                    }
                }

                if (users.Count > 0)
                {
                    return Ok(new { Status = 0, Message = "Users with sensors found", Data = users });
                }
                return Ok(new { Status = 1, Message = "No users with sensors found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = 1, Message = $"Internal server error: {ex.Message}" });
            }
        }

        [HttpGet("GetNotApprovedUsers")]
        public ActionResult GetNotApprovedUsers()
        {
            try
            {
                List<User> users = global::User.GetNotApproved();
                if (users.Count > 0)
                {
                    return Ok(new { Status = 0, Message = "Users not approved found", Data = users });
                }
                return Ok(new { Status = 1, Message = "No users not approved found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = 1, Message = $"Internal server error: {ex.Message}" });
            }
        }

        [HttpPut("UpdateApprovalStatus/{id}")]
        public ActionResult UpdateApprovalStatus(int id, [FromBody] bool approval)
        {
            try
            {
                bool success = global::User.UpdateApprovalStatus(id, approval);
                if (success)
                {
                    return Ok(new { Status = 0, Message = "User approval status updated successfully" });
                }
                return BadRequest(new { Status = 1, Message = "User approval status could not be updated." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = 1, Message = $"Internal server error: {ex.Message}" });
            }
        }

        [HttpGet("GetAllUsersAndUsersSU")]
        public ActionResult GetAllUsersAndUsersSU()
        {
            try
            {
                // Obtener todos los usuarios de Users
                List<User> users = global::User.Get();
                
                // Obtener todos los usuarios de UsersSU
                List<UserSU> usersSU = UserSU.GetAllUsersSU();

                // Combinar la información en una sola respuesta
                var combinedData = new
                {
                    Users = users,
                    UsersSU = usersSU
                };

                return Ok(new { Status = 0, Message = "Users and UsersSU found", Data = combinedData });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = 1, Message = $"Internal server error: {ex.Message}" });
            }
        }
    }
}


