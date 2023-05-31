using TMPro;
using UnityEngine;

namespace UI
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI scoreText;
        void Update() => scoreText.text = $"{Score.Value.ToString()}";
    }
}
