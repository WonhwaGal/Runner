using Collectables;
using Factories;
using PlayerSystem;
using UnityEngine;

public class PlayerTriggerModule : MonoBehaviour
{
    [SerializeField] private CapsuleCollider _collider;
    [SerializeField] private PlayerJumper _playerJumper;

    private bool _isJumping;
    private void Awake()
    {
        _collider.enabled = true;
        _collider.isTrigger = true;
        if (_playerJumper != null)
            _isJumping = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ICollectable collectable))
        {
            collectable.ExecuteAction();
        }
        else if (_isJumping && other.GetComponent<RoadSpan>())
        {
            _playerJumper.FinishJump();
        }
    }
}
