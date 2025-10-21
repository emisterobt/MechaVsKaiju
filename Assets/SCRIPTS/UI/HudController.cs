using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    private AttackHandler attackHandler;
    private PlayerHealthHandler pHealth;
    private EnemyHealthHandler eHealth;
    private EnemyMovement eMove;

    [Header("Player")]
    //public Slider playerHealth;
    public TextMeshProUGUI playerHealthCount;

    [SerializeField] private RectTransform _playerHealthBarRect;
    [SerializeField] private RectMask2D _playerHealthBarMask;
    [SerializeField] private float _playerHealthMaxRightMask;
    [SerializeField] private float _playerHealthInitialRightMask;


    [Header("Enemy")]
    //public Slider enemyHealth;
    public Slider enemyDistance;
    [SerializeField] private RectTransform _enemyHealthBarRect;
    [SerializeField] private RectMask2D _enemyHealthBarMask;
    [SerializeField] private float _enemyHealthMaxRightMask;
    [SerializeField] private float _enemyHealthInitialRightMask;

    [Header("Attacks")]
    public Slider laserCharge;
    public TextMeshProUGUI laserChargeCount;
    public TextMeshProUGUI missileCount;

    private void Start()
    {
        attackHandler = FindAnyObjectByType<AttackHandler>();
        pHealth = FindAnyObjectByType<PlayerHealthHandler>();
        eHealth = FindAnyObjectByType<EnemyHealthHandler>();
        eMove = FindAnyObjectByType<EnemyMovement>();


        _playerHealthMaxRightMask = _playerHealthBarRect.rect.width - _playerHealthBarMask.padding.x - _playerHealthBarMask.padding.z;
        _playerHealthInitialRightMask = _playerHealthBarMask.padding.z;

        _enemyHealthMaxRightMask = _enemyHealthBarRect.rect.width - _enemyHealthBarMask.padding.x - _enemyHealthBarMask.padding.z;
        _enemyHealthInitialRightMask = _enemyHealthBarMask.padding.z;
    }
    private void Update()
    {
        UpdatePlayerHealth();
        UpdateEnemyHealth();
        UpdateLaser();
        UpdateMissileCount();
        UpdateEnemyDistance();
    }

    private void UpdatePlayerHealth()
    {
        //if (playerHealth == null)
        //{
        //    return;
        //}

        //playerHealth.maxValue = pHealth.maxHealth;
        //playerHealth.value = pHealth.actualHealth;

        var targetWidth = pHealth.actualHealth * _playerHealthMaxRightMask/ pHealth.maxHealth;
        var newRightMask = _playerHealthMaxRightMask + _playerHealthInitialRightMask - targetWidth;
        var padding = _playerHealthBarMask.padding;
        padding.z = newRightMask;
        _playerHealthBarMask.padding = padding;


        playerHealthCount.text = $"{(int)pHealth.actualHealth}/{pHealth.maxHealth}";
    }

    private void UpdateEnemyHealth()
    {
        //if (enemyHealth == null)
        //{
        //    return;
        //}

        //enemyHealth.maxValue = eHealth.maxHealth;
        //enemyHealth.value = eHealth.actualHealth;

        var eTargetWidth = eHealth.actualHealth * _enemyHealthMaxRightMask / eHealth.maxHealth;
        var eNewRightMask = _enemyHealthMaxRightMask + _enemyHealthInitialRightMask - eTargetWidth;
        var ePadding = _enemyHealthBarMask.padding;
        ePadding.z = eNewRightMask;
        _enemyHealthBarMask.padding = ePadding;

    }

    private void UpdateLaser()
    {
        if (attackHandler == null)
        {
            return;
        }

        laserCharge.maxValue = attackHandler.laserCooldown;
        laserCharge.value = attackHandler.timer;

        

        int percentageCharge = (int)((attackHandler.timer * 100)/attackHandler.laserCooldown);

        laserChargeCount.text = $"{percentageCharge}%";
    }

    private void UpdateMissileCount()
    {
        if (attackHandler == null)
        {
            return;
        }

        missileCount.text = $"{attackHandler.currentMissiles}/{attackHandler.maxMissiles}";
    }

    private void UpdateEnemyDistance()
    {
        if (eMove == null)
        {
            return;
        }

        if (eMove.CalculateDistanceToEnd() == eMove.initialDistance)
        {
            enemyDistance.value = 0;
        }

        enemyDistance.maxValue = eMove.initialDistance;
        enemyDistance.value = eMove.initialDistance - eMove.CalculateDistanceToEnd() + eMove.agent.stoppingDistance;
    }
}
