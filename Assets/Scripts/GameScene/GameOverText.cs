using System;
using TMPro;
using UnityEngine;

namespace GameScene {
    public class GameOverText : MonoBehaviour {
        private GameManager GameManager => GameManager.GetInstance();
        private TMP_Text _text;

        private void Awake() {
            _text = GetComponent<TMP_Text>();
        }

        private void Start() {
            _text.enabled = false;
        }

        private void Update() {
            _text.enabled = GameManager.Lives <= 0;
        }
    }
}