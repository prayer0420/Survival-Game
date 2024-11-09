using UnityEngine;

public class EscapeState : IState
{
    private Creature creature;

    public EscapeState(Creature _creature)
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
        creature.agent.SetDestination(PlayerManager.Instance.Player.transform.position + (DirectionToEscape() * creature.data.detectDistance * 1.5f));
    }

    public void OnStateExit()
    {
        creature.animator.SetBool("Walk", false);
        creature.animator.SetBool("Run", false);
    }

    private Vector3 DirectionToEscape()
    {
        return (creature.transform.position - PlayerManager.Instance.Player.transform.position).normalized;
    }
}
