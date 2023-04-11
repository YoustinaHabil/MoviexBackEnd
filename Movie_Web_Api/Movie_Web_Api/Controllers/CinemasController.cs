using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movie_Web_Api.Models;
using Movie_Web_Api.Repository;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Movie_Web_Api.Dto;
using AutoMapper;
using Newtonsoft.Json;

namespace Movie_Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CinemasController : ControllerBase
    {
        ICinamasReprosatory cinemasReprosatory;
        IMapper mapper;
        public CinemasController(ICinamasReprosatory _cinemaReposatory, IMapper mapper) //(AppDbContext context)
        {
            //this.context = context;
            cinemasReprosatory = _cinemaReposatory;
            this.mapper = mapper;
        }
          [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            var allCinemas = cinemasReprosatory.GetAll();
            var jsonData = new { Data = allCinemas };
            var json = JsonConvert.SerializeObject(jsonData);
            return Content(json, "application/json");
            //       return Ok(cinemasReprosatory.GetAll());
            //  return Ok(context.Actors.ToList());
            // Actors.ToList());
            //View(allActors);
        }

        /// ////////////////////////////////////////////////////////////


        [AllowAnonymous]
        [HttpGet("{id}")]
        public ActionResult<Cinema> GetCinemabyid(int id)
        {
            var CinemaDetails = cinemasReprosatory.GetByID(id);
            var jsonData = new { Data = CinemaDetails };
            var json = JsonConvert.SerializeObject(jsonData);
            if (CinemaDetails == null)
            {
                return NotFound();
            }
            return Content(json, "application/json");
        }



        [HttpPost]
        public IActionResult Create([Bind("Name,Logo,Description")] CenimaDto cenimadto)
        {
            var cenima = mapper.Map<Cinema>(cenimadto);
            cinemasReprosatory.Insert(cenima);


            var json = JsonConvert.SerializeObject(cenima);
            //  await dbContext.SaveChangesAsync();
            // var actorDtoResult = imapper.Map<ActorDto>(actor);
            // return CreatedAtAction(nameof(Get), new { id = actorDtoResult.Id }, actorDtoResult);
            return Ok("Created");





        }

       
       

        [HttpPut("update/{id}")]
        public IActionResult Update(int id, CenimaDto cenimaDto)
        {
            if (id != cenimaDto.Id)
                return BadRequest("Update not allowed");

            //  var actorfromdb = dbContext.Actors.FirstOrDefault(a=>a.Id==id);
            var cenimafromdb = cinemasReprosatory.GetByCeID(id);
            if (cenimafromdb == null)
                return BadRequest("Update not allowed");

            //  actorfromdb.LastUpdatedBy = 1;
            //  actorfromdb.LastUpdatedOn = DateTime.Now;
            var actor = mapper.Map(cenimaDto, cenimafromdb);
            //imapper.Map<Actor>(actorfromdb);
            //  mapper.Map(ActorDto, actorfromdb);

            cinemasReprosatory.Edit(id, actor);
            return StatusCode(200);
        }

      


    
        //Get: Actors/Delete/1
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            //Actor actor = actorReposatory.GetByID(id);
            //actorReposatory.GetByID();
            // return Ok(actorReposatory.GetByID(id));
            Cinema c = cinemasReprosatory.GetByCeID(id);
            if (c == null)
            {
                return NotFound("Data not Found");
            }
            else
            {
                try
                {
                    cinemasReprosatory.Delete(id);
                    //  context.Actors.Remove(actor);
                    //context.SaveChanges();
                    return StatusCode(StatusCodes.Status204NoContent);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}

    
