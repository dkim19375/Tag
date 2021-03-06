using GameScene;
using TMPro;
using UnityEngine;

namespace GameOverScene {
    public class HighScoreGameOver : MonoBehaviour {
        private TMP_Text _text;
        private GameManager GameManager => GameManager.GetInstance();

        private void Awake() {
            _text = GetComponent<TMP_Text>();
        }

        private void Update() {
            string isNew;
            if (GameManager.Score > GameManager.DataFileManager.CurrentData.GetHighScore()) {
                isNew = "NEW ";
            } else {
                isNew = "";
            }
            _text.text = $"{isNew}HighScore: {GameManager.DataFileManager.CurrentData.GetHighScore()}";
        }
    }
}