using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameScene {
    public class Player : MonoBehaviour {
        public float speed = 180f;

        private Rigidbody2D _rigidbody;
        private RawImage _image;
        private readonly GameManager _gameManager = GameManager.GetInstance();
        public Vector2 TransformPos2D => new Vector2(transform.position.x, transform.position.y);

        private void Awake() {
            _rigidbody = GetComponent<Rigidbody2D>();
            _image = GetComponent<RawImage>();
            Debug.Log($"Null????? {_image == null}");
            _gameManager.DataFileManager.CurrentData.CheckData();
            _gameManager.Reset();
            _gameManager.OnStartMethods.Add(this, Utilities.GetMethodInfo(OnStart));
            _gameManager.OnEnd.Add(this, Utilities.GetMethodInfo(OnEnd));
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void Start() {
            Debug.Log("Start");
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            if (!scene.name.Equals("GameScene")) {
                return;
            }
            _gameManager.Reset();
            _gameManager.OnStart();
        }

        private void OnStart() {
            var skin = _gameManager.DataFileManager.CurrentData.GetCurrentSkin();
            Debug.Log($"Null: {_image == null}");
            skin.Image(_image);
        }

        private void OnEnd() { }

        private void FixedUpdate() {
            GameManager.GetInstance().DataFileManager.CurrentData.CheckData();
            _rigidbody.velocity = Vector2.zero;
            if (_gameManager.Active) {
                CheckKey();
            }
        }

        private void OnCollisionStay2D(Collision2D other) {
            if (_gameManager.Frozen) {
                return;
            }
            if (other.gameObject.GetComponent<Enemy>() != null) {
                _gameManager.OnHit();
            }
        }

        private void CheckKey() {
            var isUp = false;
            var isLeft = false;
            var isRight = false;
            var isDown = false;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
                isUp = true;
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                isLeft = true;
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                isRight = true;
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
                isDown = true;
            }

            if (isUp && isDown) {
                isUp = false;
                isDown = false;
            }
            if (isLeft && isRight) {
                isLeft = false;
                isRight = false;
            }

            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (!isUp && !isLeft && !isRight && !isDown) {
                // _rigidbody.velocity = Vector2.zero;
                return;
            }
            if (isUp) {
                if (isLeft) {
                    Move(Angles.Types.UpLeft);
                    return;
                }
                if (isRight) {
                    Move(Angles.Types.UpRight);
                    return;
                }
                Move(Angles.Types.Up);
                return;
            }
            if (isDown) {
                if (isLeft) {
                    Move(Angles.Types.DownLeft);
                    return;
                }
                if (isRight) {
                    Move(Angles.Types.DownRight);
                    return;
                }
                Move(Angles.Types.Down);
                return;
            }
            if (isLeft) {
                Move(Angles.Types.Left);
                return;
            }
            Move(Angles.Types.Right);
        }

        private void Move(Angles.Types type) {
            _rigidbody.velocity = Angles.GetVector(type) * (speed * Time.fixedDeltaTime);
        }
    }
}