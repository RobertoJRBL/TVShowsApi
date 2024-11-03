using TVShowsApi.Models;

namespace TVShowsApi.Repositories.Interfaces
{
    public interface ITvShowRepository
    {
        Task<List<TvShow>> Get(bool? isFavorite, string search, int pageNumber, int pageSize);
        Task<string> Add(TvShowCreateDto showDto);
        Task<string> Update(TvShowCreateDto showDto, int id);
        Task<string> Delete(int id);
    }
}
