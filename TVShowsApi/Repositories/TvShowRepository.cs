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

    public async Task<List<TvShow>> Get(string search = "")
    {
        var tvShows = new List<TvShow>();

        try
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("EXEC sp_listTvShows", connection))
            {
                command.Parameters.AddWithValue("@Search", search);
                command.CommandType = CommandType.StoredProcedure;
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
        }

        return tvShows;
    }

    public async Task<string> Add(TvShow show)
    {
        string res = "";
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("EXEC sp_addTvShow", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Name", show.Name);
                command.Parameters.AddWithValue("@Favorite", show.Favorite);
                command.Parameters.Add("@Msg", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                connection.Open();
                await command.ExecuteNonQueryAsync();

                res = Convert.ToString(command.Parameters["@Msg"].Value)!;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            res = "Error: No se pudo añadir el programa.";
        }
        return res;
    }

    public async Task<string> Update(TvShow show)
    {
        string res = "";
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("EXEC sp_updateTvShow", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", show.Id);
                command.Parameters.AddWithValue("@Name", show.Name);
                command.Parameters.AddWithValue("@Favorite", show.Favorite);
                command.Parameters.Add("@Msg", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                connection.Open();
                await command.ExecuteNonQueryAsync();

                res = Convert.ToString(command.Parameters["@Msg"].Value)!;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            res = "Error: No se pudo actualizar el programa.";
        }
        return res;
    }


    public async Task<string> Delete(int id)
    {
        string res = "";
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("EXEC sp_deleteTvShow", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.Add("@Msg", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                connection.Open();
                await command.ExecuteNonQueryAsync();

                res = Convert.ToString(command.Parameters["@Msg"].Value)!;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            res = "Error: No se pudo eliminar el programa.";
        }
        return res;
    }
}
