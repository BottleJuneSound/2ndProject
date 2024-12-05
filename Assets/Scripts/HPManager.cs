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
        //if(보스의 타격과 관련된 행동)
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

    //브레이크 포인트가 필요하다.
    //스위치 브레이크?
    //한대 맞으면 몇 float의 체력이 감소할지
    //아이템을 사용하면 몇float의 체력이 증가할지 만들어야한다.

}
