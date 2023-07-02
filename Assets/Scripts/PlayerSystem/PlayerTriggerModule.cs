using Collectables;
using Factories;
using System;
using UnityEngine;


namespace PlayerSystem
{
    internal class PlayerTriggerModule : MonoBehaviour
    {
        public Action<int> ChangeLaneOnTurning;
        public Action<RoadSpan> OnTriggeredByRoadSpan;
        //public Action<bool> OnFinishTurning;

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
            if (other.GetComponent<PlayerMagnetController>())   // make another layer
                return;

            if (other.TryGetComponent(out CollectableObject collectable))
            {
                _triggerHandler.SortOutCollectable(collectable);
                collectable.ExecuteAction();
            }
            else if (other.TryGetComponent(out RoadSpan road))
            {
                OnTriggeredByRoadSpan?.Invoke(road);
                //road.CheckForTurn();
            }
            else
            {
                Debug.Log("hit " + other.name);
                _triggerHandler.RegisterObstacleHit();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out RoadSpan road))
            {
                if (road.RoadType != RoadSpanType.Straight)
                {
                    ChangeLaneOnTurning?.Invoke(0);
                    road.OnTurning?.Invoke(false);
                }
            }
        }
    }
}