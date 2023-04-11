//using Movie_Web_Api.Models;

using Movie_Web_Api.Dto;
using Movie_Web_Api.Models;

namespace Movie_Web_Api.Repository
{
    public interface IProducersRepository
    {
        void Delete(int id);
        void Edit(int id, Producer producer);
        Task<List<ProducerDto>>GetAll();
        Producer GetByActId(int id);
        ProducerDto GetByID(int id);
        Producer GetByPrID(int id);
        Task Insert(Producer producer);
    }
}