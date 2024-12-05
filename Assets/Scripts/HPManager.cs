using UnityEngine;
using UnityEngine.UI;

public class HPManager : MonoBehaviour
{
    public float health;
    public float healthFaderSpeed = 0.2f;
    public Image playerHealth;
    

    void Start()
    {
        health = 1;
        playerHealth.fillAmount = health;
    }

    void Update()
    {
        //if(������ Ÿ�ݰ� ���õ� �ൿ)
        //{
        //    LossHealth();
        //}
        //playerHealth.fillAmount = health;

    }

    public void LossHealth()
    {
        health -= Time.deltaTime / 3;
        health = Mathf.Clamp(health, 0, 1);
    }

    //�극��ũ ����Ʈ�� �ʿ��ϴ�.
    //����ġ �극��ũ?
    //�Ѵ� ������ �� float�� ü���� ��������
    //�������� ����ϸ� ��float�� ü���� �������� �������Ѵ�.

}
