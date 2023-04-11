using Movie_Web_Api.Models;
using Movie_Web_Api.Repository;
using Movie_Web_Api.Dto;
namespace Movie_Web_Api.Repository
{
    public class CinamasReprosatory : ICinamasReprosatory
    {
        private readonly AppDbContext _context;

        public CinamasReprosatory(AppDbContext context)
        {
            _context = context;

            //context = new IEntityBase();
        }
        //public List<Cinema> GetAll()
        //{
        //    return _context.Cinemas.ToList();

        //}
        public Cinema GetByCeID(int id)
        {
            return _context.Cinemas.FirstOrDefault(e => e.Id == id);

            // return _context.Cinemas.Include(e => e.).FirstOrDefault(e => e.Id == id);

        }

        public List<CenimaDto> GetAll()
        {
            // return _context.Actors.ToList();
            var cenimas = _context.Cinemas
                       .Select(a => new CenimaDto
                       {
                           Id = a.Id,
                           Name = a.Name,
                           Logo = a.Logo,
                           Description = a.Description,

                       }).ToList();

            return cenimas;
        }
        public CenimaDto GetByID(int id)
        {
            //  return _context.Actors.Include(a=>a.Actors_Movies).FirstOrDefault(e => e.Id == id);


            var actor = _context.Cinemas.Where(e => e.Id == id)
                .Select(a => new CenimaDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Logo = a.Logo,
                    Description = a.Description,

                }
                 ).FirstOrDefault();

            return actor;


        }







        //public async Task Insert(Cinema cinema)
        //{
        //    _context.Cinemas.Add(cinema);
        //    await _context.SaveChangesAsync();
        //}


        private static object _lockObject = new object();

        public async Task Insert(Cinema cinema)
        {
            lock (_lockObject)
            {
                _context.Cinemas.Add(cinema);
                _context.SaveChanges();
            }
        }





        public async Task Edit(int id, Cinema cinema)
        {
            Cinema oldActor = GetByCeID(id);
            oldActor.Id = cinema.Id;
            oldActor.Name = cinema.Name;
            oldActor.Description = cinema.Description;
            if (cinema.Logo == null)
            {
                oldActor.Logo = oldActor.Logo;
            }
            else
            { oldActor.Logo = cinema.Logo; }
              _context.SaveChanges();
              _context.Update(cinema);
             await _context.SaveChangesAsync();
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
            Cinema act = GetByCeID(id);
            _context.Cinemas.Remove(act);
            _context.SaveChanges();
        }

    }
}

