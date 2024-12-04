using UnityEngine;
using UnityEngine.UI;

public class LightManager : MonoBehaviour
{
    public PlayerController player;
    public float lightOil;
    public Image OilAmount;
    

    void Start()
    {
        lightOil = 1;
        OilAmount.fillAmount = lightOil ;
    }

    void Update()
    {

        if(player.lightAttackButton == true)
        {
            LightOilGauge();

            //조건문1 0이면 라이트 콜라이더 disable
            //조건문2 아이템 먹으면 시간 충전
        }

        OilAmount.fillAmount = lightOil;

        Debug.Log(OilAmount);
    }

    public void LightOilGauge()
    {
        lightOil -= Time.deltaTime / 3;

        lightOil = Mathf.Clamp(lightOil, 0, 1);
    }

    // 아래 두 메서드는 라이트를 켜고 끄는 방식
    // enum사용하여 fps처럼 아이템 고를 수 있도록 제작
    // 고른아이템
    public void OffLight()
    {

    }

    public void OnLight()
    {

    }

}
