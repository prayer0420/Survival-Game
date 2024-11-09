public class IdleState : IState
{
    private Creature creature;

    public IdleState(Creature _creature)
    {
        creature = _creature;
    }

    public void OnStateEnter()
    {
        creature.animator.SetBool("Walk", false);
        creature.animator.SetBool("Run", false);
        creature.agent.isStopped = true;
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
