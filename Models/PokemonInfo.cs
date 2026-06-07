using PokeBreedr.Enums;
using PokeBreedr.Dto;

namespace PokeBreedr.Models
{
    public class PokemonInfo
    {
        public PokemonInfo() 
        {

        }

        public PokemonInfo(PokemonInfoDto dto)
        {
            this.Guid = dto.Guid;
            this.Pokemon = dto.Pokemon;
            this.Name = dto.Name;
            this.EggGroup1 = dto.EggGroup1;
            this.EggGroup2 = dto.EggGroup2;
            this.IsAlfa = dto.IsAlfa;
            this.Gender = dto.Gender;
            this.Nature = dto.Nature;
            this.HpIv = dto.HpIv;
            this.AttackIv = dto.AttackIv;
            this.DefenseIv = dto.DefenseIv;
            this.SpAttackIv = dto.SpAttackIv;
            this.SpDefenseIv = dto.SpDefenseIv;
            this.SpeedIv = dto.SpeedIv;
            this.IsSaved = true;
        }

        public Guid Guid { get; set; } = Guid.NewGuid();

        public string Pokemon = string.Empty;

        public string Name = string.Empty;

        public string? EggGroup1;

        public string? EggGroup2;

        public bool IsAlfa;

        public PokemonGenderEnum? Gender;

        public PokemonNatureEnum? Nature;

        public bool IsSaved { get; set; }

        private byte _hpIv = 0;
        private byte _attackIv = 0;
        private byte _defenseIv = 0;
        private byte _spaAttackIv = 0;
        private byte _spDefenseIv = 0;
        private byte _speedIv = 0;

        public byte HpIv
        {
            get => _hpIv;
            set
            {
                if (value > 31)
                    throw new ArgumentOutOfRangeException(nameof(HpIv), "HpIv debe estar entre 0 y 31.");

                _hpIv = value;
            }
        }

        public byte AttackIv
        {
            get => _attackIv;
            set
            {
                if (value > 31)
                    throw new ArgumentOutOfRangeException(nameof(AttackIv), "AttackIv debe estar entre 0 y 31.");

                _attackIv = value;
            }
        }

        public byte DefenseIv
        {
            get => _defenseIv;
            set
            {
                if (value > 31)
                    throw new ArgumentOutOfRangeException(nameof(DefenseIv), "DefenseIv debe estar entre 0 y 31.");

                _defenseIv = value;
            }
        }

        public byte SpAttackIv
        {
            get => _spaAttackIv;
            set
            {
                if (value > 31)
                    throw new ArgumentOutOfRangeException(nameof(SpAttackIv), "SpAttackIv debe estar entre 0 y 31.");

                _spaAttackIv = value;
            }
        }

        public byte SpDefenseIv
        {
            get => _spDefenseIv;
            set
            {
                if (value > 31)
                    throw new ArgumentOutOfRangeException(nameof(SpDefenseIv), "SpDefenseIv debe estar entre 0 y 31.");

                _spDefenseIv = value;
            }
        }

        public byte SpeedIv
        {
            get => _speedIv;
            set
            {
                if (value > 31)
                    throw new ArgumentOutOfRangeException(nameof(SpeedIv), "SpeedIv debe estar entre 0 y 31.");

                _speedIv = value;
            }
        }
    }
}
