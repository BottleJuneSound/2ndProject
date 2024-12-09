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
        gameClearText = "보스를 처치하여 마을이 정상화 되었습니다:) " +
                "\n추후 추가 콘텐츠를 추가하여 " +
                "\n완성도를 높일 수 있도록 하겠습니다. " +
                "\n플레이 해주셔서 진심으로 감사드립니다:)";
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

            if(currentActiveIndex == 0)
            {
                boss.BossDie();
                //player.moveAction.Disable();
                Debug.Log("보스가 죽었습니다.");
                gameClerarAlarm.SetActive(true);
                gameClearTMP.text = gameClearText;
            }
        }
        else
        {
            Debug.Log("비활성화할 오브젝트가 없습니다.");
        }
    }

}
