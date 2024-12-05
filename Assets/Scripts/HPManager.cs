using UnityEngine;
using UnityEngine.UI;

public class HPManager : MonoBehaviour
{
    public float health;
    public float healthFaderSpeed = 0.2f;
    public Image playerHealth;
    public ItemManager itemManager;
    public PlayerController player;
    

    void Start()
    {
        health = 0.5f;  //�ӽð�. ���߿� 1�� �����ؾ���
        playerHealth.fillAmount = health;
    }

    void Update()
    {
        if(player == null)
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
            health -= 0.4f;
            health = Mathf.Clamp(health, 0, 1);
        }
        else if(health <= 0)
        {
            Debug.Log("�÷��̾� ü�� 0���� ���� ���� ��Ʈ�ϼ�!");
        }
    }

    //�극��ũ ����Ʈ�� �ʿ��ϴ�.
    //����ġ �극��ũ?
    //�Ѵ� ������ �� float�� ü���� ��������
    //�������� ����ϸ� ��float�� ü���� �������� �������Ѵ�.

}
