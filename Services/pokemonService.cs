using Newtonsoft.Json;
using PokemonMVC.Models;

namespace PokemonMVC.Services
{
    public class PokemonService
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://pokeapi.co/api/v2/pokemon/";

        public PokemonService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PokemonList> GetPokemonsAsync(int page, int pageSize)
        {
            try
            {
                // Cálculo de la paginación
                int offset = (page - 1) * pageSize;
                var response = await _httpClient.GetAsync($"{ApiUrl}?limit={pageSize}&offset={offset}");

                if (response.IsSuccessStatusCode)
                {
                    // Obtener la respuesta JSON y deserializarla
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var pokemonList = JsonConvert.DeserializeObject<PokemonList>(jsonResponse);

                    // Obtener detalles de cada Pokémon de manera paralela
                    var tasks = pokemonList.Results.Select(async pokemon =>
                    {
                        // Obtener los detalles de cada Pokémon usando la URL proporcionada
                        var pokemonDetailsResponse = await _httpClient.GetAsync(pokemon.Url);
                        if (pokemonDetailsResponse.IsSuccessStatusCode)
                        {
                            var pokemonDetailsJson = await pokemonDetailsResponse.Content.ReadAsStringAsync();
                            var pokemonDetails = JsonConvert.DeserializeObject<Pokemon>(pokemonDetailsJson);

                            // Asignar los sprites, habilidades y tipos si están presentes
                            pokemon.Sprites = pokemonDetails.Sprites ?? new Sprites { Front_Default = "/default-pokemon.png" };
                            pokemon.Abilities = pokemonDetails.Abilities ?? new List<AbilityWrapper>();
                            pokemon.Types = pokemonDetails.Types ?? new List<TypeWrapper>();
                        }
                        else
                        {
                            // Si no se pudieron obtener los detalles, asignar valores por defecto
                            Console.WriteLine($"No se pudo obtener los detalles del Pokémon {pokemon.Name}");
                            pokemon.Sprites = new Sprites { Front_Default = "/default-pokemon.png" };
                            pokemon.Abilities = new List<AbilityWrapper>();
                            pokemon.Types = new List<TypeWrapper>();
                        }
                    });

                    // Esperar a que todas las solicitudes de detalles se completen
                    await Task.WhenAll(tasks);

                    return pokemonList;
                }
                else
                {
                    throw new HttpRequestException($"Error al obtener datos de la API: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los Pokémon: {ex.Message}");
                return new PokemonList(); // Retornamos una lista vacía en caso de error
            }
        }
    }
}
