using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Enemy : MonoBehaviour
{   //���� �κ�ũ �Լ��� ��� �ð��� ��� ��Ű�� �ʰ� �ٷ� Move�޼��尡 ����Ǵ� ��찡 �߻���
    //���� �ذ� ���̸� �ݶ��̴� �浹�� ���� ������ ������ �߻��ϸ� ����� ������ �����ϰ� ����. Ȯ���ʿ�

    public GameObject player;
    public GameObject boss;
    public NavMeshAgent agent;
    private Vector3 lastPlayerPosition; // ���� �÷��̾� ��ġ ����
    bool isBossDown = false;   // BossDown ���� ���¸� ��Ÿ���� �÷���
    public bool isRangeOut = false;    // �ݶ��̴� �浹���� ��� ��� �ӵ�, Ÿ�̸� �ʱ�ȭ�� ���� ���º���
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
            lastPlayerPosition = player.transform.position; // �ʱ� ��ġ ����
            agent.destination = lastPlayerPosition; // �ʱ� ������ ����

        }

    }

    // Update is called once per frame
    void Update()
    {
        if (player == null || isBossDown == true || isBossDown) return;

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
            isBossDown = true;
            GetComponent<Animator>().SetTrigger("BossHit");
            //Debug.Log(agent.isStopped);
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }
        else
        {
            Debug.Log("����� ���°� �ƴ���?");

            return;
        }

    }

    public void EndDown()   //�ݶ��̴� �浹������ ������� �����ϴ� �޼���
    {
        Debug.Log("EndDown �۵���?");
        isRangeOut = false;
        Invoke("DisableBoss", 2f);
    }

    public void DisableBoss()   //BossHit �ִϸ��̼ǿ��� ���ǰ� ����
    {
        Debug.Log("disable �۵���?");
        bossAnimEnd = true;
        agent.enabled = false;
        boss.SetActive(false);
        GetComponent<Animator>().ResetTrigger("BossHit");
        GetComponent<Animator>().ResetTrigger("BossIdle");
        Invoke("BossSpawn", 2f);
    }

    public void BossSpawn()    //������ �ٿ�Ǹ� ��Ȱ��ȭ ��, ���� ���ִ� �޼���
    {
        Debug.Log("spawn �۵���?");
        GetComponent<Animator>().ResetTrigger("BossHit");
        agent.enabled = true;
        boss.SetActive(true);
        isBossDown = false;
        bossAnimEnd = false;

        //������ �����Ǵ� ���������� �߰��ʿ�
        boss.transform.position = new Vector3
            (Random.Range(96, 120),
            Random.Range(1.5f, 2),
            Random.Range(95, 103));


        BossMove();
    }


    public void BossMove()
    {
        if (agent.enabled == true)  //���� ������Ʈ ��Ȱ��ȭ�� ������ �����ϱ� ���� ����
        {
            if (isBossDown == true) return;

            agent.isStopped = false;
            triggerTime = 1f;
            agent.speed = 7;
            agent.acceleration = 3f;
            agent.stoppingDistance = 5;
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
            
            triggerLight = true;

            if (triggerLight)
            {
                agent.velocity = Vector3.zero;


                if(agent.stoppingDistance < 5)
                {
                    //Debug.Log("�۵��ϳ���?");
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

    private void OnTriggerStay(Collider other)  // ���ǿ� ���� ���� ������ ���� �� �� �ֵ��� �غ���
    {
        if(triggerLight && other.CompareTag("Light"))
        {
            GetComponent<Animator>().SetBool("BossInter", true);
            triggerTime -= Time.deltaTime;
            agent.stoppingDistance -= Time.deltaTime;   //�� �κп��� Ŭ���� ����ϰ� �ִٸ� Ÿ�̸Ӱ� ��� ������ �����.
                                                        //���� �̺κ� Ȱ���ؼ� �����ð��� ������ ���� ���ȯ �� �� ������!

            if (triggerTime < 0 && agent.stoppingDistance < 4)
            {
                Debug.Log("�����ٿ� ���� ����");
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
            // �����ٰ� ���߸� �ʱ�ȭ ���ִ� ��.
            triggerLight = false;
            GetComponent<Animator>().SetBool("BossInter", false);


            if (isRangeOut && bossAnimEnd)
            {
                // �ݶ��̴��� �浹 ��Ż�� �˷��ִ� ���̱� ������ ����ִ� ������ ������ �Ͼ�� ����.
                EndDown();  
            }

        }
    }
}