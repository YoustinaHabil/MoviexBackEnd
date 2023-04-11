using Movie_Web_Api.Dto;

namespace Movie_Web_Api.Repository
{
    public interface IMovieRepository
    {
        Task AddNewMovieAsync(MoviesDto movieDto);
        void DeleteMovie(int id);
        List<MoviesDto> GetAllAsync();
        MoviesDto GetMovieById(int id);
        NewMovieDropdownListVM GetNewMovieDropdownsValues();
        Task UpdateMovieAsync(int id, MoviesDto movieDto);
    }
}