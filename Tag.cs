using Astrum.AstralCore.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Astrum
{
    partial class AstralTags
    {
        public class Tag
        {
            public static List<Tag> tags = new();
            public static event Action<Tag> OnTagRegistered = new(_ => { });

            public readonly int priority;
            public readonly List<TagInstance> instances = new();
            internal readonly Func<Player, TagData> evaluator;

            public Tag(Func<Player, TagData> evaluator, int priority = 0)
            {
                this.evaluator = evaluator;
                this.priority = priority;

                tags.Add(this);
                tags = tags.OrderByDescending(tag => tag.priority).ToList();
                OnTagRegistered(this);
            }

            public void CalculateAll() => instances.ForEach(x => x?.Calculate());
        }

        public struct TagData
        {
            public bool enabled;
            public string text;
            public Color textColor;
            public Color background;
        }

        public class TagInstance
        {
            public PlayerData player;

            public Transform parent;
            public ImageThreeSlice image;
            public TMPro.TextMeshProUGUI text;

            public Tag tag;

            public TagInstance(PlayerData player, Tag tag, Transform transform)
            {
                this.player = player;
                this.tag = tag;

                parent = transform.parent;
                image = parent.GetComponent<ImageThreeSlice>();
                text = transform.GetComponent<TMPro.TextMeshProUGUI>();
            }

            public void Calculate()
            {
                if (parent == null) return;

                TagData data = tag.evaluator(player.player);

                if (data.enabled != parent.gameObject.activeSelf)
                {
                    player.RecalculatePositions();

                    if (!(parent.gameObject.active = data.enabled))
                        return;
                }

                if (data.text != null) text.text = data.text;
                if (data.textColor != null) text.color = data.textColor;
                if (data.background != null) image.color = data.background;
            }
        }
    }
}
