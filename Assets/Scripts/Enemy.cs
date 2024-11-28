using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Enemy : MonoBehaviour
{   //종종 인보크 함수의 대기 시간을 모두 지키지 않고 바로 Move메서드가 실행되는 경우가 발생함
    //문제 해결 전이며 콜라이더 충돌에 대한 감지가 여러번 발생하며 생기는 오류로 추측하고 있음. 확인필요

    public GameObject player;
    public NavMeshAgent agent;
    private Vector3 lastPlayerPosition; // 이전 플레이어 위치 저장
    bool isBossDown = false;   // BossDown 실행 상태를 나타내는 플래그



    void Start()
    {
        GetComponent<Animator>().SetTrigger("BossIdle");
        //player = GameObject.FindWithTag("Player");
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

        if (player == null || agent.isStopped == true) return;

        // 플레이어 위치가 이전 위치와 달라졌을 때만 갱신
        if (Vector3.Distance(lastPlayerPosition, player.transform.position) > 1)
        {
            BossMove();

        }

    }

    public void BossDown()
    {
        agent.isStopped = true;
        GetComponent<Animator>().SetTrigger("BossHit");
    }

    public void EndDown()
    {
        isBossDown = false;
        Invoke("BossMove", 5f);

    }
    public void BossMove()
    {
        if (agent.enabled == true)  //보스 오브젝트 비활성화시 에러를 방지하기 위한 조건
        {
            if (isBossDown == true) return;

            agent.isStopped = false;
            lastPlayerPosition = player.transform.position; // 최신 위치 저장
            agent.destination = lastPlayerPosition; // 목적지 갱신
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
            isBossDown = true;
            BossDown();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Light"))
        {
            EndDown();

        }
    }
}