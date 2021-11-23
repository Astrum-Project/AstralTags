using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Player = Astrum.AstralCore.Types.Player;

[assembly: MelonInfo(typeof(Astrum.AstralTags), "AstralTags", "0.2.0", downloadLink: "github.com/Astrum-Project/AstralTags")]
[assembly: MelonGame("VRChat", "VRChat")]
[assembly: MelonColor(ConsoleColor.DarkMagenta)]

namespace Astrum
{
    public partial class AstralTags : MelonMod
    {
        public static bool hasIntegration;
        public static bool moderatorTag = true;
        public static bool masterTag = true;
        public static List<AstralTag> tags = new List<AstralTag>();
        public static List<WeakReference<AstralPlayerTag>> playerTags = new List<WeakReference<AstralPlayerTag>>();

        // example tags
        static AstralTags()
        {
            new AstralTag(new Func<Player, AstralTagData>(
                player => new AstralTagData()
                {
                    enabled = moderatorTag && player.APIUser.hasModerationPowers,
                    text = "Moderator",
                    textColor = Color.red,
                    backgroundColor = Color.black
                }), 200
            );

            new AstralTag(new Func<Player, AstralTagData>(
                player => new AstralTagData()
                {
                    enabled = masterTag && player.VRCPlayerApi.isMaster,
                    text = "Master",
                    textColor = Color.white,
                    backgroundColor = Color.black
                }), 100
            );
        }

        public override void OnApplicationStart()
        {
            AstralCore.Events.OnPlayerJoined += OnPlayerJoined;
            AstralCore.Events.OnPlayerLeft += OnPlayerLeft;

            MelonPreferences_Category category = MelonPreferences.CreateCategory("Astrum-AstralTags", "Astral Tags");
            category.CreateEntry("moderatorTag", true, "Moderator Tag");
            category.CreateEntry("masterTag", true, "Master Tag");

            OnPreferencesLoaded();
        }

        public override void OnPreferencesSaved() => OnPreferencesLoaded();
        public override void OnPreferencesLoaded()
        {
            MelonPreferences_Category category = MelonPreferences.GetCategory("Astrum-AstralTags");
            moderatorTag = category.GetEntry<bool>("moderatorTag").Value;
            masterTag = category.GetEntry<bool>("masterTag").Value;
        }

        private static void OnPlayerJoined(Player player) => player.Inner.gameObject.AddComponent<AstralPlayerTag>();
        private static void OnPlayerLeft(Player player)
        {
            playerTags = playerTags.Where(x => x.TryGetTarget(out AstralPlayerTag target) && target != null).ToList();
            CalculateAll();
        }

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
