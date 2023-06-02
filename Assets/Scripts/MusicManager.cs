using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioSource music;

    void OnEnable()
    {
        Core.Events.Begin += OnBegin;
        Core.Events.End += OnEnd;
        Core.Events.Restart += OnRestart;
    }
    void OnDisable()
    {
        Core.Events.Begin -= OnBegin;
        Core.Events.End -= OnEnd;
        Core.Events.Restart -= OnRestart;
    }


    void OnBegin()
    {
        if (music.isPlaying is false) music.Play();
    }
    void OnEnd() => StartCoroutine(LosePitch());
    void OnRestart() => StartCoroutine(RecoverPitch());

    IEnumerator LosePitch()
    {
        // Abruptly decrease pitch.
        music.pitch = 0.6f;
        yield return null;
    }
    
    IEnumerator RecoverPitch()
    {
        float startTime = Time.time;
        float endTime = startTime + 2f;
        while (Time.time < endTime)
        {
            float t = (Time.time - startTime) / (endTime - startTime);
            music.pitch = Mathf.Lerp(0.6f, 1, t);
            yield return null;
        }
    }
}