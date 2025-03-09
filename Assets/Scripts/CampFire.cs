using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    public int damage;
    public float damageRate;

    List<IDamagable> things = new List<IDamagable>();

    void Start()
    {
        InvokeRepeating(nameof(DealDamage), 0, damageRate);
    }

    void DealDamage()
    {
        foreach(IDamagable damagable in things)
        {
            damagable.TakeDamage(damage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out  IDamagable damagable))
        {
            things.Add(damagable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IDamagable damagable))
        {
            things.Remove(damagable);
        }
    }
}
