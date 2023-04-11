using Microsoft.EntityFrameworkCore;
using Movie_Web_Api.Models;
using Movie_Web_Api.Dto;


namespace Movie_Web_Api.Repository
{
    public class MovieRepository : IMovieRepository


    //: IMovieRepository
    {
        private readonly AppDbContext _context;

        public MovieRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<MoviesDto> GetAllAsync()
        {


            var movies = _context.Movies.Include(a => a.Actors_Movies)
                       .Select(a => new MoviesDto
                       {
                           Id = a.Id,
                           Name = a.Name,
                           Description = a.Description,
                           Price = a.Price,
                           ImageURL = a.ImageURL,
                           StartDate = a.StartDate,
                           EndDate = a.EndDate,
                           MovieCategory = a.MovieCategory,
                           //  Actors_Movies=a.Actors_Movies,
                           CinemaId = a.CinemaId,
                           // Cinema=a.Cinema,
                           ProducerId = a.ProducerId,
                           // Producer=a.Producer,
                           ActorIds = a.Actors_Movies
                             .Select(am => am.ActorId)
                               .ToList()
                       }).ToList();
            return movies;




        }


        public MoviesDto GetMovieById(int id)
        {
            //     Actor ActorIdFromdb= _context.Actors.Include(a=>a.Actors_Movies).ThenInclude(a => a.Actor).FirstOrDefault(e => e.Id == id);

            var actorIds = _context.Actors_Movies
                         .Where(am => am.MovieId == id)
                         .Select(am => am.ActorId)
                         .ToList();


            var movie = _context.Movies.Where(e => e.Id == id).Include(a => a.Actors_Movies).ThenInclude(a => a.Actor)
                .Select(a => new MoviesDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    Price = a.Price,
                    ImageURL = a.ImageURL,
                    StartDate = a.StartDate,
                    EndDate = a.EndDate,
                    MovieCategory = a.MovieCategory,
                    //  Actors_Movies=a.Actors_Movies,
                    CinemaId = a.CinemaId,
                    // Cinema=a.Cinema,
                    ProducerId = a.ProducerId,
                    // Producer=a.Producer,
                    ActorIds = actorIds

                }
            ).FirstOrDefault();

            return movie;



        }




        //public async Task<Movie> GetMovieByIdForCard(int id)
        //{
        //    var movieDetails = await _context.Movies
        //        .Include(c => c.Cinema)
        //        .Include(p => p.Producer)
        //        .Include(am => am.Actors_Movies).ThenInclude(a => a.Actor)
        //        .FirstOrDefaultAsync(n => n.Id == id);

        //    return movieDetails;


        //}

        public async Task AddNewMovieAsync(MoviesDto movieDto)
        {
            var movie = new Movie
            {
                Name = movieDto.Name,
                Description = movieDto.Description,
                Price = movieDto.Price,
                ImageURL = movieDto.ImageURL,
                StartDate = movieDto.StartDate,
                EndDate = movieDto.EndDate,
                MovieCategory = movieDto.MovieCategory,
                CinemaId = movieDto.CinemaId,
                ProducerId = movieDto.ProducerId,
                Actors_Movies = movieDto.ActorIds.Select(actorId => new Actor_Movie { ActorId = actorId }).ToList()
            };

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();







            //var newMovie = new Movie()
            //{   
            //    Name = data.Name,
            //    Description = data.Description,
            //    Price = data.Price,
            //    ImageURL = data.ImageURL,
            //    CinemaId = data.CinemaId,
            //    StartDate = data.StartDate,
            //    EndDate = data.EndDate,
            //    MovieCategory = data.MovieCategory,
            //    ProducerId = data.ProducerId

            //};



            //await _context.Movies.AddAsync(newMovie);
            //await _context.SaveChangesAsync();
            ////   return OK("Created");

            ////Add Movie Actors
            //foreach (var actorId in data.ActorIds)
            //    //{
            //        var newActorMovie = new Actor_Movie()
            //        {
            //            MovieId = data.Id,
            //            ActorId = actorId
            //        };
            //        await _context.Actors_Movies.AddAsync(newActorMovie);
            //    }
            //    await _context.SaveChangesAsync();
        }



        /// ////////////////////////////////////////////////////////////////
 
