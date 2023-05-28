using Collectables;
using UnityEngine;


namespace PlayerSystem
{
    internal class PlayerTriggerModule : MonoBehaviour
    {
        private BoxCollider _collider;
        private TriggerHandler _triggerHandler;

        public void Init(BoxCollider collider, TriggerHandler triggerHandler)
        {
            _collider = collider;
            _triggerHandler = triggerHandler;
            _collider.enabled = true;
            _collider.isTrigger = true;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out CollectableObject collectable))
            {
                _triggerHandler.SortOutCollectable(collectable);
                collectable.ExecuteAction();
            }
            else
            {
                Debug.Log("hit " + other.name);
                _triggerHandler.RegisterObstacleHit();
            }
        }
    }
}