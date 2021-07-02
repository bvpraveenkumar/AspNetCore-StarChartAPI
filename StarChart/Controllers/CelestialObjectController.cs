using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

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

        [HttpGet("id:int", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var result = _context.CelestialObjects.Where(c => c.Id == id)
                                        .SingleOrDefault();

            if(result == null)
                return NotFound();

            var objs = _context.CelestialObjects.Where(c => c.OrbitedObjectId == id);
            foreach (var item in objs)
            {
                result.Satellites.Add(item);
            }

            return Ok(result);
        }

        [HttpGet("name:string", Name = "GetByName")]
        public IActionResult GetByName(string name)
        {
            var result = _context.CelestialObjects.Where(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                                                    .SingleOrDefault();

            if (result == null)
                return NotFound();

            var objs = _context.CelestialObjects.Where(c => c.OrbitedObjectId == result.Id);
            foreach (var item in objs)
            {
                result.Satellites.Add(item);
            }

            return Ok(result);
        }

        [HttpGet(Name = "GetAll")]
        public IActionResult GetAll()
        {
            var result = _context.CelestialObjects;

            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}