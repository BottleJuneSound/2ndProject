using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossHPManager : MonoBehaviour
{
    public Enemy boss;
    public PlayerController player;
    public List<GameObject> bossHPList;
    public int currentActiveIndex = 0;

    public GameObject gameClerarAlarm;
    public TMP_Text gameClearTMP;
    public string gameClearText;

    void Start()
    {
        gameClearText = "������ óġ�Ͽ� ������ ����ȭ �Ǿ����ϴ�:) " +
                "\n���� �߰� �������� �߰��Ͽ� " +
                "\n�ϼ����� ���� �� �ֵ��� �ϰڽ��ϴ�. " +
                "\n�÷��� ���ּż� �������� ����帳�ϴ�:)";
        gameClerarAlarm.SetActive(false);
        //foreach (GameObject bossHPObj in bossHPList)
        //{
        //    bossHPObj.SetActive(false);
        //}
    }

    void Update()
    {
        
    }
    public void BossHPAdd()
    {
        if (currentActiveIndex < bossHPList.Count)
        {
            bossHPList[currentActiveIndex].SetActive(true); // ������ ������ Ȱ��ȭ
            currentActiveIndex++; // ���� �ε����� �غ�
        }
        else
        {
            Debug.Log("��� ������Ʈ�� �̹� Ȱ��ȭ�Ǿ����ϴ�.");
        }
    }

    // �������� ��Ȱ��ȭ
    public void BossHPSubtract()
    {
        if (currentActiveIndex > 0)
        {
            currentActiveIndex--; // ���� �ε���
            bossHPList[currentActiveIndex].SetActive(false); // ������ ������ ��Ȱ��ȭ

            if(currentActiveIndex == 0)
            {
                boss.BossDie();
                //player.moveAction.Disable();
                Debug.Log("������ �׾����ϴ�.");
                gameClerarAlarm.SetActive(true);
                gameClearTMP.text = gameClearText;
            }
        }
        else
        {
            Debug.Log("��Ȱ��ȭ�� ������Ʈ�� �����ϴ�.");
        }
    }

}
