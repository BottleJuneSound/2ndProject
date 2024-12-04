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

            //���ǹ�1 0�̸� ����Ʈ �ݶ��̴� disable
            //���ǹ�2 ������ ������ �ð� ����
        }

        OilAmount.fillAmount = lightOil;

        Debug.Log(OilAmount);
    }

    public void LightOilGauge()
    {
        lightOil -= Time.deltaTime / 3;

        lightOil = Mathf.Clamp(lightOil, 0, 1);
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
