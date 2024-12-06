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
        //    Debug.Log("������ �������� �ʾ� �������� ���մϴ�.");

        //}

        if (bossBox.gameObject.tag == "Player" && bossHP.currentActiveIndex > 4)
        {
            Debug.Log("������ ������ ȯ���մϴ�");
            //enterBossZone.isTrigger = true;
            //Invoke("OnDestroy", 2f);

        }
    }

    //public void OnTriggerExit(Collider bossBox)   // �ѹ������� �ٽ� �������� ���� ����غ���
    //{
    //    if (bossBox.gameObject.tag == "Player" && bossHP.currentActiveIndex > 4)
    //    {
    //        Debug.Log("���� �������ϴ�.");
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
    //        Debug.Log("������ �������� �ʾ� �������� ���մϴ�.");
    //    }

    //    if (bossBox.gameObject.tag == "Player" && bossHP.currentActiveIndex > 4)
    //    {
    //        Debug.Log("������ ������ ȯ���մϴ�");
    //        enterBossZone.isTrigger = true;

    //    }
    //}

}
