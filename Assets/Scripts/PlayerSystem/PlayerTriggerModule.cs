using Collectables;
using Factories;
using PlayerSystem;
using UnityEngine;

public class PlayerTriggerModule : MonoBehaviour
{
    [SerializeField] private CapsuleCollider _collider;
    [SerializeField] private PlayerJumper _playerJumper;

    private bool _jumpingPlayer;
    private void Awake()
    {
        _collider.enabled = true;
        _collider.isTrigger = true;
        if (_playerJumper != null)
            _jumpingPlayer = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ICollectable collectable))
        {
            collectable.ExecuteAction();
        }
        else if (_jumpingPlayer && other.GetComponent<RoadSpan>())
        {
            _playerJumper.IsJumping = false;
        }
    }
}
