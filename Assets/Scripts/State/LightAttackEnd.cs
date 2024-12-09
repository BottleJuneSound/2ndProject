using UnityEngine;

public class LightAttackEnd : IState
{
    PlayerController player;

    public LightAttackEnd(PlayerController player)
    {
        this.player = player;
    }
    public void Enter()
    {
        player.GetComponent<Animator>().SetBool("LightAttackEnd", true);
    }

    public void Execute()
    {
    }

    public void Exit()
    {
        player.GetComponent<Animator>().SetBool("LightAttackEnd", false);

    }

}
