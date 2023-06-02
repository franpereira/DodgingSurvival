using System.Collections;
using Characters;
using UnityEngine;

namespace Enemies
{
    public class ArrowSpawner : MonoBehaviour
    {
        [SerializeField] Arrow arrowPrefab;
        [SerializeField] float spawnX = 10f;
        [SerializeField] float spawnInterval = 1f;
        [SerializeField] float targetRadius = 10f;
        [SerializeField] PlayerCharacter player;

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

        void OnBegin() => StartCoroutine(SpawnArrowsRoutine());
        void OnEnd() => StopAllCoroutines();
        
        IEnumerator SpawnArrowsRoutine()
        {
            while (true)
            {
                float seconds = spawnInterval;
                yield return new WaitForSeconds(seconds);
                
                SpawnArrow();
            }
        }

        void SpawnArrow()
        {
            Vector2 spawnPosition = new(spawnX, 0);
            Arrow arrow = Instantiate(arrowPrefab, spawnPosition, Quaternion.identity);
            arrow.LaunchTowardsInParabola(player.Center.position);
        }
    }
}