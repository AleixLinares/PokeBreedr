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

        public Guid Guid { get; set; } = Guid.NewGuid();

        public string? ConfigName { get; set; }

        public string? OnlyEggGroup1;

        public string? OnlyEggGroup2;

        public bool OnlyAlfa;

        public List<string?> SelectedNatures = new List<string?>();

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
                ValidateRange(value, MaxHpIv, nameof(MinHpIv), nameof(MaxHpIv));
                _minHpIv = value;
            }
        }

        public int MaxHpIv
        {
            get => _maxHpIv;
            set
            {
                ValidateRange(MinHpIv, value, nameof(MinHpIv), nameof(MaxHpIv));
                _maxHpIv = value;
            }
        }

        public int MinAttackIv
        {
            get => _minAttackIv;
            set
            {
                ValidateRange(value, MaxAttackIv, nameof(MinAttackIv), nameof(MaxAttackIv));
                _minAttackIv = value;
            }
        }

        public int MaxAttackIv
        {
            get => _maxAttackIv;
            set
            {
                ValidateRange(MinAttackIv, value, nameof(MinAttackIv), nameof(MaxAttackIv));
                _maxAttackIv = value;
            }
        }

        public int MinDefenseIv
        {
            get => _minDefenseIv;
            set
            {
                ValidateRange(value, MaxDefenseIv, nameof(MinDefenseIv), nameof(MaxDefenseIv));
                _minDefenseIv = value;
            }
        }

        public int MaxDefenseIv
        {
            get => _maxDefenseIv;
            set
            {
                ValidateRange(MinDefenseIv, value, nameof(MinDefenseIv), nameof(MaxDefenseIv));
                _maxDefenseIv = value;
            }
        }

        public int MinSpAttackIv
        {
            get => _minSpAttackIv;
            set
            {
                ValidateRange(value, MaxSpAttackIv, nameof(MinSpAttackIv), nameof(MaxSpAttackIv));
                _minSpAttackIv = value;
            }
        }

        public int MaxSpAttackIv
        {
            get => _maxSpAttackIv;
            set
            {
                ValidateRange(MinSpAttackIv, value, nameof(MinSpAttackIv), nameof(MaxSpAttackIv));
                _maxSpAttackIv = value;
            }
        }


        public int MinSpDefenseIv
        {
            get => _minSpDefenseIv;
            set
            {
                ValidateRange(value, MaxSpDefenseIv, nameof(MinSpDefenseIv), nameof(MaxSpDefenseIv));
                _minSpDefenseIv = value;
            }
        }

        public int MaxSpDefenseIv
        {
            get => _maxSpDefenseIv;
            set
            {
                ValidateRange(MinSpDefenseIv, value, nameof(MinSpDefenseIv), nameof(MaxSpDefenseIv));
                _maxSpDefenseIv = value;
            }
        }

        public int MinSpeedIv
        {
            get => _minSpeedIv;
            set
            {
                ValidateRange(value, MaxSpeedIv, nameof(MinSpeedIv), nameof(MaxSpeedIv));
                _minSpeedIv = value;
            }
        }

        public int MaxSpeedIv
        {
            get => _maxSpeedIv;
            set
            {
                ValidateRange(MinSpeedIv, value, nameof(MinSpeedIv), nameof(MaxSpeedIv));
                _maxSpeedIv = value;
            }
        }

        private static void ValidateRange(int min, int max, string minName, string maxName)
        {
            if (min < 0 || min > 31)
                throw new ArgumentOutOfRangeException(minName, $"{minName} debe estar entre 0 y 31.");

            if (max < 0 || max > 31)
                throw new ArgumentOutOfRangeException(maxName, $"{maxName} debe estar entre 0 y 31.");

            if (min > max)
                throw new ArgumentException($"{minName} no puede ser mayor que {maxName}.");
        }
    }
}
