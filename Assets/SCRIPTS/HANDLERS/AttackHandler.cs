using System.Collections;
using System.ComponentModel;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    public Transform mechaHand;
    public Transform missilePointL;
    public Transform missilePointR;
    public Transform laserOrigin;
    public Transform attackCollider;

    public GameObject missilePrefab;

    [Header("Melee Attack")]
    public float meleeDamage;

    [Header("Throw Attack")]
    public float throwForce;
    public GameObject objectInHand;

    [Header("Laser Attack")]
    public float laserRange;
    public float laserDamage;
    public float laserDuration;
    public float laserCooldown;
    public bool canUseLaser = false;
    public LineRenderer laserPrefab;

    [Header("Missiles")]
    public int maxMissiles;
    public float missileForce;
    public float missileDuration;
    public float missileCooldown;
    private bool canShootMissile = true;
    [SerializeField]private int currentMissiles;

    private bool isAttacking = false;

    //private Animator anim;
    private Camera mainCamera;

    private void Start()
    {
        //animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        currentMissiles = maxMissiles;
    }

    private void Update()
    {
        AttackType();
    }

    private void AttackType()
    {
        if (isAttacking) return;

        if (InputController.Instance.MainAttack())
        {
            if (objectInHand != null)
            {
                StartCoroutine(ThrowObject());
                Debug.Log("Throw");
            }
            else
            {
                StartCoroutine(MeleeAttack());
                Debug.Log("Melee");
            }
        }

        if (InputController.Instance.SecondaryAttack() && canShootMissile)
        {
            StartCoroutine(ShootMissiles());
            Debug.Log("Missile");
        }

        if (InputController.Instance.SpecialAttack())
        {
            StartCoroutine(UseLaser());
            Debug.Log("Laser");
        }
    }

    private IEnumerator MeleeAttack()
    {
        isAttacking = true;
        attackCollider.gameObject.SetActive(true);
        //animator attack

        yield return new WaitForSeconds(0.5f);
        attackCollider.gameObject.SetActive(false);
        isAttacking = false ;
    }

    private IEnumerator ThrowObject()
    {
        isAttacking = true;
        //animator throw

        yield return new WaitForSeconds(0.3f);

        objectInHand.transform.SetParent(null);
        CarObject carObject = objectInHand.GetComponent<CarObject>();
        Rigidbody rb = objectInHand.GetComponent<Rigidbody>();
        Collider collider = objectInHand.GetComponent<Collider>();

        if (rb != null)
        {
            rb.isKinematic = false;
            carObject.isThrown = true;

            rb.AddForce(mainCamera.transform.forward * throwForce, ForceMode.Impulse);
            rb.AddForce(mainCamera.transform.up * throwForce/2, ForceMode.Impulse);
        }

        if(collider != null)
        {
            collider.enabled = true;
        }

        objectInHand = null;
        isAttacking = false;
    }

    private IEnumerator ShootMissiles()
    {
        isAttacking = true;
        canShootMissile = false;
        //animator
        yield return new WaitForSeconds(0.2f);

        if (missilePrefab != null && missilePointL != null && missilePointR != null && currentMissiles > 0)
        {
            GameObject missileL = Instantiate(missilePrefab, missilePointL.position, missilePointL.rotation);
            GameObject missileR = Instantiate(missilePrefab, missilePointR.position, missilePointR.rotation);
            Rigidbody rbL = missileL.GetComponent<Rigidbody>();
            Rigidbody rbR = missileR.GetComponent<Rigidbody>();

            if(rbL != null && rbR != null)
            {
                rbL.AddForce(mainCamera.transform.forward * missileForce, ForceMode.Impulse);
                rbR.AddForce(mainCamera.transform.forward * missileForce, ForceMode.Impulse);
            }

            currentMissiles -= 2;
        }
        isAttacking = false;

        yield return new WaitForSeconds(missileCooldown);
        canShootMissile = true;
    }

    private IEnumerator UseLaser()
    {
        isAttacking = true;
        canUseLaser = false;
        //animator

        LineRenderer laser = Instantiate(laserPrefab, laserOrigin.position, laserOrigin.rotation);
        laser.transform.SetParent(laserOrigin);

        float timer = 0f;
        while (timer < laserDuration)
        {
            laser.SetPosition(0, Vector3.zero);
            laser.SetPosition(1, new Vector3(0,0,50f));

            RaycastHit hit;
            if (Physics.Raycast(laserOrigin.position, mainCamera.transform.forward, out hit, laserRange))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    hit.collider.GetComponent<IDamageable>().TakeDamage(laserDamage * Time.deltaTime);
                    Debug.Log("Recibiendo Daño");
                    
                }
            }

            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(laser.gameObject);
        isAttacking = false;

        yield return new WaitForSeconds(laserCooldown);
        canUseLaser = true;


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(laserOrigin.position, Camera.main.transform.forward * laserRange);
    }

}
