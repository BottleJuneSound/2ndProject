using System.Collections.Generic;
using UnityEngine;

public class BossHPManager : MonoBehaviour
{
    public List<GameObject> bossHPList;
    public int currentActiveIndex = 0;

    void Start()
    {
        foreach (GameObject bossHPObj in bossHPList)
        {
            bossHPObj.SetActive(false);
        }
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
        }
        else
        {
            Debug.Log("��Ȱ��ȭ�� ������Ʈ�� �����ϴ�.");
        }
    }

}
