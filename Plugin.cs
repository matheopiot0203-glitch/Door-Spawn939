using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using System.Collections.Generic;

namespace DoorSpawn939
{
    public class Plugin : Plugin<Config>
    {
        public override string Name => "DoorSpawn939";
        public override string Author => "Toi";
        public override string Version => "1.0.0";

        public static Plugin Instance;

        private static readonly HashSet<ItemType> ValidKeycards = new HashSet<ItemType>
        {
            ItemType.KeycardScientist,
            ItemType.KeycardResearchCoordinator,
            ItemType.KeycardFacilityManager,
            ItemType.KeycardChaosInsurgency,
            ItemType.KeycardMilitaryCommander,
            ItemType.KeycardO5
        };

        public override void OnEnabled()
        {
            Instance = this;
            Exiled.Events.Handlers.Player.InteractingDoor += OnDoorInteract;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.InteractingDoor -= OnDoorInteract;
            base.OnDisabled();
        }

        private void OnDoorInteract(InteractingDoorEventArgs ev)
        {
            if (ev.Door.Name != Instance.Config.Scp939DoorName) return;
            if (!ev.IsAllowed) return;

            Player opener = ev.Player;

            if (opener.CurrentItem == null) return;
            if (!ValidKeycards.Contains(opener.CurrentItem.Type)) return;

            opener.Role.Set(RoleTypeId.Scp939, RoleSpawnFlags.None);
            opener.IsOverwatchEnabled = true;

            Log.Info($"{opener.Nickname} a utilisé une carte niveau Scientist et spawn en 939 overwatch !");
        }
    }

    public class Config : Exiled.API.Interfaces.IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
        public string Scp939DoorName { get; set; } = "ARCD939";
    }
}
