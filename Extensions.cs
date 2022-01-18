using UnityEngine;
using static Astrum.AstralTags;

namespace Astrum
{
    public static class Extensions
    {
        internal static void Create(this PlayerData data, Tag tag)
        {
            Transform rank = Object.Instantiate(data.stats, data.stats.parent, false);
            rank.name = $"AstralTag";
            rank.gameObject.active = true;
            Transform transform = null;

            for (int i = rank.childCount; i > 0; i--)
            {
                Transform child = rank.GetChild(i - 1);

                if (child.name == "Trust Text")
                {
                    transform = child;
                    continue;
                }

                Object.Destroy(child.gameObject);
            }

            TagInstance inst = new(data, tag, transform);

            data.tags.Add(inst);
            tag.instances.Add(inst);

            inst.Calculate();
            data.RecalculatePositions();
        }

        public static void RecalculatePositions(this PlayerData data)
        {
            int i = 1;
            foreach (TagInstance tag in data.tags)
                if (tag.parent.gameObject.activeSelf)
                    tag.parent.localPosition = new Vector3(0, 30 * ++i, 0);
        }
    }
}
