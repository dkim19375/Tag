using GameScene;
using TMPro;
using UnityEngine;

namespace GameOverScene {
    public class ScoreGameOver : MonoBehaviour {
        private TMP_Text _text;
        private GameManager GameManager => GameManager.GetInstance();

        private void Awake() {
            _text = GetComponent<TMP_Text>();
        }

        private void Update() => _text.text = $"Score: {GameManager.Score}";
    }
}