using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/payments")]
[ApiController]
public class PaymentController : ControllerBase
{
    // GET api/payments
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            List<Payment> payments = await Task.Run(() => Payment.Get());
            return Ok(PaymentListResponse.Get(payments));
        }
        catch (Exception ex)
        {
            // Log the exception
            return StatusCode(StatusCodes.Status500InternalServerError, MessageResponse.Get(1, ex.Message));
        }
    }

    // GET api/payments/{paymentFolio}
    [HttpGet("{paymentFolio}")]
    public async Task<IActionResult> Get(int paymentFolio)
    {
        try
        {
            Payment payment = await Task.Run(() => Payment.Get(paymentFolio));
            return Ok(PaymentResponse.Get(payment));
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

    // GET api/payments/recent
    [HttpGet("recent")]
    public async Task<IActionResult> GetRecentPaymentsByEmail([FromQuery] string email)
    {
        /*
        string userName = Request.Headers["username"];
        string token = Request.Headers["token"];

        if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(token))
        {
            return BadRequest(MessageResponse.Get(400, "Missing Security Headers"));
        }

        if (!Security.ValidateToken(userName, token))
        {
            return Unauthorized(MessageResponse.Get(401, "Invalid Token"));
        }

        if (string.IsNullOrEmpty(email))
        {
            return BadRequest(MessageResponse.Get(400, "Email parameter is required."));
        }
        */
        try
        {
            List<Payment> payments = await Task.Run(() => Payment.GetRecentPaymentsByEmail(email));
            return Ok(PaymentListResponse.Get(payments)); // Adapt this response method as needed
        }
        catch (Exception ex)
        {
            // Log the exception
            return StatusCode(StatusCodes.Status500InternalServerError, MessageResponse.Get(1, ex.Message));
        }
    }

    // POST api/payments
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] PaymentDto paymentDto)
    {
        if (paymentDto == null)
        {
            return BadRequest(MessageResponse.Get(400, "Invalid Input"));
        }

        try
        {
            Payment payment = new Payment
            {
                SubscriptionFolio = paymentDto.SubscriptionFolio,
                TransactionDate = paymentDto.TransactionDate,
                Total = paymentDto.Total
            };

            bool result = await Task.Run(() => Payment.Add(payment));
            if (result)
            {
                return CreatedAtAction(nameof(Get), new { paymentFolio = payment.PaymentFolio }, PaymentResponse.Get(payment));
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageResponse.Get(1, "Failed to add payment"));
            }
        }
        catch (Exception ex)
        {
            // Log the exception
            return StatusCode(StatusCodes.Status500InternalServerError, MessageResponse.Get(1, ex.Message));
        }
    }
}
