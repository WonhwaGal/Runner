using Collectables;
using UnityEngine;

public class PlayerTriggerModule : MonoBehaviour
{
    [SerializeField] private CapsuleCollider _collider;

    private void Awake()
    {
        _collider.enabled = true;
        _collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ICollectable collectable))
        {
            collectable.ExecuteAction();
        }
    }
}
