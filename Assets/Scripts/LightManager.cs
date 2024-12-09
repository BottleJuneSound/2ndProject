using UnityEngine;
using UnityEngine.UI;

public class LightManager : MonoBehaviour
{
    public ItemManager itemManager;
    public SoundManager soundManager;
    public PlayerController player;
    public float lightOil;
    public Image OilAmount;
    public Light handLight;

    //public float AddOilSpeed = 0.3f;
    public float AddOilAmount = 0.3f;
    //public float AddOilTimer = 1;
    //public float AddOilDuration = 0;
    public bool pressKey = false;
    public bool lightOff = false;


   
    void Start()
    {
        OffLight();
        lightOil = 0.5f;
        OilAmount.fillAmount = lightOil ;
    }

    void Update()
    {
        //아이템 매니저 null관리
        if (itemManager == null)
        {
            itemManager = GetComponent<ItemManager>();
        }

        
        //핸드랜턴 전원 상태관리
        if (player.lightOnOffAction.WasPressedThisFrame() && lightOff && itemManager.matcheCounter > 0)
        {
            OnLight();
        }
        else if (player.lightOnOffAction.WasPressedThisFrame() && !lightOff)
        {
            OffLight();
        }

        //핸드랜턴 충전으로 인한 게이지 관리
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

        //핸드랜턴 사용으로 인한 게이지 관리
        if(player.lightAttackButton == true && !lightOff)
        {
            LightOilGauge();

            //조건문1 0이면 라이트 콜라이더 disable
            //조건문2 아이템 먹으면 시간 충전
        }
        else if(player.lightAttackButton == false && lightOff)
        {
            LightOilAutoCharge();
        }

        else if(player.lightAttackButton == false && !lightOff && itemManager.lightCounter > 0) 
        {
            LightOilAutoSpend();
        }


        OilAmount.fillAmount = lightOil;
        //Debug.Log(player.lightAttackButton);
    }

    public void LightOilGauge()
    {
        lightOil -= Time.deltaTime / 10;
        lightOil = Mathf.Clamp(lightOil, 0, 1);

        if(lightOil <= 0)
        {
            OffLight();        
        }
    }

    public void LightOilAutoCharge()
    {
        lightOil += Time.deltaTime / 100;
        lightOil = Mathf.Clamp(lightOil, 0, 1);
    }

    public void LightOilAutoSpend()
    {
        lightOil -= Time.deltaTime / 150;
        lightOil = Mathf.Clamp(lightOil, 0, 1);
    }

    public void SpendLightChargeItem()
    {
        if (lightOil < 1 )
        {
            pressKey = true;

            lightOil += AddOilAmount;
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
        lightOff = true;
        handLight.enabled = false;
        player.lightAttackCollider.GetComponent<CapsuleCollider>().radius = 0;
        player.lightAttackCollider.GetComponent<CapsuleCollider>().height = 0;
    }

    public void OnLight()
    {
        soundManager.SpendMatcheSFX();
        itemManager.OnSpendMatche();
        lightOff = false;
        handLight.enabled = true;

    }

}
