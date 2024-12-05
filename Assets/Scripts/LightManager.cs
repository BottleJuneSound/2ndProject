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
            Debug.Log("����������� Ȯ��");
        }
        else if (player.AddLightOilAction.WasReleasedThisFrame())
        {
            pressKey = false;
        }

        if(player.lightAttackButton == true)
        {
            LightOilGauge();

            //���ǹ�1 0�̸� ����Ʈ �ݶ��̴� disable
            //���ǹ�2 ������ ������ �ð� ����
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

    }

    public void OnLight()
    {

    }

}
