using UnityEngine;

public class AttackState : IState
{
    private Creature creature;

    public AttackState(Creature _creature)
    {
        creature = _creature;
    }

    private float lastAttackTime;

    public void OnStateEnter()
    {
        creature.animator.SetBool("Walk", false);
        creature.animator.SetBool("Run", false);

        creature.agent.isStopped = true;
        
        creature.animator.speed = 1;
    }

    public void OnStateUpdate()
    {
        if (Time.time - lastAttackTime > creature.data.attackRate)
        {
            lastAttackTime = Time.time;
            creature.animator.SetTrigger("Attack");
            PlayerManager.Instance.Player.controller.GetComponent<IDamagable>().TakeDamage(creature.data.damage);
        }
    }

    public void OnStateExit()
    {
        creature.animator.SetBool("Walk", false);
        creature.animator.SetBool("Run", false);
    }
}
