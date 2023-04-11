using Movie_Web_Api.Dto;
using Movie_Web_Api.Models;

namespace Movie_Web_Api.Repository
{
    public interface ICinamasReprosatory
    {
        void Delete(int id);
        Task Edit(int id, Cinema cinema);
        List<CenimaDto> GetAll();
        Cinema GetByCeID(int id);
        CenimaDto GetByID(int id);
        Task Insert(Cinema cinema);
    }
}