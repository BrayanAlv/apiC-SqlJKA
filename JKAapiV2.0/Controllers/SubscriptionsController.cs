using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace JKAapiV2._0.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        // GET: api/subscription/GetAllSubscriptions
        [HttpGet("GetAllSubscriptions")]
        public ActionResult GetAllSubscriptions()
        {
            try
            {
                List<SubscriptionDetail> subscriptions = Subscription.Get();
                if (subscriptions.Count > 0)
                {
                    return Ok(new { Status = 0, Message = "Subscriptions found", Data = subscriptions });
                }
                return Ok(new { Status = 1, Message = "No subscriptions found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = 1, Message = $"Internal server error: {ex.Message}" });
            }
        }

        // GET: api/subscription/{id}
        [HttpGet("{id}")]
        public ActionResult GetSubscriptionById(int id)
        {
            try
            {
                Subscription subscription = Subscription.Get(id);
                return Ok(new { Status = 0, Message = "Subscription found", Data = subscription });
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

        // POST: api/subscription/newSubscription
        [HttpPost("newSubscription")]
        public ActionResult CreateSubscription([FromBody] PostSubscription newSubscription)
        {
            if (newSubscription == null)
            {
                return BadRequest(new { Status = 1, Message = "Invalid subscription data" });
            }
            try
            {
                Subscription subscription = new Subscription
                {
                    UserId = newSubscription.UserId,
                    StartDate = newSubscription.StartDate,
                    EndDate = newSubscription.EndDate
                };

                bool success = Subscription.Add(subscription);
                if (success)
                {
                    return CreatedAtAction(nameof(GetSubscriptionById), new { id = subscription.Folio }, new { Status = 0, Message = "Subscription created successfully", Data = subscription });
                }
                return BadRequest(new { Status = 1, Message = "Subscription could not be added." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = 1, Message = $"Internal server error: {ex.Message}" });
            }
        }

        // GET: api/subscription/byemail?email=email@example.com
        [HttpGet("byemail")]
        public ActionResult GetSubscriptionsByEmail([FromQuery] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest(new { Status = 1, Message = "Email parameter is required" });
            }

            try
            {
                List<SubscriptionDetail> subscriptions = Subscription.GetSubscriptionsByEmail(email);
                if (subscriptions.Count > 0)
                {
                    return Ok(new { Status = 0, Message = "Subscriptions found", Data = subscriptions });
                }
                return Ok(new { Status = 1, Message = "No subscriptions found for the provided email" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = 1, Message = $"Internal server error: {ex.Message}" });
            }
        }
    }
}
