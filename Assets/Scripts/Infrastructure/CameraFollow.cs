using UnityEngine;

namespace Infrastructure
{
    internal class CameraFollow : MonoBehaviour
    {
        [SerializeField] private float _yShift;
        [SerializeField] private float _zShift;
        [SerializeField] private float _rotationAngle;

        private Transform _target;
        private bool _canFollow;
        private Vector3 _followPos = Vector3.zero;
        private Vector3 _followShift;

        private void Start()
        {
            transform.rotation = Quaternion.Euler(_rotationAngle, 0, 0);
            _followShift = new Vector3(0, _yShift, _zShift);
        }
        public void SetTarget(Transform target)
        {
            _target = target;;
            _canFollow = true;
        }

        private void LateUpdate()
        {
            if (!_canFollow)
                return;

            _followPos = _target.transform.position + _followShift;
            transform.position = _followPos;
        }
    }
}