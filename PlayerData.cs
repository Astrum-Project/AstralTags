using Astrum.AstralCore.Types;
using System;
using System.Collections.Generic;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace Astrum
{
    public partial class AstralTags
    {
        public class PlayerData : MonoBehaviour
        {
            static PlayerData() => ClassInjector.RegisterTypeInIl2Cpp<PlayerData>();

            public Player player;

            public List<TagInstance> tags = new();
            public Transform contents;
            public Transform stats;

            public PlayerData() : base(ClassInjector.DerivedConstructorPointer<PlayerData>()) => ClassInjector.DerivedConstructorBody(this);
            public PlayerData(IntPtr ptr) : base(ptr) {}

            ~PlayerData()
            {
                foreach (TagInstance inst in tags)
                    inst.tag.instances.Remove(inst);
            }

            public void Setup()
            {
                contents = player.Inner.transform.Find("Player Nameplate/Canvas/Nameplate/Contents");
                stats = contents.Find("Quick Stats");
            }
        }
    }
}
