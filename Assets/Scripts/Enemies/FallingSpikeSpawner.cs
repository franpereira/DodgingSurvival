using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class FallingSpikeSpawner : MonoBehaviour
    {
        [SerializeField] FallingSpike fallingSpikePrefab;
        [SerializeField] float spawnY = 16f;
        [SerializeField] float rangeX = 20f;
        [SerializeField] float spawnInterval = 1f;
        [SerializeField] float minScale = 0.25f;
        [SerializeField] float maxScale = 1f;

        void OnEnable()
        {
            Core.Events.Begin += OnBegin;
            Core.Events.Restart += OnBegin;
            Core.Events.End += OnEnd;
        }
        void OnDisable()
        {
            Core.Events.Begin -= OnBegin;
            Core.Events.Restart -= OnBegin;
            Core.Events.End -= OnEnd;
        }

        void OnBegin() => StartCoroutine(SpawnSpikesRoutine());
        void OnEnd() => StopAllCoroutines();

        IEnumerator SpawnSpikesRoutine()
        {
            while (true)
            {
                float seconds = spawnInterval;
                yield return new WaitForSeconds(seconds);
            
                // One more every 100 score points with a max of 7.
                int maxPerSpawn = Mathf.Min(7, (int) (Score.Value / 100 + 1));
                int spikesToSpawn = Random.Range(1, maxPerSpawn);
            
                for (int i = 0; i < spikesToSpawn; i++)
                    SpawnSpike();
            }
        }

        void SpawnSpike()
        {
            float randomX = Random.Range(-rangeX, rangeX);

            Vector2 position = new (randomX, spawnY);
            FallingSpike spike = Instantiate(fallingSpikePrefab, position, Quaternion.Euler(0, 0, 180));
            float randomScale = Random.Range(minScale, maxScale);
            spike.transform.localScale = new Vector3(randomScale, randomScale, 1);
        }
    }
}