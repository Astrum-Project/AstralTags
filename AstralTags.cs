using Astrum.AstralCore.Types;
using MelonLoader;
using System;
using System.Reflection;
using UnityEngine;
using VRC.SDKBase;

[assembly: MelonInfo(typeof(Astrum.AstralTags), "AstralTags", "1.0.0", downloadLink: "github.com/Astrum-Project/AstralTags")]
[assembly: MelonGame("VRChat", "VRChat")]
[assembly: MelonColor(ConsoleColor.DarkMagenta)]

namespace Astrum
{
    public partial class AstralTags : MelonMod
    {
        private static readonly MethodInfo m_GetComponent = null;

        public override void OnApplicationStart()
        {
            typeof(MonoBehaviour).GetMethod(nameof(MonoBehaviour.GetComponent), new Type[0] { }).MakeGenericMethod(Player.Type);

            AstralCore.Events.OnPlayerJoined += player => SetupPlayer(player);

            BuiltInTags.Initialize();
        }

        public override void OnSceneWasLoaded(int index, string _)
        {
            if (index != -1) return;

            MelonCoroutines.Start(WaitForLocalLoad());
        }

        private static System.Collections.IEnumerator WaitForLocalLoad()
        {
            while (Networking.LocalPlayer is null)
                yield return null;

            foreach (VRCPlayerApi player in VRCPlayerApi.AllPlayers)
                SetupPlayer(new Player((MonoBehaviour)m_GetComponent.Invoke(null, new object[1] { player })));
        }

        private static void SetupPlayer(Player player)
        {
            PlayerData data = player.Inner.gameObject.AddComponent<PlayerData>();
            data.player = player;
            data.Setup();

            Tag.OnTagRegistered += tag => data.Create(tag);

            foreach (Tag tag in Tag.tags)
                data.Create(tag);
        }
    }
}
