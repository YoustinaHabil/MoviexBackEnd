using Microsoft.EntityFrameworkCore;
using Movie_Web_Api.Models;
using Movie_Web_Api.Dto;
namespace Movie_Web_Api.Repository
{
    public class ActorReposatory : IActorReposatory
    {
        private readonly AppDbContext _context;

        public ActorReposatory(AppDbContext context)
        {
            _context = context;

            //context = new IEntityBase();
        }
        public List<ActorDto> GetAll()
        {
            // return _context.Actors.ToList();
            var actors = _context.Actors
                       .Select(a => new ActorDto
                       {
                           Id = a.Id,
                           FullName = a.FullName,
                           ProfilePictureURL = a.ProfilePictureURL,
                           Bio = a.Bio,
                       })
                       .ToList();
            return actors;
        }
        public ActorDto GetByID(int id)
        {
            //  return _context.Actors.Include(a=>a.Actors_Movies).FirstOrDefault(e => e.Id == id);


            var actor = _context.Actors.Where(e => e.Id == id)
                .Select(e => new ActorDto
                {
                    Id = e.Id,
                    FullName = e.FullName,
                    ProfilePictureURL = e.ProfilePictureURL,
                    Bio = e.Bio,


                }
            ).FirstOrDefault();

            return actor;


        }
        public Actor GetByActId(int id)
        {
            return _context.Actors.FirstOrDefault(e => e.Id == id);
        }
        //public async Task Insert(Actor actor)
        //{

        //    _context.Actors.Add(actor);
        //    await _context.SaveChangesAsync();
        //}

        private static object _lockObject = new object();

        public async Task Insert(Actor actor)
        {
            lock (_lockObject)
            {
                _context.Actors.Add(actor);
                _context.SaveChanges();
            }
        }




        public void Edit(int id, Actor actor)
        {

            Actor oldActor = _context.Actors.Include(e => e.Actors_Movies).FirstOrDefault(e => e.Id == id);

            oldActor.Id = actor.Id;
            oldActor.FullName = actor.FullName;
            oldActor.Bio = actor.Bio;
            if (actor.ProfilePictureURL == null)
            {
                oldActor.ProfilePictureURL = oldActor.ProfilePictureURL;
            }
            else
            { oldActor.ProfilePictureURL = actor.ProfilePictureURL; }
            _context.SaveChanges();

            _context.Update(actor);
            _context.SaveChanges();
            //Actor oldActor = GetByID(id);
            //oldActor.Id = actor.Id;
            //oldActor.FullName = actor.FullName;
            //oldActor.Bio = actor.Bio;
            //if (actor.ProfilePictureURL == null)
            //{
            //    oldActor.ProfilePictureURL = oldActor.ProfilePictureURL;
            //}
            //else
            //{ oldActor.ProfilePictureURL = actor.ProfilePictureURL; }
            //_context.SaveChanges();
        }
        public void Delete(int id)
        {
            Actor act = GetByActId(id);
            _context.Actors.Remove(act);
            _context.SaveChanges();
        }


    }
}
