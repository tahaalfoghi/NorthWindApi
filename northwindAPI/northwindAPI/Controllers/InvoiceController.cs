using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using northwindAPI.Models;
using northwindAPI.PatternService;

namespace northwindAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController:ControllerBase
    {
        private readonly AppDbContext _context;
        public InvoiceController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("GetInvoices")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> GetInvoices()
        {
             
            
            var invoices = await _context.Invoices.OrderBy(x=>x.OrderId).ToListAsync();
            
            return Ok(invoices);
        }
    }
}