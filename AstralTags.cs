using MelonLoader;
using System;
using System.Collections.Generic;
using UnityEngine;
using Player = Astrum.AstralCore.Types.Player;

[assembly: MelonInfo(typeof(Astrum.AstralTags), "AstralTags", "0.1.1", downloadLink: "github.com/Astrum-Project/AstralTags")]
[assembly: MelonGame("VRChat", "VRChat")]
[assembly: MelonColor(ConsoleColor.DarkMagenta)]

namespace Astrum
{
    public partial class AstralTags : MelonMod
    {
        public static bool hasIntegration;
        public static List<AstralTag> tags = new List<AstralTag>();
        public static List<WeakReference<AstralPlayerTag>> playerTags = new List<WeakReference<AstralPlayerTag>>();

        // example tag
        static AstralTags() => tags.Add(new AstralTag(new Func<Player, AstralTagData>(player => new AstralTagData() { enabled = player.VRCPlayerApi.isMaster, text = "Master", textColor = Color.white, backgroundColor = Color.black }), 100));

        public override void OnApplicationStart()
        {
            AstralCore.Events.OnPlayerJoined += OnPlayerJoined;
            AstralCore.Events.OnPlayerLeft += OnPlayerLeft;
        }

        private static void OnPlayerJoined(Player player) => player.Inner.gameObject.AddComponent<AstralPlayerTag>();
        private static void OnPlayerLeft(Player player) => CalculateAll();

        public static void CalculateAll()
        {
            for (int i = playerTags.Count - 1; i >= 0; i--)
            {
                if (playerTags[i].TryGetTarget(out AstralPlayerTag tag))
                    tag.CalculateTags();
                else playerTags.RemoveAt(i);
            }
        }
    }
}
