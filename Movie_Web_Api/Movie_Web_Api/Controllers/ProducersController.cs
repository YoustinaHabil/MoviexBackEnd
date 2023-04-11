
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
    public class ProducersController : ControllerBase
    {
       private readonly IProducersRepository producersRepository;
        private readonly IMapper imapper;
        //  private AppDbContext dbContext;
        public ProducersController(IProducersRepository producersRepository, IMapper mapper)
        {
            //   this.dbContext = context;
            this.imapper = mapper;
            this.producersRepository = producersRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult<IEnumerable<Producer>> GetAllProducer()
        {
            //    var allproducers = producersRepository.GetAll();
            //    return Ok(allproducers);


            var allproducers = producersRepository.GetAll();
            var jsonData = new { Data = allproducers };
            var json = JsonConvert.SerializeObject(jsonData);
            return Content(json, "application/json");



        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public ActionResult<Producer> GetProducer(int id)
        {
            //var ProducerDetails = producersRepository.GetByID(id);
            //if (ProducerDetails == null)
            //{
            //    return NotFound();
            //}
            //return Ok(ProducerDetails);



            var ProducerDetails = producersRepository.GetByID(id);
            var jsonData = new { Data = ProducerDetails };
            var json = JsonConvert.SerializeObject(jsonData);
            if (ProducerDetails == null)
            {
                return NotFound();
            }
            return Content(json, "application/json");

        }

        [HttpPost]

        public IActionResult Create([Bind("FullName,ProfilePictureURL,Bio")] ProducerDto producerdto)
        {
            //var producer = imapper.Map<Producer>(producerdto);
            //producersRepository.Insert(producer);
            ////  await dbContext.SaveChangesAsync();
            //// var actorDtoResult = imapper.Map<ActorDto>(actor);
            //// return CreatedAtAction(nameof(Get), new { id = actorDtoResult.Id }, actorDtoResult);
            //return Ok("Created");


            var producer = imapper.Map<Producer>(producerdto);
            producersRepository.Insert(producer);
            var json = JsonConvert.SerializeObject(producer);

            return Ok(json);




        }









 

        [HttpPut("update/{id}")]
        public IActionResult Update(int id, ProducerDto producerdto)
        {
            if (id != producerdto.Id)
                return BadRequest("Update not allowed");

            //  var actorfromdb = dbContext.Actors.FirstOrDefault(a=>a.Id==id);
            var Producerfromdb = producersRepository.GetByActId(id);
            if (Producerfromdb == null)
                return BadRequest("Update not allowed");

            //  actorfromdb.LastUpdatedBy = 1;
            //  actorfromdb.LastUpdatedOn = DateTime.Now;
            var producer = imapper.Map(producerdto, Producerfromdb);
            //imapper.Map<Actor>(actorfromdb);
            //  mapper.Map(ActorDto, actorfromdb);

            producersRepository.Edit(id, producer);
            return StatusCode(200);
        }



        [HttpDelete("{id}")]
        public ActionResult DeleteProducer(int id)
        {
            var producerDetails = producersRepository.GetByID(id);
            if (producerDetails == null)
            {
                return NotFound();
            }

            producersRepository.Delete(id);

            return NoContent();
        }
    }
}
