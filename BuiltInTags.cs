using UnityEngine;

namespace Astrum
{
    partial class AstralTags
    {
        public static class BuiltInTags
        {
            public static void Initialize()
            {
                new Tag(new(
                    player => new()
                    {
                        enabled = player.APIUser.hasModerationPowers,
                        text = "Moderator",
                        textColor = Color.red,
                        background = Color.black
                    }), 2000
                );

                Tag masterTag = new(new(
                    player => new()
                    {
                        enabled = player.VRCPlayerApi.isMaster,
                        text = "Master",
                        textColor = Color.white,
                        background = Color.black
                    }), 1000
                );

                AstralCore.Events.OnPlayerLeft += _ => masterTag.CalculateAll();
            }
        }
    }
}
