using System;
using ContaAPI.Domain.Interfaces;
using ContaAPI.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContaAPI.Application.Controllers
{
    [Route("api/historic")]
    [ApiController]
    public class HistoricController : Controller
    {
        private readonly IServiceHistoric _serviceHistoric;

        public HistoricController(IServiceHistoric serviceHistoric) =>
            _serviceHistoric = serviceHistoric;

        [HttpGet]
        public IActionResult RecoverAll()
        {
            try
            {
                var historic = _serviceHistoric.RecoverAll();
                return Ok(historic);
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
                var historic = _serviceHistoric.RecoverByUserId(userId);
                return Ok(historic);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}