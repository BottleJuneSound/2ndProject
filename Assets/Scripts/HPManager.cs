using UnityEngine;
using UnityEngine.UI;

public class HPManager : MonoBehaviour
{
    public float health;
    public float healthFaderSpeed = 0.2f;
    public Image playerHealth;
    public ItemManager itemManager;
    public PlayerController player;
    public Image hpAlarm;
    

    void Start()
    {
        hpAlarm.enabled = false;
        health = 0.5f;  //임시값. 나중에 1로 변경해야함
        playerHealth.fillAmount = health;
    }

    void Update()
    {

        if (player == null)
        {
            player = GetComponent<PlayerController>();
        }


        if (player.AddPotionSpendAction.WasPressedThisFrame())
        {
            AddHealth();
        }

        //if (보스의 타격과 관련된 행동)
        //{
        //    LossHealth();
        //}
        playerHealth.fillAmount = health;
        //hpAlarm.enabled = false;

    }

    public void AddHealth()
    {
        if(itemManager.potionCounter > 0)
        {
            itemManager.OnSpendPotion();
            health += 0.5f;
            health = Mathf.Clamp(health, 0, 1);
        }
        else if(itemManager.potionCounter >= 1)
        {
            Debug.Log("체력이 가득 찼습니다!");
        }

    }

    public void LossHealth()
    {
        if(health > 0)
        {
            health -= Time.deltaTime / 20 ;
            health = Mathf.Clamp(health, 0, 1);
            hpAlarm.enabled = true;
        }
        else if(health <= 0)
        {
            Debug.Log("플레이어 체력 0으로 게임 종료 리트하셈!");
        }
    }


    public void PlayerDie()
    {
        if(health <= 0)
        {
            hpAlarm.enabled = false;
            //Invoke("GameOverPopUp", 1f);
        }

    }
    //브레이크 포인트가 필요하다.
    //스위치 브레이크?
    //한대 맞으면 몇 float의 체력이 감소할지
    //아이템을 사용하면 몇float의 체력이 증가할지 만들어야한다.

}
