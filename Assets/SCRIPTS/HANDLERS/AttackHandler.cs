using System.Collections;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    public Transform mechaHand;
    public Transform missilePoint;
    public Transform laserOrigin;

    public GameObject missilePrefab;

    [Header("Melee Attack")]
    public GameObject leftHand;
    public GameObject rightHand;
    public float meleeRange;
    public float meleeDamage;

    [Header("Throw Attack")]
    public float throwForce;

    [Header("Laser Attack")]
    public float laserRange;
    public float laserDamage;
    public float laserDuration;
    public float laserCooldown;

    [Header("Missiles")]
    public int maxMissiles;
    public float missileForce;

    public GameObject objectInHand;
    private int currentMissiles;
    private bool canUseLaser = false;
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
                //melee
                Debug.Log("Melee");
            }
        }

        if (InputController.Instance.SecondaryAttack())
        {
            //Missile
            Debug.Log("Missile");
        }

        if (InputController.Instance.SpecialAttack())
        {
            //laser
            Debug.Log("Laser");
        }
    }

    private IEnumerator MeleeAttack()
    {
        isAttacking = true;
        //animator attack

        yield return new WaitForSeconds(0.5f);

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

}
