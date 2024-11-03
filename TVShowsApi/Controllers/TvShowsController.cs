using Microsoft.AspNetCore.Mvc;
using TVShowsApi.Models;
using TVShowsApi.Repositories.Interfaces;

namespace TVShowsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TvShowsController : ControllerBase
    {
        private readonly ITvShowRepository _tvShowRepository;

        public TvShowsController(ITvShowRepository tvShowRepository)
        {
            _tvShowRepository = tvShowRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(bool? isFavorite, string search = "", int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var tvShows = await _tvShowRepository.Get(isFavorite, search, pageNumber, pageSize);
                if (tvShows == null || tvShows.Count == 0)
                    return NotFound(new { Message = "No se encontraron programas de televisión." });
                else
                    return Ok(tvShows);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(TvShowCreateDto showDto)
        {
            if (showDto == null || string.IsNullOrWhiteSpace(showDto.Name))
                return BadRequest(new { Message = "Los datos del programa de televisión son inválidos." });

            try
            {
                string result = await _tvShowRepository.Add(showDto);
                if (result == "Conflict")
                    return Conflict(new { Message = "El programa ya existe y no puede ser duplicado." });
                else 
                    return Ok(new { Message = "El programa se agrego correctamente." });
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(TvShowCreateDto showDto, int id)
        {
            if (showDto == null)
                return BadRequest(new { Message = "Los datos del programa de televisión son inválidos." });

            try
            {
                string result = await _tvShowRepository.Update(showDto, id);
                if (result == "Conflict")
                    return Conflict(new { Message = "El programa ya existe y no puede ser duplicado." });
                else if(result == "NotFound")
                    return NotFound(new { Message = "No fue encontrado el programa de televisión." });
                else
                    return Ok(new { Message = "El programa se actualizó correctamente." });
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _tvShowRepository.Delete(id);
                if (result == "NotFound")
                    return NotFound(new { Message = "No fue encontrado el programa de televisión." });
                else
                    return Ok(new { Message = "El programa se elimino correctamente." });
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
