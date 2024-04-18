using Colossal.Logging;
using Game;
using Game.Modding;
using Game.Prefabs;
using Game.SceneFlow;
using Game.Simulation;
using UnityEngine.PlayerLoop;
using Unity.Entities;
using Colossal.IO.AssetDatabase;
using Game.Debug;
using Game.Settings;
using UnityEngine;
using Game.UI;
using System.Threading.Tasks;
using Colossal.Serialization.Entities;
using UnityEngine.Scripting;



namespace NoPollution
{
    public class Mod : IMod
    {
        public Setting m_Setting;

        public BruceyPollutionSystem _bruceyPollutionSystem;

        public static ILog log = LogManager.GetLogger($"{nameof(NoPollution)}.{nameof(Mod)}").SetShowsErrorsInUI(false);

        public void OnLoad(UpdateSystem updateSystem)
        {

            log.Info(nameof(OnLoad));

            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
                log.Info($"Current mod asset at {asset.path}");

            if (_bruceyPollutionSystem == null)
            {
                _bruceyPollutionSystem = new BruceyPollutionSystem(this);
                log.Info("Pollution System Found");
            }
            World.DefaultGameObjectInjectionWorld.AddSystemManaged(_bruceyPollutionSystem);

            m_Setting = new Setting(this, _bruceyPollutionSystem);
            m_Setting.RegisterInOptionsUI();

            GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(m_Setting));
            AssetDatabase.global.LoadSettings(nameof(NoPollution), m_Setting, new Setting(this, _bruceyPollutionSystem));

            m_Setting.Apply();

            updateSystem.UpdateAt<BruceyPollutionSystem>(SystemUpdatePhase.MainLoop);
            updateSystem.UpdateAt<BruceyPollutionSystem>(SystemUpdatePhase.PrefabUpdate);
            updateSystem.UpdateAt<BruceyPollutionSystem>(SystemUpdatePhase.GameSimulation);
            updateSystem.UpdateAt<BruceyPollutionSystem>(SystemUpdatePhase.ApplyTool);

            
        }



        public void OnDispose()
        {
            log.Info(nameof(OnDispose));
            if (m_Setting != null)
            {
                m_Setting.UnregisterInOptionsUI();
                m_Setting = null;
            }
        }

    }

    public partial class BruceyPollutionSystem : GameSystemBase
    {
        public Mod _mod;
        public NoisePollutionSystem _noisePollutionSystem;
        public NetPollutionSystem _netPollutionSystem;
        public BuildingPollutionAddSystem _buildingPollutionAddSystem;
        public GroundWaterPollutionSystem _groundWaterPollutionSystem;
        public WaterPipePollutionSystem _waterPipePollutionSystem;
        public AirPollutionSystem _airPollutionSystem;

        public BruceyPollutionSystem(Mod mod)
        {
            _mod = mod;
            

        }

        protected override void OnCreate()
        {
            base.OnCreate();
        }


        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {

            base.OnGameLoadingComplete(purpose, mode);

            if (!mode.IsGameOrEditor())

                return;

            if (mode.IsGameOrEditor())
            {

                _noisePollutionSystem = World.GetExistingSystemManaged<NoisePollutionSystem>();
                _netPollutionSystem = World.GetExistingSystemManaged<NetPollutionSystem>();
                _buildingPollutionAddSystem = World.GetExistingSystemManaged<BuildingPollutionAddSystem>();
                _groundWaterPollutionSystem = World.GetExistingSystemManaged<GroundWaterPollutionSystem>();
                _waterPipePollutionSystem = World.GetExistingSystemManaged<WaterPipePollutionSystem>();
                _airPollutionSystem = World.GetExistingSystemManaged<AirPollutionSystem>();

            }

        }

        public void TogglePollution(bool Toggle)
        {
            if (_noisePollutionSystem != null && _netPollutionSystem != null && _buildingPollutionAddSystem !=null && _groundWaterPollutionSystem != null && _waterPipePollutionSystem != null && _airPollutionSystem != null)
            {
                _noisePollutionSystem.Enabled = Toggle;
                _netPollutionSystem.Enabled = Toggle;
                _buildingPollutionAddSystem.Enabled = Toggle;
                _groundWaterPollutionSystem.Enabled = Toggle;
                _waterPipePollutionSystem.Enabled = Toggle;
                _airPollutionSystem.Enabled = Toggle;
                Mod.log.Info("noisePollutionSystem = " + _noisePollutionSystem.Enabled);
                Mod.log.Info("netPollutionSystem = " + _netPollutionSystem.Enabled);

            }
            else
            {
                Mod.log.Info("Something is null");
            }


        }

        protected override void OnUpdate()
        {

        }

        public void OnGameExit()
        {

        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }


    }
}