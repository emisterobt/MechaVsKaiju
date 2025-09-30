using UnityEngine;

public class FallingAnimation : MonoBehaviour
{

    public Animator anim;
    public ParticleSystem particula;
    void Start()
    {
        anim = GetComponent<Animator>();
        particula = GetComponent<ParticleSystem>();
    }


    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Destruction"))
        {
            particula.Play();
            anim.SetBool("isFalling", true);
        }
    }
}
