using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipTool : EquipObject
{
    public float attackRate;
    public float attackRange;
    public float useStamina;
    private bool attacking;

    [Header("Resource Gather")]
    public bool isTool;
    public ToolType toolType;

    [Header("Combat")]
    public bool isWeapon;
    public int damage;

    public LayerMask layer;

    private Camera cam;

    private CharacterBuffManager atkBuff;
    private Animator animator;

    private void Awake()
    {
        cam = Camera.main;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            OnAttackInput();
        }
    }

    public override void OnAttackInput()
    {
        if(!attacking)
        {
            attacking = true;
            animator.SetTrigger("Attack");
            Invoke("OnCanAttack", attackRate);
        }
    }

    public override void OnEquiped(CharacterBuffManager buff)
    {
        atkBuff = buff;
    }

    private void OnCanAttack()
    {
        attacking = false;
    }

    public void OnHit()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, attackRange, layer))
        {
            if(isTool && hit.collider.TryGetComponent(out Resource resource))
            {
                resource.Gather(hit.point, hit.normal, toolType);
            }

            if(isWeapon && hit.collider.TryGetComponent(out Creature creature))
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.attackClip);
                float dealDamage = damage + atkBuff.AttackUp;
                creature.TakeDamage(dealDamage);
                creature.isAttacked = true;
            }
        }
    }
}
