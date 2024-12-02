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
    public bool isRangeOut = false;    // 콜라이더 충돌에서 벗어난 경우 속도, 타이머 초기화를 위한 상태변수
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
        if (player == null || agent.isStopped == true || isBossDown) return;

        // 플레이어 위치가 이전 위치와 달라졌을 때만 갱신
        if (Vector3.Distance(lastPlayerPosition, player.transform.position) > 1)
        {
            BossMove();

        }
    }

    public void BossDown()
    {
        //보스 오브젝트 비활성화시 에러를 방지하기 위한 조건
        if (agent.enabled == true)
        {
            isBossDown = true;
            GetComponent<Animator>().SetTrigger("BossHit");
            //Debug.Log(agent.isStopped);
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            EndDown();

            //agent.enabled = false;        //이부분은 추후 살려낼 코드! 한번에 없어지지말고 점진적으로 사라져야함!
            //Invoke("BossSpawn", 5f);      //보스 스폰 메서드 제작 후 그곳에서 에이전트 활성화 후 보스무브 메서드로 넘기기
            //스폰될때도 점진적으로 스폰시키기!
        }
        else
        {
            Debug.Log("여기로 오는거 아니지?");

            return;
        }

    }

    public void EndDown()   //때리다가 멈추면 초기화 해주는 곳.완전히 다운되고 영역을 벗어났을때 초기화 해주기도한다. 아직 불안정한 메서드
    {
        isRangeOut = false;
        isBossDown = false;
        //triggerTime = 1f;
        //agent.speed = 5f;
        //agent.acceleration = 0.8f;
        //agent.stoppingDistance = 5;
        Invoke("BossMove", 5f);
    }

    public void BossMove()
    {
        if (agent.enabled == true)  //보스 오브젝트 비활성화시 에러를 방지하기 위한 조건
        {
            if (isBossDown == true) return;

            agent.isStopped = false;
            triggerTime = 1f;
            agent.speed = 10f;
            agent.acceleration = 8f;
            agent.stoppingDistance = 5;
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
            agent.speed = 0.3f;
            triggerLight = true;

            if (triggerLight) agent.velocity = Vector3.zero;
            
            //isRangeOut = true;
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
            agent.stoppingDistance -= Time.deltaTime;   //이 부분에서 클릭을 계속하고 있다면 타이머가 계속 음수로 진행됨.
                                                        //그래서 한번 쓰러진 적을 계속 잡아놓을 수 있는 부분이기도 함.
                                                        //이후 이부분 활용해서 일정시간이 지나면 보스 재소환 할 수 있을듯!

            if (triggerTime < 0 && agent.stoppingDistance > 2)
            {
                isRangeOut = true;
                BossDown();
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Light"))
        {
            triggerLight = false;

            if (isRangeOut)
            {
                EndDown();  // 때리다가 멈추면 초기화 해주는 곳. 완전히 다운되고 영역을 벗어났을때 초기화 해주기도한다.
                            // 그래서 애니메이션이 씹히는 에러가 존재하나?
            }

        }
    }
}