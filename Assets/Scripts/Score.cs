using System.Collections;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static long Value { get; private set; }
    void Awake() => Value = 0;
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

    void OnBegin()
    {
        Value = 0;
        StartCoroutine(IncreaseRoutine());
    }
    
    void OnEnd() => StopAllCoroutines();
    
    
    readonly WaitForSeconds _wait = new(0.1f);
    IEnumerator IncreaseRoutine()
    {
        while (true)
        {
            yield return _wait;
            // Increase more depending on the time elapsed since the start of the game.
            // When the player resurrects, the score will be back to 0 but it will be increased faster
            // and easier to get back to the previous score.
            // This is intentional but could make the game difficult to players still learning.
            Value += Mathf.CeilToInt(Time.timeSinceLevelLoad / 10);
        }
    }
}