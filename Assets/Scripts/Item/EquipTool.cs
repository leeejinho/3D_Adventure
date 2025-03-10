using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipTool : Equip
{
    public float attackRate;
    public float attackDistance;
    public float useStamina;
    bool attacking;

    [Header("Resource Gathering")]
    public bool doesGatherResources;
    public ResourceType type;

    [Header("Combat")]
    public bool doesDealDamage;
    public int damage;

    private Animator animator;
    private Camera mainCam;

    private void Start()
    {
        animator = GetComponent<Animator>();
        mainCam = Camera.main;
    }

    public override void OnAttackInput()
    {
        if (!attacking)
        {
            if (GameManager.Instance.player.condition.UseStamina(useStamina))
            {
                attacking = true;
                animator.SetTrigger("Attack");
                Invoke("OnCanAttack", attackRate);
            }
        }
    }
    void OnCanAttack()
    {
        attacking = false;
    }

    public void OnHit()
    {
        Ray ray = mainCam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, attackDistance))
        {
            if (doesGatherResources && hit.collider.TryGetComponent(out Resource resource) && type == resource.type)
            {
                resource.Gather(hit.point, hit.normal);
            }
            if (doesDealDamage && hit.collider.TryGetComponent(out IDamagable damagable))
            {
                damagable.TakeDamage(damage);
            }
        }
    }
}
