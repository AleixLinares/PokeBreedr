using PokeBreedr.Enums;
using PokeBreedr.Dto;

namespace PokeBreedr.Models
{
    public class ConfigCardInfo
    {
        public ConfigCardInfo()
        {

        }

        public ConfigCardInfo(ConfigCardInfoDto dto)
        {
            this.Guid = dto.Guid;
            this.ConfigName = dto.ConfigName;
            this.OnlyEggGroup1 = dto.OnlyEggGroup1;
            this.OnlyEggGroup2 = dto.OnlyEggGroup2;
            this.OnlyAlfa = dto.OnlyAlfa;
            this.SelectedNatures = dto.SelectedNatures;

            this.MinHpIv = dto.MinHpIv;
            this.MaxHpIv = dto.MaxHpIv;

            this.MinAttackIv = dto.MinAttackIv;
            this.MaxAttackIv = dto.MaxAttackIv;

            this.MinDefenseIv = dto.MinDefenseIv;
            this.MaxDefenseIv = dto.MaxDefenseIv;

            this.MinSpAttackIv = dto.MinSpAttackIv;
            this.MaxSpAttackIv = dto.MaxSpAttackIv;

            this.MinSpDefenseIv = dto.MinSpDefenseIv;
            this.MaxSpDefenseIv = dto.MaxSpDefenseIv;

            this.MinSpeedIv = dto.MinSpeedIv;
            this.MaxSpeedIv = dto.MaxSpeedIv;

            this.IsSaved = true;
        }

        public Guid? Guid { get; set; }

        public string? ConfigName { get; set; }

        public string? OnlyEggGroup1;

        public string? OnlyEggGroup2;

        public bool OnlyAlfa;

        public List<string?> SelectedNatures { get; set; } = new();

        public bool IsSaved { get; set; }

        private int _minHpIv = 0;
        private int _maxHpIv = 31;

        private int _minAttackIv = 0;
        private int _maxAttackIv = 31;

        private int _minDefenseIv = 0;
        private int _maxDefenseIv = 31;

        private int _minSpAttackIv = 0;
        private int _maxSpAttackIv = 31;

        private int _minSpDefenseIv = 0;
        private int _maxSpDefenseIv = 31;

        private int _minSpeedIv = 0;
        private int _maxSpeedIv = 31;

        public int MinHpIv
        {
            get => _minHpIv;
            set
            {
                var correctValue = ValidateRange(value, MaxHpIv, nameof(MinHpIv), nameof(MaxHpIv), value);
                _minHpIv = correctValue;
            }
        }

        public int MaxHpIv
        {
            get => _maxHpIv;
            set
            {
                var correctValue = ValidateRange(MinHpIv, value, nameof(MinHpIv), nameof(MaxHpIv), value);
                _maxHpIv = correctValue;
            }
        }

        public int MinAttackIv
        {
            get => _minAttackIv;
            set
            {
                var correctValue = ValidateRange(value, MaxAttackIv, nameof(MinAttackIv), nameof(MaxAttackIv), value);
                _minAttackIv = correctValue;
            }
        }

        public int MaxAttackIv
        {
            get => _maxAttackIv;
            set
            {
                var correctValue = ValidateRange(MinAttackIv, value, nameof(MinAttackIv), nameof(MaxAttackIv), value);
                _maxAttackIv = correctValue;
            }
        }

        public int MinDefenseIv
        {
            get => _minDefenseIv;
            set
            {
                var correctValue = ValidateRange(value, MaxDefenseIv, nameof(MinDefenseIv), nameof(MaxDefenseIv), value);
                _minDefenseIv = correctValue;
            }
        }

        public int MaxDefenseIv
        {
            get => _maxDefenseIv;
            set
            {
                var correctValue = ValidateRange(MinDefenseIv, value, nameof(MinDefenseIv), nameof(MaxDefenseIv), value);
                _maxDefenseIv = correctValue;
            }
        }

        public int MinSpAttackIv
        {
            get => _minSpAttackIv;
            set
            {
                var correctValue = ValidateRange(value, MaxSpAttackIv, nameof(MinSpAttackIv), nameof(MaxSpAttackIv), value);
                _minSpAttackIv = correctValue;
            }
        }

        public int MaxSpAttackIv
        {
            get => _maxSpAttackIv;
            set
            {
                var correctValue = ValidateRange(MinSpAttackIv, value, nameof(MinSpAttackIv), nameof(MaxSpAttackIv), value);
                _maxSpAttackIv = correctValue;
            }
        }


        public int MinSpDefenseIv
        {
            get => _minSpDefenseIv;
            set
            {
                var correctValue = ValidateRange(value, MaxSpDefenseIv, nameof(MinSpDefenseIv), nameof(MaxSpDefenseIv), value);
                _minSpDefenseIv = correctValue;
            }
        }

        public int MaxSpDefenseIv
        {
            get => _maxSpDefenseIv;
            set
            {
                var correctValue = ValidateRange(MinSpDefenseIv, value, nameof(MinSpDefenseIv), nameof(MaxSpDefenseIv), value);
                _maxSpDefenseIv = correctValue;
            }
        }

        public int MinSpeedIv
        {
            get => _minSpeedIv;
            set
            {
                var correctValue = ValidateRange(value, MaxSpeedIv, nameof(MinSpeedIv), nameof(MaxSpeedIv), value);
                _minSpeedIv = correctValue;
            }
        }

        public int MaxSpeedIv
        {
            get => _maxSpeedIv;
            set
            {
                var correctValue = ValidateRange(MinSpeedIv, value, nameof(MinSpeedIv), nameof(MaxSpeedIv), value);
                _maxSpeedIv = correctValue;
            }
        }

        private static int ValidateRange(int min, int max, string minName, string maxName, int desiredValue)
        {
            if (min < 0 || min > 31)
            {
                if (min < 0)
                {
                    return 0;
                }
                return 31;
            }

            if (max < 0 || max > 31)
            {
                if (max < 0)
                {
                    return 0;
                }
                return 31;
            }

            if (min > max)
                return min;

            return desiredValue;
        }
    }
}
