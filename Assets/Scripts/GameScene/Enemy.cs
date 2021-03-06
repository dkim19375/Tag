using UnityEngine;

namespace GameScene {
    public class Enemy : MonoBehaviour {
        private readonly GameManager _gameManager = GameManager.GetInstance();
        private Player _player;
        private float Speed => _player.speed / 65;
        // ReSharper disable once InconsistentNaming
        private new RectTransform transform => (RectTransform) base.transform;
        private Vector2 TransformPos2D => new Vector2(transform.position.x, transform.position.y);
        private Rigidbody2D _rigidbody;

        private void Awake() {
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _gameManager.OnEnd.Add(this, Utilities.GetMethodInfo(OnEnd));
        }

        private void OnEnd() {
            
        }

        private void FixedUpdate() {
            Move();
        }

        private void Move() {
            _rigidbody.velocity = Vector2.zero;
            if (GameManager.GetInstance().Frozen || !GameManager.GetInstance().Active) {
                return;
            }
            var angle = Angles.GetAngle(TransformPos2D , _player.TransformPos2D);
            var newPos = Angles.GetDirectionPoint(Vector2.zero, Speed * Time.fixedDeltaTime, angle);
            _rigidbody.velocity = newPos.normalized * (_player.speed * 0.8f * Time.fixedDeltaTime);
        }
    }
}