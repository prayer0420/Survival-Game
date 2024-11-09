using UnityEngine;

public class ChaseState : IState
{
    private Creature creature;

    public ChaseState(Creature _creature)
    {
        creature = _creature;
    }

    public void OnStateEnter()
    {
        creature.animator.SetBool("Walk", false);
        creature.animator.SetBool("Run", true);
        creature.agent.isStopped = false;
        creature.agent.speed = creature.data.runSpeed;
        //creature.animator.speed = creature.agent.speed / creature.data.walkSpeed;
    }

    public void OnStateUpdate()
    {
        creature.agent.SetDestination(PlayerManager.Instance.Player.transform.position);
    }

    public void OnStateExit()
    {
        creature.animator.SetBool("Walk", false);
        creature.animator.SetBool("Run", false);
    }
}