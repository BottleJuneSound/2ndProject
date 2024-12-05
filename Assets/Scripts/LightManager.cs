using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class LightManager : MonoBehaviour
{
    public ItemManager itemManager;
    public PlayerController player;
    public float lightOil;
    public Image OilAmount;

    //public float AddOilSpeed = 0.3f;
    public float AddOilAmount = 0.3f;
    //public float AddOilTimer = 1;
    //public float AddOilDuration = 0;
    public bool pressKey = false;

   
    void Start()
    {
        lightOil = 0.5f;
        OilAmount.fillAmount = lightOil ;
    }

    void Update()
    {
        if(itemManager == null)
        {
            itemManager = GetComponent<ItemManager>();
        }


        if (player.AddLightOilAction.WasPressedThisFrame() && itemManager.lightCounter > 0)
        {
            if(pressKey) return;
            SpendLightChargeItem();
            itemManager.OnSpendLight();
            Debug.Log("몇번눌리는지 확인");
        }
        else if (player.AddLightOilAction.WasReleasedThisFrame())
        {
            pressKey = false;
        }

        if(player.lightAttackButton == true)
        {
            LightOilGauge();

            //조건문1 0이면 라이트 콜라이더 disable
            //조건문2 아이템 먹으면 시간 충전
        }
        else if(player.lightAttackButton == false)
        {
            LightOilAutoCharge();
        }


        OilAmount.fillAmount = lightOil;
        Debug.Log(player.lightAttackButton);
    }

    public void LightOilGauge()
    {
        lightOil -= Time.deltaTime / 3;
        lightOil = Mathf.Clamp(lightOil, 0, 1);
    }

    public void LightOilAutoCharge()
    {
        lightOil += Time.deltaTime / 100;
        lightOil = Mathf.Clamp(lightOil, 0, 1);
    }

    public void SpendLightChargeItem()
    {
        if (lightOil < 1)
        {
            pressKey = true;

            lightOil += AddOilAmount;
            //AddOilAmount = Mathf.Clamp(AddOilAmount, 0, 0.3f);
            lightOil = Mathf.Clamp(lightOil, 0, 1);
            Debug.Log("라이트 오일양 " + lightOil);
        }
        else if (lightOil >= 1)
        {
            pressKey = false;
            Debug.Log("오일이 가득합니다.");
            return;
        }

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
