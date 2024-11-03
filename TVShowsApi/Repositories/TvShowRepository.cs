using System.Data;
using System.Data.SqlClient;
using TVShowsApi.Models;
using TVShowsApi.Repositories.Interfaces;
using TVShowsApi.Services;

public class TvShowRepository : ITvShowRepository
{
    private readonly string _connectionString;
    private readonly Logger _logger;

    public TvShowRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        _logger = new Logger();
    }

    public async Task<List<TvShow>> Get(bool? isFavorite, string search, int pageNumber, int pageSize)
    {
        var tvShows = new List<TvShow>();
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_listTvShows", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@IsFavorite", isFavorite as object ?? DBNull.Value);
                command.Parameters.AddWithValue("@Search", search);
                command.Parameters.AddWithValue("@PageNumber", pageNumber);
                command.Parameters.AddWithValue("@PageSize", pageSize);
                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        tvShows.Add(new TvShow
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Favorite = reader.GetBoolean(reader.GetOrdinal("Favorite"))
                        });
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new Exception("Error al obtener la lista de programas de televisión.");
        }

        return tvShows;
    }


    public async Task<string> Add(TvShowCreateDto showDto)
    {
        string res = "";
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_addTvShow", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Name", showDto.Name);
                command.Parameters.AddWithValue("@Favorite", showDto.Favorite);
                command.Parameters.Add("@Msg", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();

                res = Convert.ToString(command.Parameters["@Msg"].Value)!;

                if (res.StartsWith("Error:"))
                    throw new Exception(res);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new Exception("Ocurrió un error al agregar el programa de televisión.");
        }
        return res;
    }

    public async Task<string> Update(TvShowCreateDto showDto, int id)
    {
        string res = "";
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_updateTvShow", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@Name", showDto.Name);
                command.Parameters.AddWithValue("@Favorite", showDto.Favorite);
                command.Parameters.Add("@Msg", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();

                res = Convert.ToString(command.Parameters["@Msg"].Value)!;

                if (res.StartsWith("Error:"))
                    throw new Exception(res);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new Exception("Ocurrió un error al actualizar el programa.");
        }
        return res;
    }

    public async Task<string> Delete(int id)
    {
        string res = "";
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_deleteTvShow", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.Add("@Msg", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();

                res = Convert.ToString(command.Parameters["@Msg"].Value)!;

                if (res.StartsWith("Error:"))
                    throw new Exception(res);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new Exception("Ocurrió un error al eliminar el programa.");
        }
        return res;
    }
}
