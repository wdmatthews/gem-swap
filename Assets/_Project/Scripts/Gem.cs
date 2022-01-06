using UnityEngine;

namespace GemSwap
{
    [AddComponentMenu("Gem Swap/Gem")]
    public class Gem : MonoBehaviour
    {
        [SerializeField] private float _minGravityScale;
        [SerializeField] private float _maxGravityScale;
        [SerializeField] private float _swapSpeed;
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshRenderer _renderer;
        [SerializeField] private Rigidbody2D _rigidbody;

        private GemSO _data;
        private Vector2Int _position;
        private Vector2Int _oldPosition;
        private Vector3 _targetPosition;
        private bool _isFalling;
        private bool _isSwapping;
        private System.Action _onStoppedFalling;

        public GemSO Data => _data;
        public Vector2Int Position => _position;
        public bool IsMoving => _isFalling || _isSwapping;

        private void FixedUpdate()
        {
            if (_isFalling && transform.position.y <= _targetPosition.y)
            {
                StopFalling();
            }
            else if (_isSwapping
                && (_position.x < _oldPosition.x && transform.position.x <= _targetPosition.x
                    || _position.x > _oldPosition.x && transform.position.x >= _targetPosition.x
                    || _position.y < _oldPosition.y && transform.position.y <= _targetPosition.y
                    || _position.y > _oldPosition.y && transform.position.y >= _targetPosition.y))
            {
                StopSwapping();
            }
        }

        public void Place(GemSO data, Vector2Int gridPosition, Vector3 worldPosition)
        {
            _data = data;
            _meshFilter.mesh = _data.Mesh;
            _renderer.material = _data.Material;
            _position = gridPosition;
            _oldPosition = _position;
            transform.position = worldPosition;
            gameObject.SetActive(true);
        }

        public void FallTo(Vector2Int gridPosition, Vector3 worldPosition, System.Action onStoppedFalling = null)
        {
            _isFalling = true;
            _oldPosition = _position;
            _position = gridPosition;
            _targetPosition = worldPosition;
            _rigidbody.WakeUp();
            _rigidbody.gravityScale = Random.Range(_minGravityScale, _maxGravityScale);
            _onStoppedFalling = onStoppedFalling;
        }

        public void SwapTo(Vector2Int gridPosition, Vector3 worldPosition)
        {
            Vector2Int swapDirection = gridPosition - _position;
            _isSwapping = true;
            _oldPosition = _position;
            _position = gridPosition;
            _targetPosition = worldPosition;
            _rigidbody.WakeUp();
            _rigidbody.velocity = new Vector2(_swapSpeed * swapDirection.x, _swapSpeed * swapDirection.y);
        }

        public void Remove()
        {
            gameObject.SetActive(false);
        }

        private void StopMoving()
        {
            transform.position = _targetPosition;
            _rigidbody.velocity = new Vector2();
            _rigidbody.Sleep();
        }

        private void StopFalling()
        {
            _isFalling = false;
            _rigidbody.gravityScale = 0;
            StopMoving();
            _onStoppedFalling?.Invoke();
        }

        private void StopSwapping()
        {
            _isSwapping = false;
            StopMoving();
        }
    }
}
