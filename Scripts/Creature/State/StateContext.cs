public enum CreatureState
{
    Idle,
    Wander,
    Alerted,
    Chase,
    Attack,
    Escape,
    Dead
}

public class StateContext
{
    public IState currentState;

    public IdleState idle;
    public WanderState wander;
    public AlertedState alerted;
    public ChaseState chase;
    public AttackState attack;
    public EscapeState escape;
    public DeadState dead;

    public StateContext(Creature _creature)
    {
        idle = new IdleState(_creature);
        wander = new WanderState(_creature);
        alerted = new AlertedState(_creature);
        chase = new ChaseState(_creature);
        attack = new AttackState(_creature);
        escape = new EscapeState(_creature);
        dead = new DeadState(_creature);
    }

    public void Transition(IState newState)
    {
        if (currentState == newState) return;
        if (currentState != null) currentState.OnStateExit();
        currentState = newState;
        currentState.OnStateEnter();
    }
}