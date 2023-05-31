using System.Collections;
using UnityEngine;

namespace Enemies
{
    public class Warning : MonoBehaviour
    {
        void OnEnable() => StartCoroutine(Routine());

        IEnumerator Routine()
        {
            // Fade in/out many times quickly to create a warning effect.
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            Color color = spriteRenderer.color;
            float startTime = Time.time;
            float endTime = startTime + 0.25f;
            while (Time.time < endTime)
            {
                float t = (Time.time - startTime) / (endTime - startTime);
                color.a = Mathf.Lerp(0, 1, t);
                spriteRenderer.color = color;
                yield return null;
            }
            startTime = Time.time;
            endTime = startTime + 0.25f;
            while (Time.time < endTime)
            {
                float t = (Time.time - startTime) / (endTime - startTime);
                color.a = Mathf.Lerp(1, 0, t);
                spriteRenderer.color = color;
                yield return null;
            }
            Destroy(gameObject);
        }
    }
}
