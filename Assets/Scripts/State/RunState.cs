using UnityEngine;

public class RunState : IState
{
    PlayerController player;

    public RunState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.GetComponent<Animator>().SetTrigger("PlayerRun");
    }

    public void Execute()
    {

    }
    public void Exit()
    {

    }
}
