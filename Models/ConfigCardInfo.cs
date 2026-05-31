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
                _minHpIv = ValidateMinInRange(value, MaxHpIv);
            }
        }

        public int MaxHpIv
        {
            get => _maxHpIv;
            set
            {
                _maxHpIv = ValidateMaxInRange(value, MinHpIv);
            }
        }

        public int MinAttackIv
        {
            get => _minAttackIv;
            set
            {
                _minAttackIv = ValidateMinInRange(value, MaxAttackIv);
            }
        }

        public int MaxAttackIv
        {
            get => _maxAttackIv;
            set
            {
                _maxAttackIv = ValidateMinInRange(value, MinAttackIv);
            }
        }

        public int MinDefenseIv
        {
            get => _minDefenseIv;
            set
            {
                _minDefenseIv = ValidateMinInRange(value, MaxDefenseIv);
            }
        }

        public int MaxDefenseIv
        {
            get => _maxDefenseIv;
            set
            {
                _maxDefenseIv = ValidateMaxInRange(value, MinDefenseIv);
            }
        }

        public int MinSpAttackIv
        {
            get => _minSpAttackIv;
            set
            {
                _minSpAttackIv = ValidateMinInRange(value, MaxSpAttackIv);
            }
        }

        public int MaxSpAttackIv
        {
            get => _maxSpAttackIv;
            set
            {
                _maxSpAttackIv = ValidateMaxInRange(value, MinSpAttackIv);
            }
        }


        public int MinSpDefenseIv
        {
            get => _minSpDefenseIv;
            set
            {
                _minSpDefenseIv = ValidateMinInRange(value, MaxSpDefenseIv);
            }
        }

        public int MaxSpDefenseIv
        {
            get => _maxSpDefenseIv;
            set
            {
                _maxSpDefenseIv = ValidateMaxInRange(value, MinSpDefenseIv);
            }
        }

        public int MinSpeedIv
        {
            get => _minSpeedIv;
            set
            {
                _minSpeedIv = ValidateMinInRange(value, MaxSpeedIv);
            }
        }

        public int MaxSpeedIv
        {
            get => _maxSpeedIv;
            set
            {
                _maxSpeedIv = ValidateMaxInRange(value, MinSpeedIv);
            }
        }


        private static int ValidateMinInRange(int value, int max)
        {
            if (value > max)
            {
                return max;
            }
            if(value < 0)
            {
                return 0;
            }
            if (value > 31)
            {
                return 31;
            }
            return value;
        }


        private static int ValidateMaxInRange(int value, int min)
        {
            if (value < min)
            {
                return min;
            }
            if (value < 0)
            {
                return 0;
            }
            if (value > 31)
            {
                return 31;
            }
            return value;
        }
    }
}
