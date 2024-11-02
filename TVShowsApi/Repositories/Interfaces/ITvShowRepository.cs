using TVShowsApi.Models;

namespace TVShowsApi.Repositories.Interfaces
{
    public interface ITvShowRepository
    {
        Task<List<TvShow>> Get(string search);
        Task<string> Add(TvShow show);
        Task<string> Update(TvShow show);
        Task<string> Delete(int id);
    }
}
