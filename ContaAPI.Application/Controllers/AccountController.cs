using System;
using ContaAPI.Domain.Interfaces;
using ContaAPI.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContaAPI.Application.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IServiceAccount _serviceAccount;

        public AccountController(IServiceAccount serviceAccount) =>
            _serviceAccount = serviceAccount;


        [HttpPut("deposit/{userId}")]
        public IActionResult Deposit([FromRoute] Guid userId, [FromBody] UpdateAccountModel accountModel)
        {
            try
            {
                var account = _serviceAccount.Deposit(userId, accountModel);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("withdraw/{userId}")]
        public IActionResult Withdraw([FromRoute] Guid userId, [FromBody] UpdateAccountModel accountModel)
        {
            try
            {
                var account = _serviceAccount.Withdraw(userId, accountModel);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("payment/{userId}")]
        public IActionResult Payment([FromRoute] Guid userId, [FromBody] PaymentAccountModel paymentAccountModel)
        {
            try
            {
                var account = _serviceAccount.Payment(userId, paymentAccountModel);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("monetize")]
        public IActionResult Monetize()
        {
            try
            {
                _serviceAccount.Monetize();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        public IActionResult RecoverAll()
        {
            try
            {
                var accounts = _serviceAccount.RecoverAll();
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("{userId}")]
        public IActionResult Recover([FromRoute] Guid userId)
        {
            try
            {
                var account = _serviceAccount.RecoverByUserId(userId);
                return Ok(account);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}