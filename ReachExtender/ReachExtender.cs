using BepInEx;
using BepInEx.Configuration;

namespace ReachExtender {
    [BepInPlugin("Mattdokn.ReachExtender", "ReachExtender", "1.0.1")]
    public class ReachExtenderPlugin : BaseUnityPlugin {

        public static ConfigEntry<float> InteractionDistance;
        public static ConfigEntry<float> DoorInteractionDistance;


        void Awake() {
            InteractionDistance = Config.Bind("Reach Distances", "Loot/Container Interaction Distance", 2f, new ConfigDescription("Default Value is 1.3", new AcceptableValueRange<float>(0f, 20f)));
            DoorInteractionDistance = Config.Bind("Reach Distances", "Door Interaction Distance", 1f, new ConfigDescription("Default Value is 1", new AcceptableValueRange<float>(0f, 5f)));

            EFTHardSettings.Instance.LOOT_RAYCAST_DISTANCE = InteractionDistance.Value;
            EFTHardSettings.Instance.DOOR_RAYCAST_DISTANCE = DoorInteractionDistance.Value;

            Config.SettingChanged += Config_SettingChanged;
        }

        private void Config_SettingChanged(object sender, SettingChangedEventArgs e) {
            EFTHardSettings.Instance.LOOT_RAYCAST_DISTANCE = InteractionDistance.Value;
            EFTHardSettings.Instance.DOOR_RAYCAST_DISTANCE = DoorInteractionDistance.Value;
        }
    }
}
