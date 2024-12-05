using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Enemy : MonoBehaviour
{   //종종 인보크 함수의 대기 시간을 모두 지키지 않고 바로 Move메서드가 실행되는 경우가 발생함
    //문제 해결 전이며 콜라이더 충돌에 대한 감지가 여러번 발생하며 생기는 오류로 추측하고 있음. 확인필요

    public GameObject player;
    public GameObject boss;
    public NavMeshAgent agent;
    private Vector3 lastPlayerPosition; // 이전 플레이어 위치 저장
    bool isBossDown = false;   // BossDown 실행 상태를 나타내는 플래그
    public bool isRangeOut = false;    // 콜라이더 충돌에서 벗어난 경우 속도, 타이머 초기화를 위한 상태변수
    public float triggerTime = 2f;
    public bool triggerLight = false;
    public bool bossAnimEnd = false;


    void Start()
    {
        boss.transform.position = new Vector3
            (Random.Range(96, 120),
            Random.Range(1.5f, 2),
            Random.Range(95, 103));

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
        if (player == null || isBossDown == true || isBossDown) return;

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
        }
        else
        {
            Debug.Log("여기로 오는거 아니지?");

            return;
        }

    }

    public void EndDown()   //콜라이더 충돌판정을 벗어났을때 실행하는 메서드
    {
        Debug.Log("EndDown 작동중?");
        isRangeOut = false;
        Invoke("DisableBoss", 2f);
    }

    public void DisableBoss()   //BossHit 애니메이션에서 사용되고 있음
    {
        Debug.Log("disable 작동중?");
        bossAnimEnd = true;
        agent.enabled = false;
        boss.SetActive(false);
        GetComponent<Animator>().ResetTrigger("BossHit");
        GetComponent<Animator>().ResetTrigger("BossIdle");
        Invoke("BossSpawn", 2f);
    }

    public void BossSpawn()    //보스가 다운되면 비활성화 및, 리셋 해주는 메서드
    {
        Debug.Log("spawn 작동중?");
        GetComponent<Animator>().ResetTrigger("BossHit");
        agent.enabled = true;
        boss.SetActive(true);
        isBossDown = false;
        bossAnimEnd = false;

        //보스가 스폰되는 랜덤레인지 추가필요
        boss.transform.position = new Vector3
            (Random.Range(96, 120),
            Random.Range(1.5f, 2),
            Random.Range(95, 103));


        BossMove();
    }


    public void BossMove()
    {
        if (agent.enabled == true)  //보스 오브젝트 비활성화시 에러를 방지하기 위한 조건
        {
            if (isBossDown == true) return;

            agent.isStopped = false;
            triggerTime = 1f;
            agent.speed = 7;
            agent.acceleration = 3f;
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
            
            triggerLight = true;

            if (triggerLight)
            {
                agent.velocity = Vector3.zero;


                if(agent.stoppingDistance < 5)
                {
                    //Debug.Log("작동하나요?");
                }

            }
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
            GetComponent<Animator>().SetBool("BossInter", true);
            triggerTime -= Time.deltaTime;
            agent.stoppingDistance -= Time.deltaTime;   //이 부분에서 클릭을 계속하고 있다면 타이머가 계속 음수로 진행됨.
                                                        //이후 이부분 활용해서 일정시간이 지나면 보스 재소환 할 수 있을듯!

            if (triggerTime < 0 && agent.stoppingDistance < 4)
            {
                Debug.Log("보스다운 조건 도달");
                isRangeOut = true;
                GetComponent<Animator>().SetBool("BossInter", false);
                BossDown();
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Light"))
        {
            // 때리다가 멈추면 초기화 해주는 곳.
            triggerLight = false;
            GetComponent<Animator>().SetBool("BossInter", false);


            if (isRangeOut && bossAnimEnd)
            {
                // 콜라이더의 충돌 이탈을 알려주는 곳이기 때문에 닿아있는 동안은 보스가 일어나지 않음.
                EndDown();  
            }

        }
    }
}