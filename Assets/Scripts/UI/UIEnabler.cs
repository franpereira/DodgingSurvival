using UnityEngine;

namespace UI
{
    public class UIEnabler : MonoBehaviour
    {
        [SerializeField] GameObject continueButton;
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
    
        void OnBegin() => continueButton.SetActive(false);
        void OnEnd() => continueButton.SetActive(true);
    }
}
