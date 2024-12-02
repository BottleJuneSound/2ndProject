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
    public bool isRangeOut = false;    // �ݶ��̴� �浹���� ��� ��� �ӵ�, Ÿ�̸� �ʱ�ȭ�� ���� ���º���
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
        if (player == null || agent.isStopped == true || isBossDown) return;

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
            EndDown();

            //agent.enabled = false;        //�̺κ��� ���� ����� �ڵ�! �ѹ��� ������������ ���������� ���������!
            //Invoke("BossSpawn", 5f);      //���� ���� �޼��� ���� �� �װ����� ������Ʈ Ȱ��ȭ �� �������� �޼���� �ѱ��
            //�����ɶ��� ���������� ������Ű��!
        }
        else
        {
            Debug.Log("����� ���°� �ƴ���?");

            return;
        }

    }

    public void EndDown()   //�����ٰ� ���߸� �ʱ�ȭ ���ִ� ��.������ �ٿ�ǰ� ������ ������� �ʱ�ȭ ���ֱ⵵�Ѵ�. ���� �Ҿ����� �޼���
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
        if (agent.enabled == true)  //���� ������Ʈ ��Ȱ��ȭ�� ������ �����ϱ� ���� ����
        {
            if (isBossDown == true) return;

            agent.isStopped = false;
            triggerTime = 1f;
            agent.speed = 10f;
            agent.acceleration = 8f;
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

    private void OnTriggerStay(Collider other)  // ���ǿ� ���� ���� ������ ���� �� �� �ֵ��� �غ���
    {
        if(triggerLight && other.CompareTag("Light"))
        {
            triggerTime -= Time.deltaTime;
            agent.stoppingDistance -= Time.deltaTime;   //�� �κп��� Ŭ���� ����ϰ� �ִٸ� Ÿ�̸Ӱ� ��� ������ �����.
                                                        //�׷��� �ѹ� ������ ���� ��� ��Ƴ��� �� �ִ� �κ��̱⵵ ��.
                                                        //���� �̺κ� Ȱ���ؼ� �����ð��� ������ ���� ���ȯ �� �� ������!

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
                EndDown();  // �����ٰ� ���߸� �ʱ�ȭ ���ִ� ��. ������ �ٿ�ǰ� ������ ������� �ʱ�ȭ ���ֱ⵵�Ѵ�.
                            // �׷��� �ִϸ��̼��� ������ ������ �����ϳ�?
            }

        }
    }
}