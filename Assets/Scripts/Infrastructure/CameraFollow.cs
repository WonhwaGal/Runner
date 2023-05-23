using UnityEngine;
using DG.Tweening;

namespace Infrastructure
{
    internal class CameraFollow : MonoBehaviour
    {
        [SerializeField] private float _yShift;
        [SerializeField] private float _zShift;
        [SerializeField] private float _rotationAngle;

        private Transform _target;
        private bool _canFollow;
        private Vector3 _followPos;
        private Vector3 _followShift;
        private Camera _camera;

        private void Start()
        {
            transform.rotation = Quaternion.Euler(_rotationAngle, 0, 0);
            _followShift = new Vector3(0, _yShift, _zShift);
            _followPos = Vector3.zero;
            _camera = GetComponent<Camera>();
        }
        public void SetTarget(Transform target)
        {
            _target = target;
            _canFollow = true;
        }
        public void ShakeCamera()
        {
            _canFollow = false;
            _camera.DOShakePosition(1.0f, 1, 5, 60, true);
        }

        private void LateUpdate()
        {
            if (!_canFollow)
                return;

            _followPos = new Vector3(_target.transform.position.x, 0, _target.transform.position.z) + _followShift;
            transform.position = _followPos;
        }
    }
}