using UnityEngine;

public class PlayerDeathState : IState
{
    PlayerController player;

    public PlayerDeathState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.GetComponent<Animator>().SetBool("PlayerDeath", true);
    }

    public void Execute()
    {
    }

    public void Exit()
    {
        player.GetComponent<Animator>().SetBool("PlayerDeath", false);
    }


}
