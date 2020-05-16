using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedPark.Common.Dispatchers;
using MedPark.CustomersService.Model;
using MedPark.CustomersService.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedPark.CustomersService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MedicalSchemesController : ControllerBase
    {
        private readonly IDispatcher _dispatcher;

        public MedicalSchemesController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpGet("{customerid}")]
        public async Task<ActionResult<List<CustomerMedicalSchemeItem>>> GetAllByCustomerId([FromRoute] GetCustomerMedicalSchemes query)
        {
            try
            {
                var medicalSchemes = await _dispatcher.QueryAsync(query);

                return Ok(medicalSchemes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}