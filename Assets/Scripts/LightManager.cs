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
        //������ �Ŵ��� null����
        if (itemManager == null)
        {
            itemManager = GetComponent<ItemManager>();
        }

        
        //�ڵ巣�� ���� ���°���
        if (player.lightOnOffAction.WasPressedThisFrame() && lightOff && itemManager.matcheCounter > 0)
        {
            OnLight();
        }
        else if (player.lightOnOffAction.WasPressedThisFrame() && !lightOff)
        {
            OffLight();
        }

        //�ڵ巣�� �������� ���� ������ ����
        if (player.AddLightOilAction.WasPressedThisFrame() && itemManager.lightCounter > 0)
        {
            if(pressKey) return;
            SpendLightChargeItem();
            itemManager.OnSpendLight();
            Debug.Log("����������� Ȯ��");
        }
        else if (player.AddLightOilAction.WasReleasedThisFrame())
        {
            pressKey = false;
        }

        //�ڵ巣�� ������� ���� ������ ����
        if(player.lightAttackButton == true && !lightOff)
        {
            LightOilGauge();

            //���ǹ�1 0�̸� ����Ʈ �ݶ��̴� disable
            //���ǹ�2 ������ ������ �ð� ����
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
            Debug.Log("����Ʈ ���Ͼ� " + lightOil);
        }
        else if (lightOil >= 1)
        {
            pressKey = false;
            Debug.Log("������ �����մϴ�.");
            return;
        }

    }

    // �Ʒ� �� �޼���� ����Ʈ�� �Ѱ� ���� ���
    // enum����Ͽ� fpsó�� ������ �� �� �ֵ��� ����
    // ��������
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
