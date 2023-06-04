using Collectables;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSystem
{
    internal class PlayerMagnetController : MonoBehaviour
    {
        private CapsuleCollider _capsuleCollider;
        private List<CoinView> _inCircleTransforms;
        private Vector3 _shift = new Vector3(0, 3, 1);

        public List<CoinView> InCircleTransforms { get => _inCircleTransforms; set => _inCircleTransforms = value; }


        private void Start() => _inCircleTransforms = new List<CoinView>();


        private void Update()
        {
            var targetVector = transform.position + _shift;
            MagnetCoins(targetVector);
        }


        public void Init(CapsuleCollider capsuleCollider)
        {
            _capsuleCollider = capsuleCollider;
            _capsuleCollider.isTrigger = true;
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out CoinView coin))
                InCircleTransforms.Add(coin);
        }

        private void MagnetCoins(Vector3 targetVector)
        {
            foreach (var coin in SortOutTheList())
            {
                if (coin.gameObject.activeInHierarchy)
                    coin.MoveToTarget(targetVector);
            }
        }
        private List<CoinView> SortOutTheList()
        {
            for (int i = 0; i < InCircleTransforms.Count; i++)
            {
                if (!InCircleTransforms[i].gameObject.activeInHierarchy)
                    InCircleTransforms.Remove(InCircleTransforms[i]);
            }
            return InCircleTransforms;
        }

        private void OnDisable()
        {
            if (InCircleTransforms != null) 
                InCircleTransforms.Clear();
        }
    }
}