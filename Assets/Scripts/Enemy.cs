using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    public GameObject player;
    public NavMeshAgent agent;
    private Vector3 lastPlayerPosition; // ���� �÷��̾� ��ġ ����


    void Start()
    {
        GetComponent<Animator>().SetTrigger("BossIdle");
        player = GameObject.FindWithTag("Player");
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

        if (player == null) return;
        if (agent.isStopped == true) return;


        // �÷��̾� ��ġ�� ���� ��ġ�� �޶����� ���� ����
        if (Vector3.Distance(lastPlayerPosition, player.transform.position) > 7)
        {
            BossMove();

        }

        //if (agent.remainingDistance < 5)
        //{
        //    BossDown();
        //}

    }

    public void BossDown()
    {
        agent.isStopped = true;
        GetComponent<Animator>().SetTrigger("BossHit");
        Invoke("BossMove", 5f);


    }

    public void BossMove()
    {
        agent.isStopped = false;
        lastPlayerPosition = player.transform.position; // �ֽ� ��ġ ����
        agent.destination = lastPlayerPosition; // ������ ����
        GetComponent<Animator>().SetTrigger("BossIdle");
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Light"))
        {
            BossDown(); // BossDown ����
        }
    }
}