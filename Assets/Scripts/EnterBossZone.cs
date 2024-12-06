using UnityEngine;
using UnityEngine.AI;

public class EnterBossZone : MonoBehaviour
{
    public BoxCollider enterBossZone;
    public BossHPManager bossHP;

    void Start()
    {
        enterBossZone.isTrigger = false;
    }

    void Update()
    {
        if (!enterBossZone.isTrigger && bossHP.currentActiveIndex > 4)
        {
            if (enterBossZone.isTrigger) return;

            enterBossZone.isTrigger = true;
        }


    }
    public void OnTriggerEnter(Collider bossBox)
    {
        //if(bossBox.gameObject.tag == "Player" && bossHP.currentActiveIndex <= 4)
        //{
        //    Debug.Log("조건이 부합하지 않아 입장하지 못합니다.");

        //}

        if (bossBox.gameObject.tag == "Player" && bossHP.currentActiveIndex > 4)
        {
            Debug.Log("보스전 입장을 환영합니다");
            //enterBossZone.isTrigger = true;
            //Invoke("OnDestroy", 2f);

        }
    }

    //public void OnTriggerExit(Collider bossBox)   // 한번들어오면 다시 못나가게 할지 고민해보기
    //{
    //    if (bossBox.gameObject.tag == "Player" && bossHP.currentActiveIndex > 4)
    //    {
    //        Debug.Log("문이 닫혔습니다.");
    //        enterBossZone.isTrigger = false;
    //    }
    //}


    //public void OnDestroy()
    //{
    //    Destroy(gameObject);
    //}

    //public void OnCollisionEnter(Collision bossBox)
    //{
    //    if(CompareTag("Player") && bossHP.currentActiveIndex <= 4)
    //    {
    //        Debug.Log("조건이 부합하지 않아 입장하지 못합니다.");
    //    }

    //    if (bossBox.gameObject.tag == "Player" && bossHP.currentActiveIndex > 4)
    //    {
    //        Debug.Log("보스전 입장을 환영합니다");
    //        enterBossZone.isTrigger = true;

    //    }
    //}

}
