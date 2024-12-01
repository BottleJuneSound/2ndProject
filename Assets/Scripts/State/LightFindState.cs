using UnityEngine;

public class LightFindState : IState
{
    PlayerController player;

    public LightFindState (PlayerController player)
    {
        this.player = player;
    }
    public void Enter()
    {
        player.GetComponent<Animator>().SetBool("LightFindBool", true);
        //Debug.Log("11");
    }

    public void Execute()
    {
    }

    public void Exit()
    {
        player.GetComponent<Animator>().SetBool("LightFindBool", false);

    }

}
