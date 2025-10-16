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
    public Slider playerHealth;
    public TextMeshProUGUI playerHealthCount;
    [Header("Enemy")]
    public Slider enemyHealth;
    public Slider enemyDistance;

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
        if (playerHealth == null)
        {
            return;
        }

        playerHealth.maxValue = pHealth.maxHealth;
        playerHealth.value = pHealth.actualHealth;

        playerHealthCount.text = $"{(int)pHealth.actualHealth}/{pHealth.maxHealth}";
    }

    private void UpdateEnemyHealth()
    {
        if (enemyHealth == null)
        {
            return;
        }

        enemyHealth.maxValue = eHealth.maxHealth;
        enemyHealth.value = eHealth.actualHealth;
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
        enemyDistance.value = eMove.initialDistance - eMove.CalculateDistanceToEnd();
    }
}
