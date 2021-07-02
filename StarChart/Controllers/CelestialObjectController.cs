using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [ApiController]
    [Route("")]
    public class CelestialObjectController: ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var result = _context.CelestialObjects.Where(c => c.Id == id)
                                        .SingleOrDefault();

            if(result == null)
                return NotFound();

            var objs = _context.CelestialObjects.Where(c => c.OrbitedObjectId == id);
            result.Satellites = new System.Collections.Generic.List<Models.CelestialObject>();
            foreach (var item in objs)
            {
                result.Satellites.Add(item);
            }

            return Ok(result);
        }

        [HttpGet("{name}", Name = "GetByName")]
        public IActionResult GetByName(string name)
        {
            var result = _context.CelestialObjects.Where(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (!result.Any())
                return NotFound();

            foreach (var celestial in result)
            {
                var objs = _context.CelestialObjects.Where(c => c.OrbitedObjectId == celestial.Id);
                celestial.Satellites = new System.Collections.Generic.List<Models.CelestialObject>();
                foreach (var item in objs)
                {
                    celestial.Satellites.Add(item);
                }
            }

            return Ok(result);
        }

        [HttpGet(Name = "GetAll")]
        public IActionResult GetAll()
        {
            var result = _context.CelestialObjects;

            if (!result.Any())
                return NotFound();

            foreach (var celestial in result)
            {
                var objs = _context.CelestialObjects.Where(c => c.OrbitedObjectId == celestial.Id);
                celestial.Satellites = new System.Collections.Generic.List<Models.CelestialObject>();
                foreach (var item in objs)
                {
                    celestial.Satellites.Add(item);
                }
            }

            return Ok(result);
        }

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject input)
        {
            _context.CelestialObjects.Add(input);
            _context.SaveChanges();

            return CreatedAtRoute("GetById", new { id = input.Id }, input);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CelestialObject input)
        {
            var result = _context.CelestialObjects.Where(c => c.Id == id)
                                        .SingleOrDefault();

            if (result == null)
                return NotFound();

            result.Name = input.Name;
            result.OrbitalPeriod = input.OrbitalPeriod;
            result.OrbitedObjectId = input.OrbitedObjectId;

            _context.Update(result);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var result = _context.CelestialObjects.Where(c => c.Id == id)
                                        .SingleOrDefault();

            if (result == null)
                return NotFound();

            result.Name = name;

            _context.Update(result);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _context.CelestialObjects.Where(c => c.Id == id || c.OrbitedObjectId == id);

            if(!result.Any())
                return NotFound();

            _context.CelestialObjects.RemoveRange(result);
            _context.SaveChanges();

            return NoContent();
        }
    }
}