        public async Task UpdateMovieAsync(int id, MoviesDto movieDto)
        {
           
                var movie =  _context.Movies.Include(m => m.Actors_Movies).FirstOrDefault(m => m.Id == id);
                if (movie != null)
                {
                    movie.Name = movieDto.Name;
                    movie.Description = movieDto.Description;
                    movie.Price = movieDto.Price;
                    movie.ImageURL = movieDto.ImageURL;
                    movie.StartDate = movieDto.StartDate;
                    movie.EndDate = movieDto.EndDate;
                    movie.MovieCategory = movieDto.MovieCategory;
                    movie.CinemaId = movieDto.CinemaId;
                    movie.ProducerId = movieDto.ProducerId;

                    movie.Actors_Movies.Clear();
                    foreach (var actorId in movieDto.ActorIds)
                    {
                        movie.Actors_Movies.Add(new Actor_Movie { ActorId = actorId });
                    }

                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception($"Movie with id {id} not found.");
                }
            
           
        }





        ////public async Task UpdateMovieAsync(int id, MoviesDto movieDto)
        ////{
        ////    //  var dbMovie = await _context.Movies.FirstOrDefaultAsync(n => n.Id == data.Id);


        ////    var movie = await _context.Movies.Include(m => m.Actors_Movies).FirstOrDefaultAsync(m => m.Id == id);






        ////        movie.Name = movieDto.Name;
        ////        movie.Description = movieDto.Description;
        ////        movie.Price = movieDto.Price;
        ////        movie.ImageURL = movieDto.ImageURL;
        ////        movie.StartDate = movieDto.StartDate;
        ////        movie.EndDate = movieDto.EndDate;
        ////        movie.MovieCategory = movieDto.MovieCategory;
        ////        movie.CinemaId = movieDto.CinemaId;
        ////        movie.ProducerId = movieDto.ProducerId;

        ////        movie.Actors_Movies.Clear();
        ////        foreach (var actorId in movieDto.ActorIds)
        ////        {
        ////            movie.Actors_Movies.Add(new Actor_Movie { ActorId = actorId });
        ////        }

        ////        await _context.SaveChangesAsync();



        //if (dbMovie != null)
        //{
        //    dbMovie.Name = data.Name;
        //    dbMovie.Description = data.Description;
        //    dbMovie.Price = data.Price;
        //    dbMovie.CinemaId = data.CinemaId;
        //    dbMovie.StartDate = data.StartDate;
        //    dbMovie.EndDate = data.EndDate;
        //    dbMovie.MovieCategory = data.MovieCategory;
        //    dbMovie.ProducerId = data.ProducerId;
        //    if (data.ImageURL == null)
        //    {
        //        dbMovie.ImageURL = dbMovie.ImageURL;
        //    }
        //    else
        //    {
        //        dbMovie.ImageURL = data.ImageURL;
        //    }

        //    _context.Update(dbMovie);
        //    await _context.SaveChangesAsync();


        //    //Remove existing actors
        //    var existingActorsDb = _context.Actors_Movies.Where(n => n.MovieId == data.Id).ToList();
        //    _context.Actors_Movies.RemoveRange(existingActorsDb);
        //    await _context.SaveChangesAsync();

        //    //Add Movie Actors
        //    foreach (var actorId in data.ActorIds)
        //    {
        //        var newActorMovie = new Actor_Movie()
        //        {
        //            MovieId = data.Id,
        //            ActorId = actorId
        //        };
        //        await _context.Actors_Movies.AddAsync(newActorMovie);
        //    }
        //    await _context.SaveChangesAsync();
        //}

        ////   }
        public void DeleteMovie(int id)
        {
            var movie = _context.Movies.FirstOrDefault(m => m.Id == id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                _context.SaveChanges();
            }
        }
        public NewMovieDropdownListVM GetNewMovieDropdownsValues()
        {
            var response = new NewMovieDropdownListVM()
            {
                Actors = _context.Actors.OrderBy(n => n.FullName).ToList(),
                Cinemas = _context.Cinemas.OrderBy(n => n.Name).ToList(),
                Producers = _context.Producers.OrderBy(n => n.FullName).ToList()
            };
            return response;
        }
    }
}
