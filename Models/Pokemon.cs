using System.Text.Json.Serialization;

namespace PokemonMVC.Models
{
    // Modelo para un solo Pokémon
    public class Pokemon
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int Height { get; set; }  // Se agregó esta propiedad
        public int Weight { get; set; }  // Se agregó esta propiedad
        public Sprites Sprites { get; set; }
        public List<AbilityWrapper> Abilities { get; set; }
        public List<TypeWrapper> Types { get; set; }
        public string Url { get; set; }

    }

    // Clase para envolver la lista de Pokémon (Resultados)
    public class PokemonList
    {
        public List<Pokemon> Results { get; set; }
    }

    // Clase para manejar los sprites (imágenes)
    public class Sprites
    {
        public string Front_Default { get; set; }
    }

    // Clase para envolver las habilidades de un Pokémon
    public class AbilityWrapper
    {
        public Ability Ability { get; set; }
    }

    // Modelo para la habilidad
    public class Ability
    {
        public string Name { get; set; }
    }

    // Clase para envolver los tipos del Pokémon
    public class TypeWrapper
    {
        public Type Type { get; set; }
    }

    // Modelo para el tipo
    public class Type
    {
        public string Name { get; set; }
    }

    public class TypeContainer
    {
        public TypeInfo Type { get; set; }
    }

    public class TypeInfo
    {
        public string Name { get; set; }
    }

}
