using PokeBreedr.Enums;
using PokeBreedr.Models;

namespace PokeBreedr.Dto
{
    public class PokemonInfoDto
    {
        public PokemonInfoDto()
        {

        }

        public PokemonInfoDto(PokemonInfo pokemonInfo)  
        { 
            this.Guid = pokemonInfo.Guid;
            this.Pokemon = pokemonInfo.Pokemon;
            this.EggGroup1 = pokemonInfo.EggGroup1;
            this.EggGroup2 = pokemonInfo.EggGroup2;
            this.IsAlfa = pokemonInfo.IsAlfa;
            this.Gender = pokemonInfo.Gender;
            this.Nature = pokemonInfo.Nature;
            this.HpIv = pokemonInfo.HpIv;
            this.AttackIv = pokemonInfo.AttackIv;
            this.DefenseIv = pokemonInfo.DefenseIv;
            this.SpAttackIv = pokemonInfo.SpAttackIv;
            this.SpDefenseIv = pokemonInfo.SpDefenseIv;
            this.SpeedIv = pokemonInfo.SpeedIv;
        }

        public Guid Guid { get; set; }

        public string Pokemon { get; set; } = string.Empty;

        public string? EggGroup1 { get; set; }

        public string? EggGroup2 { get; set; }

        public bool IsAlfa { get; set; }

        public PokemonGenderEnum? Gender { get; set; }

        public PokemonNatureEnum? Nature { get; set; }

        public byte HpIv { get; set; }

        public byte AttackIv { get; set; }

        public byte DefenseIv { get; set; }

        public byte SpAttackIv { get; set; }

        public byte SpDefenseIv { get; set; }

        public byte SpeedIv { get; set; }
    }
}
