using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameScene {
    public class GameManager {
        private static readonly GameManager Singleton = new GameManager();
        public readonly DataFileManager DataFileManager = new DataFileManager();
        public readonly Dictionary<object, MethodInfo> OnStartMethods = new Dictionary<object, MethodInfo>();
        public readonly Dictionary<object, MethodInfo> OnEnd = new Dictionary<object, MethodInfo>();
        public int Score;
        public int Lives;
        public int CollectedCoins;
        public bool Frozen;
        public bool Active = true;
        private Player _player;
        private Coin _coin;

        private GameManager() { }

        public static GameManager GetInstance() => Singleton;

        public void Reset() {
            Score = 0;
            Lives = 5;
            CollectedCoins = 0;
            Frozen = false;
            Active = true;
        }

        public void OnStart() {
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            _coin = GameObject.FindGameObjectWithTag("Coin").GetComponent<Coin>();
            _player.StartCoroutine(OnStartCoroutine());
            foreach (var pair in OnStartMethods) {
                pair.Value.Invoke(pair.Key, new object[] { });
            }
            OnStartMethods.Clear();
        }

        private IEnumerator OnStartCoroutine() {
            while (Active) {
                yield return new WaitForSeconds(1);
                _player.speed += 10f;
                if (!Frozen) {
                    Score++;
                }
            }
        }

        public void OnHit() {
            Frozen = true;
            Lives--;
            _coin.TeleportToRandomPoint();
            if (Lives > 0) {
                _player.StartCoroutine(FreezeCoroutine());
                return;
            }
            foreach (var pair in OnEnd) {
                pair.Value.Invoke(pair.Key, new object[] { });
            }
            OnEnd.Clear();
            _player.StartCoroutine(EndCoroutine());
        }
        
        private IEnumerator FreezeCoroutine() {
            yield return new WaitForSeconds(3);
            Frozen = false;
        }

        private IEnumerator EndCoroutine() {
            Active = false;
            DataFileManager.CurrentData.SetCoins(CollectedCoins);
            var newHighScore = Score > DataFileManager.CurrentData.GetHighScore();
            if (newHighScore) {
                DataFileManager.CurrentData.SetHighScore(Score);
            }
            Debug.Log($"Coins: {DataFileManager.CurrentData.GetCoins()}");
            DataFileManager.Reload();
            Debug.Log($"Reloaded Coins: {DataFileManager.CurrentData.GetCoins()}");
            Active = false;
            yield return new WaitForSeconds(3);
            SceneManager.LoadScene("GameOverScene");
        }
    }
}