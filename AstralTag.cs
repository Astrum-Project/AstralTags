using Astrum.AstralCore.Types;
using System;
using System.Linq;
using UnityEngine;

namespace Astrum
{
    partial class AstralTags
    {
        public class AstralTag
        {
            public readonly int priority;

            internal readonly Func<Player, AstralTagData> evaluator;

            public AstralTag(Func<Player, AstralTagData> evaluator, int priority = 0)
            {
                this.evaluator = evaluator;
                this.priority = priority;

                tags.Add(this);
                tags = tags.OrderByDescending(tag => tag.priority).ToList();

                CalculateAll();
            }

            public void Destroy()
            {
                tags.Remove(this);
                CalculateAll();
            }
        }

        public struct AstralTagData
        {
            public bool enabled;
            public string text;
            public Color textColor;
            public Color backgroundColor;
        }
    }
}
