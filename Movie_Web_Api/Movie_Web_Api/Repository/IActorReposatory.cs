using Movie_Web_Api.Dto;
using Movie_Web_Api.Models;

namespace Movie_Web_Api.Repository
{
    public interface IActorReposatory
    {
        void Delete(int id);
        void Edit(int id, Actor actor);
        List<ActorDto> GetAll();
        Actor GetByActId(int id);
        ActorDto GetByID(int id);
        Task Insert(Actor actor);
    }
}