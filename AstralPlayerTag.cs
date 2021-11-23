using System;
using System.Reflection;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace Astrum
{
    partial class AstralTags
    {
        // i hate monos however this is the best implementation possible
        public class AstralPlayerTag : MonoBehaviour
        {
            private static readonly MethodInfo m_GetComponent = null;
            private readonly AstralCore.Types.Player player = null;

            private readonly Transform contents = null;
            private readonly Transform stats = null;

            static AstralPlayerTag()
            {
                ClassInjector.RegisterTypeInIl2Cpp<AstralPlayerTag>();

                m_GetComponent = typeof(MonoBehaviour).GetMethod(nameof(MonoBehaviour.GetComponent), new Type[0] { }).MakeGenericMethod(AstralCore.Types.Player.Type);
            }

            public AstralPlayerTag() : base(ClassInjector.DerivedConstructorPointer<AstralPlayerTag>()) => ClassInjector.DerivedConstructorBody(this);
            public AstralPlayerTag(IntPtr ptr) : base(ptr) 
            {
                player = new AstralCore.Types.Player((MonoBehaviour)m_GetComponent.Invoke(this, new object[0] { }));
                contents = transform.Find("Player Nameplate/Canvas/Nameplate/Contents");
                stats = contents.Find("Quick Stats");

                playerTags.Add(new WeakReference<AstralPlayerTag>(this));

                CalculateTags();
            }

            public void CalculateTags()
            {
                int s = 0;
                for (int i = 0; i < tags.Count; i++)
                {
                    if (contents is null)
                        return;

                    AstralTagData res = tags[i].evaluator(player);

                    Transform tag = contents.Find($"AstralTag{i}/Trust Text");

                    if (tag is null)
                        tag = CreateTag(s);

                    tag.parent.gameObject.active = res.enabled;

                    if (!res.enabled)
                        continue;

                    s++;

                    TMPro.TextMeshProUGUI label = tag.GetComponent<TMPro.TextMeshProUGUI>();

                    if (res.text != null) label.text = res.text;
                    if (res.textColor != null) label.color = res.textColor;
                    if (res.backgroundColor != null) tag.parent.GetComponent<ImageThreeSlice>().color = res.backgroundColor;
                }

                stats.localPosition = new Vector3(0, (s + 1) * 30, 0);
            }

            private Transform CreateTag(int index)
            {
                Transform rank = Instantiate(stats, stats.parent, false);
                rank.name = $"AstralTag{index}";
                rank.localPosition = new Vector3(0, 30 * (index + 1), 0);
                rank.gameObject.active = true;
                Transform textGO = null;

                for (int i = rank.childCount; i > 0; i--)
                {
                    Transform child = rank.GetChild(i - 1);

                    if (child.name == "Trust Text")
                    {
                        textGO = child;
                        continue;
                    }

                    Destroy(child.gameObject);
                }

                return textGO;
            }
        }
    }
}
