using UnityEngine;

public class StateMachine
{
    public IState CurrentState { get; private set; }
    PlayerController player;

    public IdleState idleState;
    public WalkState walkState;
    public RunState runState;
    public LightFindState lightFindState;

    public StateMachine(PlayerController player)
    {
        this.player = player;

        idleState = new IdleState(player);
        walkState = new WalkState(player);
        runState = new RunState(player);
        lightFindState = new LightFindState(player);
    }

    public void Initialize(IState state)
    {
        CurrentState = state;
        state.Enter();

    }

    public void TransitionTo(IState nextState)
    {
        CurrentState.Exit();
        CurrentState = nextState;
        CurrentState.Enter();
    }

    public void Execute()
    {
        CurrentState.Execute();
    }


}
