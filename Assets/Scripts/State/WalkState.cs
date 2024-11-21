using UnityEngine;

public class WalkState : IState
{
    PlayerController player;

    public WalkState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.GetComponent<Animator>().SetTrigger("PlayerWalk");
    }

    public void Execute()
    {

    }
    public void Exit()
    {

    }

}
