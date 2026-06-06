using PokeBreedr.Enums;
using PokeBreedr.Models;

namespace PokeBreedr.Dto
{
    public class ConfigCardInfoDto
    {
        public ConfigCardInfoDto()
        {

        }

        public ConfigCardInfoDto(ConfigCardInfo cardInfo)
        {
            if (!cardInfo.Guid.HasValue)
            {
                this.Guid = Guid.NewGuid();
            }
            else
            {
                this.Guid = cardInfo.Guid.Value;
            }

            this.ConfigName = cardInfo.ConfigName;
            this.OnlyEggGroup1 = cardInfo.OnlyEggGroup1;
            this.OnlyEggGroup2 = cardInfo.OnlyEggGroup2;
            this.AlfaFilter = cardInfo.AlfaFilter;
            this.SelectedNatures = cardInfo.SelectedNatures;

            this.MinHpIv = (byte)cardInfo.MinHpIv;
            this.MaxHpIv = (byte)cardInfo.MaxHpIv;

            this.MinAttackIv = (byte)cardInfo.MinAttackIv;
            this.MaxAttackIv = (byte)cardInfo.MaxAttackIv;

            this.MinDefenseIv = (byte)cardInfo.MinDefenseIv;
            this.MaxDefenseIv = (byte)cardInfo.MaxDefenseIv;

            this.MinSpAttackIv = (byte)cardInfo.MinSpAttackIv;
            this.MaxSpAttackIv = (byte)cardInfo.MaxSpAttackIv;

            this.MinSpDefenseIv = (byte)cardInfo.MinSpDefenseIv;
            this.MaxSpDefenseIv = (byte)cardInfo.MaxSpDefenseIv;

            this.MinSpeedIv = (byte)cardInfo.MinSpeedIv;
            this.MaxSpeedIv = (byte)cardInfo.MaxSpeedIv;
        }

        public Guid Guid { get; set; }

        public string? ConfigName { get; set; }

        public string? OnlyEggGroup1 { get; set; }

        public string? OnlyEggGroup2 { get; set; }

        public AlphaFilterEnum AlfaFilter { get; set; }

        public List<string?> SelectedNatures { get; set; } = new ();

        public byte MinHpIv { get; set; }
        public byte MaxHpIv { get; set; }

        public byte MinAttackIv { get; set; }
        public byte MaxAttackIv { get; set; }

        public byte MinDefenseIv { get; set; }
        public byte MaxDefenseIv { get; set; }
        
        public byte MinSpAttackIv { get; set; }
        public byte MaxSpAttackIv { get; set; }

        public byte MinSpDefenseIv { get; set; }
        public byte MaxSpDefenseIv { get; set; }

        public byte MinSpeedIv { get; set; }
        public byte MaxSpeedIv { get; set; }
    }
}
