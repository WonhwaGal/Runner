using Collectables;
using UnityEngine;
using System;

namespace PlayerSystem
{
    internal class PlayerTriggerModule : MonoBehaviour
    {
        private CapsuleCollider _collider;
        private PlayerRBJumper _playerJumper;
        private TriggerHandler _triggerHandler;
        public void Init(CapsuleCollider collider, TriggerHandler triggerHandler)
        {
            _collider = collider;
            _triggerHandler = triggerHandler;
            _collider.enabled = true;
            _collider.isTrigger = true;
        }

        public void Init(CapsuleCollider collider, PlayerRBJumper playerJumper)
        {
            _collider = collider;
            _collider.enabled = true;
            _collider.isTrigger = true;
            _playerJumper = playerJumper;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ICollectable collectable))
            {
                _triggerHandler.SortOutCollectable(collectable);
                collectable.ExecuteAction();
            }
        }
    }

    internal class TriggerHandler
    {
        public Action<int> OnTriggeredByCoin;
        public void SortOutCollectable(ICollectable collectable)
        {
            if (collectable.Type == CollectableType.Coin)
                OnTriggeredByCoin?.Invoke(collectable.Value);
        }
    }
}