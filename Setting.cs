using Colossal;
using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;
using Game.UI;
using Game.UI.Widgets;
using System.Collections.Generic;

namespace NoPollution
{
    [FileLocation(nameof(NoPollution))]
    [SettingsUIGroupOrder(kToggleGroup, kButtonGroup)]
    [SettingsUIShowGroupName(kToggleGroup, kButtonGroup)]
    public class Setting : ModSetting
    {

        private readonly Mod _mod;
        private BruceyPollutionSystem _system;
       // private BruceyPollutionSystem _pollutionSystem;
        public bool Toggle1;


        public const string kSection = "Main";

        public const string kButtonGroup = "Button";
        public const string kToggleGroup = "Toggle";

        public Setting(IMod mod, BruceyPollutionSystem bruceySystem) : base(mod)
        {
            _mod = (Mod)mod;
            _system = bruceySystem;

            Toggle1 = Toggle;
        }



        [SettingsUISection(kSection, kToggleGroup)]
        public bool Toggle { get; set; }

        public override void Apply()
        {
            base.Apply();

            if (Toggle == true)
            {
                Toggle1 = true;
                Mod.log.Info("Pollution = " + Toggle1);
            }
            else
            {
                Toggle1 = false;
                Mod.log.Info("Pollution = " + Toggle1);
            }

            if (_system != null)
            {
                _system.TogglePollution(Toggle1);
                Mod.log.Info("Pollution Updated from Apply Method. Pollution = " + Toggle1);
            }
            else
            {
                Mod.log.Info("Pollution or Mod System is null");
            }
        }




        public override void SetDefaults()
        {
            //throw new System.NotImplementedException();
        }
    }

    public class LocaleEN : IDictionarySource
    {
        private readonly Setting m_Setting;
        public LocaleEN(Setting setting)
        {
            m_Setting = setting;
        }
        public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts)
        {
            return new Dictionary<string, string>
            {
                { m_Setting.GetSettingsLocaleID(), "No Pollution" },
                { m_Setting.GetOptionTabLocaleID(Setting.kSection), "Main" },

                { m_Setting.GetOptionGroupLocaleID(Setting.kButtonGroup), "Buttons" },
                { m_Setting.GetOptionGroupLocaleID(Setting.kToggleGroup), "Toggle" },



                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.Toggle)), "Toggle Pollution" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.Toggle)), $"Toggles Pollution On or Off" },

            };
        }

        public void Unload()
        {

        }
    }
}
