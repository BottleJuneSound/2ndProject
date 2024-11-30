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
    bool isRangeOut = false;    // 콜라이더 충돌에서 벗어난 경우 속도, 타이머 초기화를 위한 상태변수
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

    public void SetBossSpeed()  //플레이어와 상호작용 상태에 따라 이동 속도, 보스다운 타이머 초기화 하는 메서드
    {
        isRangeOut = false;
        isBossDown = false;
        triggerTime = 0.5f;
        agent.speed = 5f;
        agent.acceleration = 0.8f;
        agent.stoppingDistance = 5;
        //GetComponent<Animator>().SetTrigger("BossIdle");

    }

    public void BossDown()
    {
        //보스 오브젝트 비활성화시 에러를 방지하기 위한 조건
        if (agent.enabled == true)
        {
            agent.isStopped = true;
            isBossDown = true;
            GetComponent<Animator>().SetTrigger("BossHit");
            Invoke("SetBossSpeed", 5f);
            Invoke("BossMove", 5f);

        }
        else
        {
            return;
        }

    }

    public void EndDown()   //보스가 다운 후 플레이어가 범위안에 있다면 바로 일어서지 않도록 하는 메서드
    {
        isBossDown = false;
        triggerTime = 0.5f;
        agent.speed = 5f;
        agent.acceleration = 0.8f;
        agent.stoppingDistance = 5;

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
            agent.acceleration = 0.1f;
            agent.speed = 0.5f;
            triggerLight = true;
            isRangeOut = true;
            //Debug.Log(agent.speed);

            //isBossDown = true;
            //Invoke("BossDown", 2f);
            //BossDown();

            //if(triggerTime > 0 && !isBossDown) agent.speed = 15f;

        }
    }

    private void OnTriggerStay(Collider other)  // 조건에 따라 보스 움직임 제한 할 수 있도록 해보자
    {
        if(triggerLight && other.CompareTag("Light"))
        {
            triggerTime -= Time.deltaTime;
            agent.stoppingDistance -= Time.deltaTime;

            if (triggerTime < 0 && agent.stoppingDistance > 2)
            {
                isRangeOut = true;
                BossDown();
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
            if (isRangeOut)
            {

                EndDown();

            }
        }
    }
}