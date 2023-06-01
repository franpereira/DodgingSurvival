using System.Collections;
using UnityEngine;

namespace Enemies
{
    public class GroundSpikeSpawner : MonoBehaviour
    {
        [SerializeField] GroundSpike spikePrefab;
        [SerializeField] float spawnY;
        [SerializeField] float rangeX;
        [SerializeField] float spawnInterval = 1f;
    
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
            
                // One more every 500 score points with a max of 10.
                int maxPerSpawn = Mathf.Min(10, (int) (Score.Value / 500 + 1));
                int spikesToSpawn = Random.Range(1, maxPerSpawn);
            
                // Should the spikes be spawned at the same time or one by one?
                bool spawnAllAtOnce = Random.Range(0, 2) > 0;

                if (spawnAllAtOnce)
                {
                    for (int i = 0; i < spikesToSpawn; i++)
                        SpawnSpike();
                }
                else
                {
                    for (int i = 0; i < spikesToSpawn; i++)
                    {
                        SpawnSpike();
                        float wait = Random.Range(0.2f, 1f);
                        yield return new WaitForSeconds(wait);
                    }
                }
            }
        }

        void SpawnSpike()
        {
            float randomX = Random.Range(-rangeX, rangeX);
            Vector2 position = new (randomX, spawnY);
            GroundSpike spike = Instantiate(spikePrefab, position, Quaternion.identity);
        }
    
    }
}