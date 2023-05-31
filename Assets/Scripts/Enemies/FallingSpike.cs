using System.Collections;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class FallingSpike : MonoBehaviour
    {
        Rigidbody2D _rb;
        [SerializeField] float peekTime = 2f;
        [SerializeField] float peekDistance = 2f;
        [SerializeField] int fallAfterSeconds = 1;

        void Awake() => _rb = GetComponent<Rigidbody2D>();

        void OnEnable()
        {
            _rb.isKinematic = true;
            StartCoroutine(Peek());
        }
    
        // The spike peeks out of the ceiling for a few seconds before falling.
        IEnumerator Peek()
        {
            Vector2 startPosition = _rb.position;
            Vector2 endPosition = startPosition + Vector2.down * peekDistance;
            float startTime = Time.time;
            float endTime = startTime + peekTime;

            while (Time.time < endTime)
            {
                float t = (Time.time - startTime) / peekTime;
                _rb.MovePosition(Vector2.Lerp(startPosition, endPosition, t));
                yield return null;
            }

            // Ensure the position at the end of the peek is exact
            // (floating point errors, not that it matters here).
            _rb.MovePosition(endPosition); 
        
            _rb.isKinematic = false; // Free fall.
            StartCoroutine(FallingRoutine());
        }

        IEnumerator FallingRoutine()
        {
            yield return new WaitForSeconds(fallAfterSeconds);

            // Set the collider to trigger so it clips through things.
            GetComponent<Collider2D>().isTrigger = true;
        
            // Fade out some time while falling.
            var spriteRenderer = GetComponent<SpriteRenderer>();
            var color = spriteRenderer.color;
            float startTime = Time.time;
            float endTime = startTime + 1f;
            while (Time.time < endTime)
            { 
                float t = (Time.time - startTime) / 1;
                // Lerp between the current color and fully transparent.
                spriteRenderer.color = Color.Lerp(color, Color.clear, t);
                yield return null;
            }
        
            // After the fade out, destroy the object.
            // TODO: The spikes should be pooled instead of destroyed.
            Destroy(gameObject);
        }
    }
}
