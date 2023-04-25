using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integration_modul.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Integration_modul.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagsController : Controller
    {
        private readonly RwaMoviesContext _context;

        public TagsController(RwaMoviesContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public IActionResult GetTags()
        {
            var tags = _context.Tags.ToList();
            return Ok(tags);
        }

        
        [HttpGet("{id}")]
        public IActionResult GetTag(int id)
        {
            var tag = _context.Tags.Find(id);

            if (tag == null)
            {
                return NotFound();
            }

            return Ok(tag);
        }

        
        [HttpPost]
        public IActionResult CreateTag([FromBody] Tag tag)
        {
            _context.Tags.Add(tag);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetTag), new { id = tag.Id }, tag);
        }

        
        [HttpPut("{id}")]
        public IActionResult UpdateTag(int id, [FromBody] Tag tag)
        {
            if (id != tag.Id)
            {
                return BadRequest();
            }

            _context.Entry(tag).State = EntityState.Modified;
            _context.SaveChanges();

            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public IActionResult DeleteTag(int id)
        {
            var tag = _context.Tags.Find(id);

            if (tag == null)
            {
                return NotFound();
            }

            _context.Tags.Remove(tag);
            _context.SaveChanges();

            return NoContent();
        }
    }
}

