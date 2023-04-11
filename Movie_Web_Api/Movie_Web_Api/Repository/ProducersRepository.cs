//using Movie_Web_Api.Models;

//namespace Movie_Web_Api.Repository
//{
//    public class ProducersRepository : IProducersRepository
//    {
//        private readonly AppDbContext _context;
//        public ProducersRepository(AppDbContext context)
//        {
//            _context = context;
//        }

//        public List<Producer> GetAllAsync()
//        {
//            return _context.Producers.ToList();

//        }

//        public List<Producer> GetById(int id) { return _context.Producers.Where(n => n.Id == id).ToList(); }
//        public async Task AddAsync(Producer p)
//        {
//            await _context.Set<Producer>().AddAsync(p);
//            await _context.SaveChangesAsync();
//        }
//        public void Add(Producer p)
//        {
//            _context.Producers.Add(p);
//            _context.SaveChanges();
//        }

//        public void Update(int id, Producer newp)
//        {
//            //Producer oldp = _context.Producers.FirstOrDefault(n => n.Id == id);
//            //oldp.Id = newp.Id;
//            //oldp.FullName = newp.FullName;
//            //oldp.Bio = newp.Bio;
//            //if (newp.ProfilePictureURL == null)
//            //{
//            //    oldp.ProfilePictureURL = oldp.ProfilePictureURL;
//            //}
//            //else
//            //{ oldp.ProfilePictureURL = newp.ProfilePictureURL; }
//            _context.Update(newp);
//            _context.SaveChanges();
//        }

//        public void Delete(int id)
//        {

//            Producer p = _context.Producers.FirstOrDefault(n => n.Id == id);
//            _context.Producers.Remove(p);
//            _context.SaveChanges();
//        }
//    }


//}


using Microsoft.EntityFrameworkCore;
using Movie_Web_Api.Models;
using Movie_Web_Api.Dto;
using Microsoft.Extensions.Logging;

namespace Movie_Web_Api.Repository
{
    public class ProducersRepository : IProducersRepository
    {

        private readonly AppDbContext _context;

        public ProducersRepository(AppDbContext context)
        {
            _context = context;

            //context = new IEntityBase();
        }
        //public List<Producer> GetAll()
        //{
        //    return _context.Producers.ToList();

        //}
        public Producer GetByPrID(int id)
        {
            return _context.Producers.FirstOrDefault(e => e.Id == id);
        }


        //public List<ProducerDto> GetAll()
        //{
        //    // return _context.Actors.ToList();
        //    var producers = _context.Producers
        //               .Select(a => new ProducerDto
        //               {
        //                   Id = a.Id,
        //                   FullName = a.FullName,
        //                   ProfilePictureURL = a.ProfilePictureURL,
        //                   Bio = a.Bio,
        //               })
        //               .ToList();
        //    return producers;
        //}

        public async Task<List<ProducerDto>> GetAll()
        {
            var producers =  _context.Producers
                .Select(a => new ProducerDto
                {
                    Id = a.Id,
                    FullName = a.FullName,
                    ProfilePictureURL = a.ProfilePictureURL,
                    Bio = a.Bio,
                })
                .ToList();
            return  producers;
            
        }



        public ProducerDto GetByID(int id)
        {
            //  return _context.Actors.Include(a=>a.Actors_Movies).FirstOrDefault(e => e.Id == id);


            var producer = _context.Producers.Where(e => e.Id == id)
                .Select(e => new ProducerDto
                {
                    Id = e.Id,
                    FullName = e.FullName,
                    ProfilePictureURL = e.ProfilePictureURL,
                    Bio = e.Bio,


                }
            ).FirstOrDefault();

            return producer;


        }

        public Producer GetByActId(int id)
        {
            return _context.Producers.FirstOrDefault(e => e.Id == id);
        }
        //public async Task Insert(Producer producer)
        //{

        //        await _context.Producers.AddAsync(producer);
        //        await _context.SaveChangesAsync();


        //}

        private static object _lockObject = new object();

        public async Task Insert(Producer producer)
        {
            lock (_lockObject)
            {
                _context.Producers.Add(producer);
                _context.SaveChanges();
            }
        }





        public void Edit(int id, Producer producer)
        {

            Producer oldproducer = _context.Producers.Include(p => p.Movies).FirstOrDefault(e => e.Id == id);

            oldproducer.Id = producer.Id;
            oldproducer.FullName = producer.FullName;
            oldproducer.Bio = producer.Bio;
            if (producer.ProfilePictureURL == null)
            {
                oldproducer.ProfilePictureURL = oldproducer.ProfilePictureURL;
            }
            else
            { oldproducer.ProfilePictureURL = producer.ProfilePictureURL; }
            _context.SaveChanges();

            _context.Update(producer);
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
            Producer producer = GetByPrID(id);
            _context.Producers.Remove(producer);
            _context.SaveChanges();
        }
    }
}


