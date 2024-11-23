using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    public GameObject player;
    public NavMeshAgent agent;
    private Vector3 lastPlayerPosition; // 이전 플레이어 위치 저장


    void Start()
    {
        GetComponent<Animator>().SetTrigger("BossIdle");
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();

        if (player != null)
        {
            lastPlayerPosition = player.transform.position; // 초기 위치 저장
            agent.destination = lastPlayerPosition; // 초기 목적지 설정

        }

    }

    // Update is called once per frame
    void Update()
    {

        if (player == null) return;
        if (agent.isStopped == true) return;


        // 플레이어 위치가 이전 위치와 달라졌을 때만 갱신
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
        lastPlayerPosition = player.transform.position; // 최신 위치 저장
        agent.destination = lastPlayerPosition; // 목적지 갱신
        GetComponent<Animator>().SetTrigger("BossIdle");
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Light"))
        {
            BossDown(); // BossDown 실행
        }
    }
}