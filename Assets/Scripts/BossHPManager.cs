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
            bossHPList[currentActiveIndex].SetActive(true); // 씬에서 실제로 활성화
            currentActiveIndex++; // 다음 인덱스를 준비
        }
        else
        {
            Debug.Log("모든 오브젝트가 이미 활성화되었습니다.");
        }
    }

    // 역순으로 비활성화
    public void BossHPSubtract()
    {
        if (currentActiveIndex > 0)
        {
            currentActiveIndex--; // 이전 인덱스
            bossHPList[currentActiveIndex].SetActive(false); // 씬에서 실제로 비활성화
        }
        else
        {
            Debug.Log("비활성화할 오브젝트가 없습니다.");
        }
    }

}
