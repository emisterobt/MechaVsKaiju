using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(float damage);

    public void OnDeath();// Death = (vida = 0)


}
