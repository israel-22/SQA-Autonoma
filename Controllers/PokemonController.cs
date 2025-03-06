using Microsoft.AspNetCore.Mvc;
using PokemonMVC.Models;
using PokemonMVC.Services;

public class HomeController : Controller
{
    private readonly PokemonService _pokemonService;

    public HomeController(PokemonService pokemonService)
    {
        _pokemonService = pokemonService;
    }

    // Acción para la vista principal
    public async Task<IActionResult> Index(int page = 1)
    {
        int pageSize = 20;  // Número de Pokémon por página

        // Obtener los Pokémon de la API usando paginación
        var pokemonList = await _pokemonService.GetPokemonsAsync(page, pageSize);

        // Establecer las páginas anteriores y siguientes
        ViewBag.PreviousPage = page > 1 ? page - 1 : (int?)null;
        ViewBag.NextPage = pokemonList.Results.Count == pageSize ? page + 1 : (int?)null;

        // Pasamos el resultado a la vista
        return View(pokemonList);  // Asegúrate de que la vista espera un modelo de tipo PokemonList
    }

    // Acción para ver los detalles de un Pokémon
    public async Task<IActionResult> Details(string name)
    {
        try
        {
            // Obtener el Pokémon por su nombre
            var pokemon = await _pokemonService.GetPokemonDetailsByNameAsync(name);
            if (pokemon == null)
            {
                return NotFound(); // Si no se encuentra el Pokémon, retorna un 404
            }
            return View(pokemon); // Devolver la vista con los detalles del Pokémon
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener los detalles del Pokémon: {ex.Message}");
            return View("Error"); // Retorna una vista de error en caso de excepción
        }
    }
}
