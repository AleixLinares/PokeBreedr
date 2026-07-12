using PokeBreedr.Enums;
using PokeBreedr.Dto;

namespace PokeBreedr.Models
{
    public class CombinationInfo
    {
        public CombinationInfo()
        {

        }

        public CombinationInfo(PokemonInfoDto pokemon1, PokemonInfoDto pokemon2, byte[] flags)
        {
            if (pokemon1.EggGroup1 == "Ditto")
            {
                this.Pokemon = pokemon2.Pokemon;
                this.HasHiddenAbility = pokemon2.HasHiddenAbility;
                this.EggGroup1 = pokemon2.EggGroup1;
                this.EggGroup2 = pokemon2.EggGroup2;
            }
            else if (pokemon2.EggGroup1 == "Ditto")
            {
                this.Pokemon = pokemon1.Pokemon;
                this.HasHiddenAbility = pokemon1.HasHiddenAbility;
                this.EggGroup1 = pokemon1.EggGroup1;
                this.EggGroup2 = pokemon1.EggGroup2;
            }
            else if (pokemon1.EggGroup1 == "Genderless")
            {
                this.Pokemon = pokemon1.Pokemon;
                this.HasHiddenAbility = pokemon1.HasHiddenAbility || pokemon2.HasHiddenAbility;
                this.EggGroup1 = pokemon1.EggGroup1;
                this.EggGroup2 = pokemon1.EggGroup2;
            }
            else if (pokemon1.Gender == PokemonGenderEnum.Female)
            {
                this.Pokemon = pokemon1.Pokemon;
                this.HasHiddenAbility = pokemon1.HasHiddenAbility;

                this.EggGroup1 = pokemon1.EggGroup1;
                this.EggGroup2 = pokemon1.EggGroup2;
            }
            else
            {
                this.Pokemon = pokemon2.Pokemon;
                this.HasHiddenAbility = pokemon2.HasHiddenAbility;

                this.EggGroup1 = pokemon2.EggGroup1;
                this.EggGroup2 = pokemon2.EggGroup2;
            }

            this.Parent1 = pokemon1.Guid;
            this.Parent2 = pokemon2.Guid;

            this.IsAlfa = pokemon1.IsAlfa && pokemon2.IsAlfa;

            this.Particles = pokemon1.Particles
                .Union(pokemon2.Particles)
                .ToList();

            if (flags[6] == 1)
            {
                this.Nature = pokemon1.Nature;
            }
            else if (flags[6] == 2)
            {
                this.Nature = pokemon2.Nature;
            }

            this.HpIv1 = pokemon1.HpIv;
            this.HpIv2 = pokemon2.HpIv;            

            if (flags[0] == 1)
            {
                this.SelectedHpIv = SelectedIvEnum.Iv1;
            } 
            else if (flags[0] == 2)
            {
                this.SelectedHpIv = SelectedIvEnum.Iv2;
            }

            this.AttackIv1 = pokemon1.AttackIv;
            this.AttackIv2 = pokemon2.AttackIv;

            if (flags[1] == 1)
            {
                this.SelectedAttackIv = SelectedIvEnum.Iv1;
            }
            else if (flags[1] == 2)
            {
                this.SelectedAttackIv = SelectedIvEnum.Iv2;
            }

            this.DefenseIv1 = pokemon1.DefenseIv;
            this.DefenseIv2 = pokemon2.DefenseIv;

            if (flags[2] == 1)
            {
                this.SelectedDefenseIv = SelectedIvEnum.Iv1;
            }
            else if (flags[2] == 2)
            {
                this.SelectedDefenseIv = SelectedIvEnum.Iv2;
            }

            this.SpAttackIv1 = pokemon1.SpAttackIv;
            this.SpAttackIv2 = pokemon2.SpAttackIv;

            if (flags[3] == 1)
            {
                this.SelectedSpAttackIv = SelectedIvEnum.Iv1;
            }
            else if (flags[3] == 2)
            {
                this.SelectedSpAttackIv = SelectedIvEnum.Iv2;
            }

            this.SpDefenseIv1 = pokemon1.SpDefenseIv;
            this.SpDefenseIv2 = pokemon2.SpDefenseIv;

            if (flags[4] == 1)
            {
                this.SelectedSpDefenseIv = SelectedIvEnum.Iv1;
            }
            else if (flags[4] == 2)
            {
                this.SelectedSpDefenseIv = SelectedIvEnum.Iv2;
            }

            this.SpeedIv1 = pokemon1.SpeedIv;
            this.SpeedIv2 = pokemon2.SpeedIv;

            if (flags[5] == 1)
            {
                this.SelectedSpeedIv = SelectedIvEnum.Iv1;
            }
            else if (flags[5] == 2)
            {
                this.SelectedSpeedIv = SelectedIvEnum.Iv2;
            }
        }

        public Guid Parent1 { get; set; }

        public Guid Parent2 { get; set; }

        public string Pokemon { get; set; } = string.Empty;

        public string PokemonEgg { get; set; } = string.Empty;

        public bool IsAlfa { get; set; }

        public bool IsIgnored { get; set; }

        public bool HasHiddenAbility { get; set; }

        public PokemonNatureEnum? Nature { get; set; }

        public List<ParticleEnum> Particles { get; set; } = new();

        public string? EggGroup1 { get; set; } = string.Empty;

        public string? EggGroup2 { get; set; } = string.Empty;

        public byte HpIv1 { get; set; }
        public byte HpIv2 { get; set; }

        public SelectedIvEnum SelectedHpIv { get; set; }

        public byte AttackIv1 { get; set; }
        public byte AttackIv2 { get; set; }
        public SelectedIvEnum SelectedAttackIv { get; set; }

        public byte DefenseIv1 { get; set; }
        public byte DefenseIv2 { get; set; }
        public SelectedIvEnum SelectedDefenseIv { get; set; }

        public byte SpAttackIv1 { get; set; }
        public byte SpAttackIv2 { get; set; }
        public SelectedIvEnum SelectedSpAttackIv { get; set; }

        public byte SpDefenseIv1 { get; set; }
        public byte SpDefenseIv2 { get; set; }
        public SelectedIvEnum SelectedSpDefenseIv { get; set; }

        public byte SpeedIv1 { get; set; }
        public byte SpeedIv2 { get; set; }
        public SelectedIvEnum SelectedSpeedIv { get; set; }
    }
}
