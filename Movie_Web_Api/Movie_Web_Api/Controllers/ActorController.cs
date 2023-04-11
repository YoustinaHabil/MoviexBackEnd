
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movie_Web_Api.Models;
using Movie_Web_Api.Repository;
using System.Collections.Generic;
using Movie_Web_Api.Dto;
using AutoMapper;
using Newtonsoft.Json;

namespace Movie_Web_Api.Controllers
{
   // [Authorize(Roles = UserRoles.Admin)]
    [ApiController]
    [Route("api/[controller]")]
    public class ActorsController : ControllerBase
    {
        private readonly IActorReposatory actorReposatory;
        private readonly IMapper imapper;
      //  private AppDbContext dbContext;
        public ActorsController(IActorReposatory actorReposatory,IMapper mapper)
        {
         //   this.dbContext = context;
            this.imapper = mapper;
            this.actorReposatory = actorReposatory;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult<IEnumerable<Actor>> GetAllActors()
        {
           // var allActors = actorReposatory.GetAll();

            var allActors = actorReposatory.GetAll();
            var jsonData = new { Data = allActors };
            var json = JsonConvert.SerializeObject(jsonData);


            return Ok(json);

        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public ActionResult<Actor> GetActor(int id)
        {
           



            var actorDetails = actorReposatory.GetByID(id);
            var jsonData = new { Data = actorDetails };
            var json = JsonConvert.SerializeObject(jsonData);
            if (actorDetails == null)
            {
                return NotFound();
            }
            return Content(json, "application/json");


        }

        [HttpPost]

        public IActionResult Create([Bind("FullName,ProfilePictureURL,Bio")] ActorDto actordto)
        {
            var actor = imapper.Map<Actor>(actordto);
           actorReposatory.Insert(actor);
         //  await dbContext.SaveChangesAsync();
           // var actorDtoResult = imapper.Map<ActorDto>(actor);
            // return CreatedAtAction(nameof(Get), new { id = actorDtoResult.Id }, actorDtoResult);
            return Ok("Created");

        }















        [HttpPut("update/{id}")]
        public IActionResult Update(int id, ActorDto actorDto)
        {
            if (id != actorDto.Id)
                return BadRequest("Update not allowed");

          //  var actorfromdb = dbContext.Actors.FirstOrDefault(a=>a.Id==id);
            var actorfromdb =actorReposatory.GetByActId(id);
            if (actorfromdb == null)
                return BadRequest("Update not allowed");

            //  actorfromdb.LastUpdatedBy = 1;
            //  actorfromdb.LastUpdatedOn = DateTime.Now;
            var actor = imapper.Map(actorDto, actorfromdb);
            //imapper.Map<Actor>(actorfromdb);
            //  mapper.Map(ActorDto, actorfromdb);

            actorReposatory.Edit(id, actor);
            return StatusCode(200);
        }

      



        [HttpDelete("{id}")]
        public ActionResult DeleteActor(int id)
        {
            var actorDetails = actorReposatory.GetByID(id);
            if (actorDetails == null)
            {
                return NotFound();
            }

            actorReposatory.Delete(id);

            return NoContent();
        }
    }
}
