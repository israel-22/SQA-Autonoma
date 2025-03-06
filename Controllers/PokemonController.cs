using Microsoft.AspNetCore.Mvc;
using PokemonMVC.Models;
using PokemonMVC.Services;

public class PokemonController : Controller
{
    private readonly PokemonService _pokemonService;

    public PokemonController(PokemonService pokemonService)
    {
        _pokemonService = pokemonService;
    }

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
}
