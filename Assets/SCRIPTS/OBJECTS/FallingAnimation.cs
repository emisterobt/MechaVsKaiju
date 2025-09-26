using UnityEngine;

public class FallingAnimation : MonoBehaviour
{
 
    public Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }


    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Destruction"))
        {
            anim.SetBool("isFalling", true);
        }
    }
}
