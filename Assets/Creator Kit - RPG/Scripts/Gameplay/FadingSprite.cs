using System.Collections;
using RPGM.Core;
using UnityEngine;

namespace RPGM.Gameplay
{
    /// <summary>
    /// Marks a sprite that should fade away when any object enters its trigger.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
    public class FadingSprite : InstanceTracker<FadingSprite>
    {
        internal SpriteRenderer spriteRenderer;
        internal float alpha = 1, velocity, targetAlpha = 1;

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();

            // Ensure the material supports transparency
            if (spriteRenderer.material == null || spriteRenderer.material.shader.name != "Sprites/Default")
            {
                Debug.LogWarning("SpriteRenderer does not have a valid material or shader. Assigning default shader.");
                spriteRenderer.material = new Material(Shader.Find("Sprites/Default"));
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Object entered trigger, starting fade out.");
            targetAlpha = 0.5f;
            ApplyAlpha();
        }

        void OnTriggerExit2D(Collider2D other)
        {
            Debug.Log("Object exited trigger, starting fade in.");
            targetAlpha = 1f;
            ApplyAlpha();
        }

        void ApplyAlpha()
        {
            alpha = targetAlpha;
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
            //Debug.Log($"Current alpha: {alpha}");
        }
    }
}
