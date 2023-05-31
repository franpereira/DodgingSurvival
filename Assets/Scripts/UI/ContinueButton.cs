using UnityEngine;

namespace UI
{
    public class ContinueButton : MonoBehaviour
    {
        public void OnClick() => Core.Events.InvokeRestart();
    }
}
