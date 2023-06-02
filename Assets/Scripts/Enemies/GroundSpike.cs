using System.Collections;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class GroundSpike : MonoBehaviour
    {
        Rigidbody2D _rb;
        [SerializeField] float peekTime = 2f;
        [SerializeField] float peekDistance = 2f;
        [SerializeField] float hideAfterSeconds = 1.5f;
        [SerializeField] Warning warningPrefab;
        [SerializeField] float warningY = 1f;
        [SerializeField] AudioSource peekSound;

        void Awake() => _rb = GetComponent<Rigidbody2D>();
        void OnEnable() => StartCoroutine(Peek());

        // The spike peeks out of the ground for a few seconds before hiding.
        IEnumerator Peek()
        {
            Vector2 warningPos = new Vector2(transform.position.x, warningY);
            Warning warning = Instantiate(warningPrefab, warningPos, Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
            
        
            Vector2 startPosition = _rb.position;
            Vector2 endPosition = startPosition + Vector2.up * peekDistance;
            float startTime = Time.time;
            float endTime = startTime + peekTime;
        
            while (Time.time < endTime)
            {
                float t = (Time.time - startTime) / peekTime;
                _rb.MovePosition(Vector2.Lerp(startPosition, endPosition, t));

                if (!peekSound.isPlaying && t is > 0.25f and < 0.5f)
                {
                    peekSound.pitch = Random.Range(0.8f, 2f);
                    peekSound.Play();
                    Destroy(warning.gameObject);
                }

                yield return null;
            }
            _rb.MovePosition(endPosition);
            StartCoroutine(Hide());
        }
    
        IEnumerator Hide()
        {
            yield return new WaitForSeconds(hideAfterSeconds);
        
            // Move back to the starting position.
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
            _rb.MovePosition(endPosition);
        
            // TODO: Pool
            Destroy(gameObject);
        }
    }
}
