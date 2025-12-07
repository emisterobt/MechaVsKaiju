using UnityEngine;
using UnityEngine.UI;

public class CentralNuclearHandler : MonoBehaviour
{
    private EnemyMovement eMove;
    [SerializeField] private RectTransform _nuclearHealthBarRect;
    [SerializeField] private RectMask2D _nuclearHealthBarMask;
    [SerializeField] private float _nuclearHealthMaxRightMask;
    [SerializeField] private float _nuclearHealthInitialRightMask;

    private void Start()
    {
        eMove = FindFirstObjectByType<EnemyMovement>();

        _nuclearHealthMaxRightMask = _nuclearHealthBarRect.rect.width - _nuclearHealthBarMask.padding.x - _nuclearHealthBarMask.padding.z;
        _nuclearHealthInitialRightMask = _nuclearHealthBarMask.padding.z;
    }

    private void Update()
    {
        UpdateNuclearHealth();
    }

    private void UpdateNuclearHealth()
    {
        var targetWidth = eMove.nuclerHealth * _nuclearHealthMaxRightMask / eMove.nuclerMaxHealth;
        var newRightMask = _nuclearHealthMaxRightMask + _nuclearHealthInitialRightMask - targetWidth;
        var padding = _nuclearHealthBarMask.padding;
        padding.z = newRightMask;
        _nuclearHealthBarMask.padding = padding;
    }

}
