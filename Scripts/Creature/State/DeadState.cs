using UnityEngine;

public class DeadState : IState
{
    private Creature creature;

    public DeadState(Creature _creature)
    {
        creature = _creature;
    }

    public void OnStateEnter()
    {
        creature.animator.SetBool("Walk", false);
        creature.animator.SetBool("Run", false);
        creature.agent.isStopped = true;
        creature.animator.speed = 1f;
        creature.animator.SetTrigger("Death");
    }

    public void OnStateUpdate()
    {
        
    }

    public void OnStateExit()
    {
        creature.animator.SetBool("Walk", false);
        creature.animator.SetBool("Run", false);
    }
}
