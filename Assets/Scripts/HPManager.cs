using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class HPManager : MonoBehaviour
{
    public float health;
    public float healthFaderSpeed = 0.2f;
    public Image playerHealth;
    public ItemManager itemManager;
    public PlayerController player;
    public Image hpAlarm;
    public StateMachine stateMachine;
    public bool playerDeath;

    public GameObject gameOverAlarm;
    public TMP_Text gameOverTMP;
    public string gameOverText;
    CharacterController characterController;

    void Start()
    {
        gameOverText = "������ ���� ������ ������ ���߽��ϴ�.";
        gameOverAlarm.SetActive(false);

        playerDeath = false;
        hpAlarm.enabled = false;
        health = 0.5f;  //�ӽð�. ���߿� 1�� �����ؾ���
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

        //if (������ Ÿ�ݰ� ���õ� �ൿ)
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
            Debug.Log("ü���� ���� á���ϴ�!");
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
        else if (health <= 0)
        {
            if (!hpAlarm.enabled) return;
            
            playerDeath = true;

            player.GetComponent<Animator>().ResetTrigger("PlayerIdle");
            player.GetComponent<Animator>().ResetTrigger("PlayerRun");
            player.GetComponent<Animator>().ResetTrigger("PlayerWalk");
            //player.GetComponent<Animator>().SetBool("LightAttackBool", false);


            player.OnPlayerDeath();
            hpAlarm.enabled = false;
            gameOverAlarm.SetActive(true);
            gameOverTMP.text = gameOverText;

            player.lightAttackAction.Disable();
            player.activeInteract = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Debug.Log("�÷��̾� ü�� 0���� ���� ���� ��Ʈ�ϼ�!");
            //Invoke("GameOverPopUp", 1f);
        }
    }


    //public void PlayerDie()
    //{
    //    if(health <= 0)
    //    {
    //        player.OnPlayerDeath();
    //        hpAlarm.enabled = false;
    //        Debug.Log("�÷��̾� ü�� 0���� ���� ���� ��Ʈ�ϼ�!");
    //        //Invoke("GameOverPopUp", 1f);
    //    }

    //}
    //�극��ũ ����Ʈ�� �ʿ��ϴ�.
    //����ġ �극��ũ?
    //�Ѵ� ������ �� float�� ü���� ��������
    //�������� ����ϸ� ��float�� ü���� �������� �������Ѵ�.

}
