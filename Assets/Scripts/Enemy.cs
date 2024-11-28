using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Enemy : MonoBehaviour
{   //���� �κ�ũ �Լ��� ��� �ð��� ��� ��Ű�� �ʰ� �ٷ� Move�޼��尡 ����Ǵ� ��찡 �߻���
    //���� �ذ� ���̸� �ݶ��̴� �浹�� ���� ������ ������ �߻��ϸ� ����� ������ �����ϰ� ����. Ȯ���ʿ�

    public GameObject player;
    public NavMeshAgent agent;
    private Vector3 lastPlayerPosition; // ���� �÷��̾� ��ġ ����
    bool isBossDown = false;   // BossDown ���� ���¸� ��Ÿ���� �÷���
    public float triggerTime = 2f;
    public bool triggerLight = false;
    



    void Start()
    {
        //GetComponent<NavMeshAgent>().speed = 30;
        GetComponent<Animator>().SetTrigger("BossIdle");
        //player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();

        if (player != null)
        {
            lastPlayerPosition = player.transform.position; // �ʱ� ��ġ ����
            agent.destination = lastPlayerPosition; // �ʱ� ������ ����

        }

    }

    // Update is called once per frame
    void Update()
    {
        if (player == null || agent.isStopped == true) return;

        // �÷��̾� ��ġ�� ���� ��ġ�� �޶����� ���� ����
        if (Vector3.Distance(lastPlayerPosition, player.transform.position) > 1)
        {
            BossMove();

        }

    }

    public void BossDown()
    {
        //���� ������Ʈ ��Ȱ��ȭ�� ������ �����ϱ� ���� ����
        if (agent.enabled == true)
        {
            triggerTime = 2f;
            agent.speed = 15f;
            agent.isStopped = true;
            GetComponent<Animator>().SetTrigger("BossHit");
        }
        else
        {
            return;
        }

    }

    public void EndDown()
    {
        isBossDown = false;
        Invoke("BossMove", 5f);

    }
    public void BossMove()
    {
        if (agent.enabled == true)  //���� ������Ʈ ��Ȱ��ȭ�� ������ �����ϱ� ���� ����
        {
            if (isBossDown == true) return;

            agent.isStopped = false;
            lastPlayerPosition = player.transform.position; // �ֽ� ��ġ ����
            agent.destination = lastPlayerPosition; // ������ ����
            GetComponent<Animator>().SetTrigger("BossIdle");
        }
        else
        {
            return;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Light"))
        {
            agent.speed = 0.5f;
            triggerLight = true;
            //Debug.Log(agent.speed);

            //isBossDown = true;
            //Invoke("BossDown", 2f);
            //BossDown();

            //if(triggerTime > 0 && !isBossDown) agent.speed = 15f;
            
        }
    }

    private void OnTriggerStay(Collider other)  // ���ǿ� ���� ���� ������ ���� �� �� �ֵ��� �غ���
    {
        if(triggerLight && other.CompareTag("Light"))
        {
            triggerTime -= Time.deltaTime;

            if(triggerTime <= 0)
            {
                isBossDown = true;
                Invoke("BossDown", 2f);
            }
        }
        //else
        //{
        //    triggerLight = true;
        //    triggerTime = 2f;
        //    BossMove();
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Light"))
        {
            EndDown();

        }
    }
}