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
    public float missileDamage;
    public float explosionRadius;
    public float missileCooldown;
    private bool canShootMissile = true;
    [SerializeField]private int currentMissiles;

    public bool isAttacking = false;

    //private Animator anim;
    private Camera mainCamera;
    private PlayerMovement pM;
    private PlayerDash pDash;

    private Ray ray;
    private Vector3 direccionRayo;
    private Vector3 puntoObjetivo;


    private void Start()
    {
        //animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        currentMissiles = maxMissiles;
        pM = GetComponent<PlayerMovement>();
        pDash = GetComponent<PlayerDash>();

        StartCoroutine(ChargeLaser());
        
    }

    private void Update()
    {
        AttackType();
        ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
    }

    private void AttackType()
    {
        if (isAttacking) return;

        if (InputController.Instance.MainAttack() && pDash.isDashing == false)
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

        if (InputController.Instance.SecondaryAttack() && canShootMissile && pDash.isDashing == false)
        {
            StartCoroutine(ShootMissiles());
            Debug.Log("Missile");
        }

        if (InputController.Instance.SpecialAttack() && pDash.isDashing == false && canUseLaser)
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
            rb.AddForce(mainCamera.transform.up * throwForce/3, ForceMode.Impulse);
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
        pM.lockMovement = true;
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
        pM.lockMovement = false;
        yield return new WaitForSeconds(missileCooldown);
        canShootMissile = true;
    }

    private IEnumerator UseLaser()
    {
        isAttacking = true;
        canUseLaser = false;
        StartCoroutine(ChargeLaser());
        //animator
        pM.lockMovement = true;
        LineRenderer laser = Instantiate(laserPrefab, laserOrigin.position, laserOrigin.rotation);
        laser.transform.SetParent(laserOrigin);

        float timer = 0f;
        while (timer < laserDuration)
        {
            ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            puntoObjetivo =  ray.origin + ray.direction * laserRange;


            laserOrigin.LookAt(puntoObjetivo);

            laser.SetPosition(0, Vector3.zero);
            laser.SetPosition(1, new Vector3(0,0,50f));


            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(laser.gameObject);
        isAttacking = false;
        pM.lockMovement = false;

        
        //yield return new WaitForSeconds(laserCooldown);
        //canUseLaser = true;


    }

    public IEnumerator ChargeLaser()
    {
        yield return new WaitForSeconds(laserCooldown);
        canUseLaser = true;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(laserOrigin.position, direccionRayo * laserRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(ray.origin, ray.direction * laserRange);
    }

}
