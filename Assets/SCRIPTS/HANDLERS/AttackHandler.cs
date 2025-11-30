using System.Collections;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    public Transform mechaHand;
    public Transform missilePointL;
    public Transform missilePointR;
    public Transform laserOrigin;
    public Transform attackCollider;

    public GameObject missileRPrefab;
    public GameObject missileLPrefab;

    [Header("Melee Attack")]
    public float meleeDamage;
    public bool isPunching;
    public int hitType;
    public float meleeCooldown;
    public float meleeTimer;

    [Header("Throw Attack")]
    public float throwForce;
    public GameObject objectInHand;
    public bool isThrowing;

    [Header("Laser Attack")]
    public float laserRange;
    public float laserDamage;
    public float laserDuration;
    public float laserCooldown;
    public bool canUseLaser = false;
    public LineRenderer laserPrefab;
    public bool isUsingLaser = false;
    public float timer;

    [Header("Missiles")]
    public int maxMissiles;
    public float missileForce;
    public float missileDuration;
    public float missileDamage;
    public float explosionRadius;
    public float missileCooldown;
    private bool canShootMissile = true;
    public bool isShootMissile = false;
    public int currentMissiles;

    [Header("Blocking/Defense")]
    [Range(0f, 1f)]
    public float damageReduction;
    public bool isBlocking = false;
    public GameObject shield;


    private bool isAttacking = false;

    //private Animator anim;
    private Camera mainCamera;
    private PlayerMovement pM;
    private PlayerDash pDash;
    private CheckGround grndChck;
    [SerializeField]
    private CameraController camController;

    private Ray ray;
    private Vector3 direccionRayo;
    private Vector3 puntoObjetivo;

    private Coroutine coroutine;

    private WaitForSeconds waitFor = new WaitForSeconds(1f);

    public TypeOfAttack type;

    private void Start()
    {
        //animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        currentMissiles = maxMissiles;
        pM = GetComponent<PlayerMovement>();
        pDash = GetComponent<PlayerDash>();
        
        grndChck = GetComponent<CheckGround>();
        StartCoroutine(ChargeLaser());
        coroutine = StartCoroutine(OutOfCombatMode());
    }

    private void Update()
    {
        AttackType();
        ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
    }

    private void AttackType()
    {
        if (isAttacking || !grndChck.IsGrounded()) return;

        if (InputController.Instance.Blocking())
        {
            isBlocking = true;
            return;
        }
        else
        {
            isBlocking = false;
        }

        if (InputController.Instance.MainAttack() && pDash.isDashing == false)
        {
            if (objectInHand != null)
            {
                StartCoroutine(ThrowObject());
            }
            else
            {
                if (Input.GetKey(KeyCode.S))
                {
                    hitType = 1;
                    AudioManager.Instance.Play("Kick");

                }
                else if (InputController.Instance.JumpHold())
                {
                    hitType = 2;
                    AudioManager.Instance.Play("Upper");
                }
                else
                {
                    hitType = 0;
                    AudioManager.Instance.Play("Punch");

                }

                StartCoroutine(MeleeAttack());

            }
        }


        if (InputController.Instance.SecondaryAttack() && canShootMissile && pDash.isDashing == false)
        {
            StartCoroutine(ShootMissiles());
        }

        if (InputController.Instance.SpecialAttack() && pDash.isDashing == false && canUseLaser)
        {
            StartCoroutine(UseLaser());
        }
    }

    private IEnumerator MeleeAttack()
    {
        pM.lockMovement = true;
        isAttacking = true;
        attackCollider.gameObject.SetActive(true);
        isPunching = true;

        yield return new WaitForSeconds(0.3f);
        attackCollider.gameObject.SetActive(false);
        isPunching = false;
        //yield return new WaitForSeconds(0.25f);
        meleeTimer = meleeCooldown;
        while (meleeTimer > 0)
        {
            meleeTimer -= Time.deltaTime;
            yield return null;
        }

        isAttacking = false;
        pM.lockMovement = false;
        hitType = 0;
    }

    private IEnumerator ThrowObject()
    {
        isAttacking = true;
        isThrowing = true;
        //animator throw

        yield return new WaitForSeconds(0.3f);
        AudioManager.Instance.Play("Throw");
        objectInHand.transform.SetParent(null);
        CarObject carObject = objectInHand.GetComponent<CarObject>();
        Rigidbody rb = objectInHand.GetComponent<Rigidbody>();
        Collider collider = objectInHand.GetComponent<Collider>();

        if (rb != null)
        {
            rb.isKinematic = false;
            carObject.isThrown = true;

            rb.AddForce(mainCamera.transform.forward * throwForce, ForceMode.Impulse);
            rb.AddForce(mainCamera.transform.up * throwForce / 3, ForceMode.Impulse);
        }

        if (collider != null)
        {
            collider.enabled = true;
        }

        objectInHand = null;
        isAttacking = false;
        isThrowing = false;
    }

    private IEnumerator ShootMissiles()
    {
        isAttacking = true;
        canShootMissile = false;
        pM.lockMovement = true;
        isShootMissile = true;
        //animator
        yield return new WaitForSeconds(0.2f);
        AudioManager.Instance.Play("LanzarMisil");

        if (missileRPrefab != null && missileLPrefab != null && missilePointL != null && missilePointR != null && currentMissiles > 0)
        {
            GameObject missileL = Instantiate(missileLPrefab, missilePointL.position, missilePointL.rotation);
            GameObject missileR = Instantiate(missileRPrefab, missilePointR.position, missilePointR.rotation);
            Rigidbody rbL = missileL.GetComponent<Rigidbody>();
            Rigidbody rbR = missileR.GetComponent<Rigidbody>();


            if (rbL != null && rbR != null)
            {
                rbL.AddForce(mainCamera.transform.forward * missileForce, ForceMode.Impulse);
                rbR.AddForce(mainCamera.transform.forward * missileForce, ForceMode.Impulse);
            }

            currentMissiles -= 2;
        }
        isAttacking = false;

        yield return new WaitForSeconds(0.5f);
        pM.lockMovement = false;
        isShootMissile = false;

        yield return new WaitForSeconds(missileCooldown);
        canShootMissile = true;
    }

    private IEnumerator UseLaser()
    {
        isAttacking = true;
        canUseLaser = false;
        
        isUsingLaser = true;
        pM.lockMovement = true;
        yield return new WaitForSeconds(.5f);
        GameObject laser = laserOrigin.GetChild(0).gameObject;
        AudioManager.Instance.Play("Laser");

        float timer = 0f;
        ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        puntoObjetivo = ray.origin + ray.direction * laserRange;


        laserOrigin.LookAt(puntoObjetivo);
        camController.lockMainCamera = true;
        while (timer < laserDuration)
        {
           

            laser.SetActive(true);

            timer += Time.deltaTime;
            yield return null;
        }

        laser.SetActive(false);
        StartCoroutine(ChargeLaser());
        isAttacking = false;
        isUsingLaser = false;
        pM.lockMovement = false;
        camController.lockMainCamera = false;



        //yield return new WaitForSeconds(laserCooldown);
        //canUseLaser = true;


    }

    public IEnumerator ChargeLaser()
    {
        timer = 0;

        while (timer < laserCooldown)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        canUseLaser = true;
    }

    public IEnumerator OutOfCombatMode()
    {

        yield return waitFor;
        //camController.inCombat = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(laserOrigin.position, direccionRayo * laserRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(ray.origin, ray.direction * laserRange);
    }

    public enum TypeOfAttack
    {
        Melee,Laser, Kick
    }

}
