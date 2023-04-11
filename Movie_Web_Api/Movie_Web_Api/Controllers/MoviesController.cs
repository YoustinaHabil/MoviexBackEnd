using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movie_Web_Api.Repository;
using Movie_Web_Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using AutoMapper;
using Movie_Web_Api.Dto;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Movie_Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        IMovieRepository moviesRepository;
        IMapper imapper;
        AppDbContext _context;
        public MoviesController(IMovieRepository _movieRepo,IMapper mapper, AppDbContext context)
        {
            imapper = mapper;
            moviesRepository = _movieRepo;
            _context = context;
        }



        [HttpGet]
        public IActionResult GetAllMovies()
        {
            //    var allMovies = moviesRepository.GetAllAsync();
            //    return Ok(allMovies);


            var allMovies = moviesRepository.GetAllAsync();
            var jsonData = new { Data = allMovies };
            var json = JsonConvert.SerializeObject(jsonData);
            return Content(json, "application/json");

        }


        [HttpGet("filter")]
        public IActionResult Filter(string searchString)
        {
            var allMovies = moviesRepository.GetAllAsync();

            if (!string.IsNullOrEmpty(searchString))
            {
                var filteredResultNew = allMovies.Where(n => string.Equals(n.Name, searchString, System.StringComparison.CurrentCultureIgnoreCase) || string.Equals(n.Description, searchString, System.StringComparison.CurrentCultureIgnoreCase)).ToList();

                return Ok(filteredResultNew);
            }

            return Ok(allMovies);
        }





        [HttpGet("{id}")]

        public IActionResult Details(int id)
        {
            //var movieDetail = moviesRepository.GetMovieById(id);

            //return Ok(movieDetail);




            var movieDetail = moviesRepository.GetMovieById(id);
            var jsonData = new { Data = movieDetail };
            var json = JsonConvert.SerializeObject(jsonData);
            return Content(json, "application/json");
        }

        [HttpPost]
        public async Task<ActionResult> CreateMovie([FromBody] MoviesDto movieDto)
        {
            var movie = moviesRepository.AddNewMovieAsync(movieDto);

            var json = JsonConvert.SerializeObject(movie, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Content(json);
        }

        //[HttpPost]
        //public async Task<ActionResult> CreateMovie([FromBody] MoviesDto movieDto)
        //{
            //var movie = new Movie
            //{
            //    Name = movieDto.Name,
            //    Description = movieDto.Description,
            //    Price = movieDto.Price,
            //    ImageURL = movieDto.ImageURL,
            //    StartDate = movieDto.StartDate,
            //    EndDate = movieDto.EndDate,
            //    MovieCategory = movieDto.MovieCategory,
            //    CinemaId = movieDto.CinemaId,
            //    ProducerId = movieDto.ProducerId,
            //    Actors_Movies = movieDto.ActorIds.Select(actorId => new Actor_Movie { ActorId = actorId }).ToList()
            //};

            //_context.Movies.Add(movie);
            //await _context.SaveChangesAsync();

            //var movie = moviesRepository.AddNewMovieAsync(movieDto);
            //var jsonData = new { Data = movie };
            //var json = JsonConvert.SerializeObject(jsonData);
            //return Ok(json);


            //moviesRepository.AddNewMovieAsync(movieDto);
            //return Ok("Created");
        //}






        //public async Task<IActionResult> Create( NewMovieVM movie,FormFile image)
        //{
        //    movie.ImageURL = "/Images/pexels - alex - azabache - 3879071.jpg";
        //    if (movie.Name != null && movie.Description != null && movie.Price != null
        //         && movie.StartDate != null && movie.EndDate != null && movie.MovieCategory != null
        //         && movie.ActorIds != null && movie.ActorIds.Count > 0 && movie.CinemaId != null
        //         && movie.ProducerId != null)
        //    {
        //        if (image != null && image.Length > 0)
        //        {
        //            var fileName = Path.GetFileName(image.FileName);
        //            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images\\", fileName);

        //            using (var fileStream = new FileStream(filePath, FileMode.Create))
        //            {
        //                image.CopyTo(fileStream);
        //            }

        //            movie.ImageURL = "/Images/" + fileName;
        //        }

        //        await moviesRepository.AddNewMovieAsync(movie);
        //        return Ok();
        //    }
        //    else
        //    {
        //        var movieDropdownsData = moviesRepository.GetNewMovieDropdownsValues();

        //        var cinemas = new SelectList(movieDropdownsData.Cinemas, "Id", "Name");
        //        var producers = new SelectList(movieDropdownsData.Producers, "Id", "FullName");
        //        var actors = new SelectList(movieDropdownsData.Actors, "Id", "FullName");
        //        var enumData = from MovieCategory e in Enum.GetValues(typeof(MovieCategory))
        //                       select new
        //                       {
        //                           Id = (int)e,
        //                           FullName = e.ToString()
        //                       };
        //        var categories = new SelectList(enumData, "Id", "FullName");

        //        var response = new
        //        {
        //            Cinemas = cinemas,
        //            Producers = producers,
        //            Actors = actors,
        //            Categories = categories
        //        };

        //        return BadRequest(response);
        //    }
        //}

        //  [HttpPut("update/{id}")]
        //  public IActionResult Update(int id, MoviesDto movieDto)
        //  {
        //      if (id != movieDto.Id)
        //          return BadRequest("Update not allowed");

        //      //  var actorfromdb = dbContext.Actors.FirstOrDefault(a=>a.Id==id);
        //      //Movie moviefromdb = moviesRepository.GetMovieById(id);
        //      var moviefromdb = _context.Movies.FirstOrDefault(m => m.Id == id);
        //      if (moviefromdb == null)
        //          return BadRequest("Update not allowed");


        ////      var actor = imapper.Map(movieDto, moviefromdb);
        //      //imapper.Map<Actor>(actorfromdb);
        //      //  mapper.Map(ActorDto, actorfromdb);

        //      moviesRepository.UpdateMovieAsync( moviefromdb,movieDto);

        //      return StatusCode(200);
        //  }


        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateMovie(int id, MoviesDto movieDto)
        //{
            //    var movie = await _context.Movies.Include(m => m.Actors_Movies)
            //                            .FirstOrDefaultAsync(m => m.Id == id);

            //    if (movie == null)
            //    {
            //        return NotFound();
            //    }

            //    movie.Name = movieDto.Name;
            //    movie.Description = movieDto.Description;
            //    movie.Price = movieDto.Price;
            //    movie.ImageURL = movieDto.ImageURL;
            //    movie.StartDate = movieDto.StartDate;
            //    movie.EndDate = movieDto.EndDate;
            //    movie.MovieCategory = movieDto.MovieCategory;
            //    movie.CinemaId = movieDto.CinemaId;
            //    movie.ProducerId = movieDto.ProducerId;

            //    movie.Actors_Movies.Clear();
            //    foreach (var actorId in movieDto.ActorIds)
            //    {
            //        movie.Actors_Movies.Add(new Actor_Movie { ActorId = actorId });
            //    }

            //    await _context.SaveChangesAsync();
            //moviesRepository.UpdateMovieAsync(id, movieDto);
            //return Ok("Updated");





            [HttpPut("{id}")]
            public async Task<IActionResult> UpdateMovie(int id, MoviesDto movieDto)
            {
                var movie = moviesRepository.UpdateMovieAsync(id, movieDto);
                var json = JsonConvert.SerializeObject(movie, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return Content(json);
            }





        //[HttpGet("{id}/Edit")]
        ////[HttpGet("{id}")]
        //public async Task<ActionResult> Edit(int id)
        //{
        //    var movieDetails = await moviesRepository.GetMovieById(id);
        //    if (movieDetails == null) return NotFound();

        //    var response = new NewMovieVM()
        //    {
        //        Id = movieDetails.Id,
        //        Name = movieDetails.Name,
        //        Description = movieDetails.Description,
        //        Price = movieDetails.Price,
        //        StartDate = movieDetails.StartDate,
        //        EndDate = movieDetails.EndDate,
        //        ImageURL = movieDetails.ImageURL,
        //        MovieCategory = movieDetails.MovieCategory,
        //        CinemaId = movieDetails.CinemaId,
        //        ProducerId = (int)movieDetails.ProducerId,
        //        ActorIds = movieDetails.Actors_Movies.Select(n => n.ActorId).ToList(),
        //    };

        //    var movieDropdownsData = moviesRepository.GetNewMovieDropdownsValues();
        //    var cinemas = new SelectList(movieDropdownsData.Cinemas, "Id", "Name");
        //    var producers = new SelectList(movieDropdownsData.Producers, "Id", "FullName");
        //    var actors = new SelectList(movieDropdownsData.Actors, "Id", "FullName");
        //    var categories = from MovieCategory e in Enum.GetValues(typeof(MovieCategory))
        //                     select new
        //                     {
        //                         Id = (int)e,
        //                         FullName = e.ToString()
        //                     };
        //    var categoriesList = new SelectList(categories, "Id", "FullName");

        //    //response.Cinemas = cinemas;
        //    //response.Producers = producers;
        //    //response.Actors = actors;
        //    //response.Categories = categoriesList;

        //    return Ok(response);

        //}


        //*********************************************************


        // [HttpPut("{id:int}",Name="Update Movie")]
        //   [HttpPut("{id}")]
        //  [Route("api/movies/id")]
        //[HttpPatch("{id}")]
        //public IActionResult Update(int id, [FromForm] NewMovieVM movie, [FromForm] IFormFile image)
        //{
        //    if (id != movie.Id) return NotFound();

        //    if (movie.Name != null && movie.Description != null && movie.Price != null
        //        && movie.StartDate != null && movie.EndDate != null && movie.MovieCategory != null
        //        && movie.ActorIds != null && movie.ActorIds.Count > 0 && movie.CinemaId != null
        //        && movie.ProducerId != null)
        //    {
        //        if (image != null && image.Length > 0)
        //        {
        //            var fileName = Path.GetFileName(image.FileName);
        //            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images\\", fileName);

        //            using (var fileStream = new FileStream(filePath, FileMode.Create))
        //            {
        //                image.CopyTo(fileStream);
        //            }

        //            movie.ImageURL = "/Images/" + fileName;
        //        }

        //        moviesRepository.UpdateMovieAsync(movie);

        //        return Ok();
        //    }

        //    var movieDropdownsData = moviesRepository.GetNewMovieDropdownsValues();

        //    return BadRequest();
        //}

        //*********************************************************

        [HttpDelete("{id}")]

        public IActionResult Delete(int id)
        {
            moviesRepository.DeleteMovie(id);
            return NoContent();
        }





    }
}
