using UnityEngine;

public class IdleState : IState
{
    PlayerController player;
    public IdleState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.GetComponent<Animator>().SetTrigger("PlayerIdle");
    }

    public void Execute()
    {

    }

    public void Exit()
    {

    }
}
