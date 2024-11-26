using UnityEngine;

public class NPC : MonoBehaviour
{
    public GameObject pressPanel;

    [SerializeField]
    private PlayerController playerController;

    void Start()
    {
        pressPanel.SetActive(false);
        if (playerController == null)
        {
            //�̷��� ���� ���� �־���� ���ٰ�?
            playerController = GetComponent<PlayerController>();

            //Debug.Log("������ null�̶��?");
        }
        //playerController.activeInteract = false;
        //Debug.Log(playerController.activeInteract);


    }

    void Update()
    {
        //if (gameObject.tag != "Player")
        //{
        //    pressPanel.SetActive(false);
        //    Debug.Log("�ȴ���ִ�");
        //}
        //if (!gameObject.CompareTag("Player"))
        //{
        //    pressPanel.SetActive(false);
        //    Debug.Log("�ȴ���ִ�");
        //}
        //if (gameObject.CompareTag("Player"))
        //{
        //    pressPanel.SetActive(true);
        //    Debug.Log("����ִ�!");
        //}

    }

    public void OnTriggerEnter(Collider npcCollider)
    {
        if (npcCollider.gameObject.tag == "Player")
        {
            Debug.Log(playerController.activeInteract);
            playerController.activeInteract = false;
            pressPanel.SetActive(true);
            Debug.Log("����ִ�!");

        }

    }

    public void OnTriggerExit(Collider npcCollider)
    {
        if (npcCollider.gameObject.tag == "Player")
        {
            pressPanel.SetActive(false);
            Debug.Log("�����");

            if(!pressPanel.activeSelf) //��ȣ�ۿ���·� ����� ��츦 ���
            {
                playerController.OffInteractive();    //�÷��̾� ��Ʈ�ѷ� Ŭ������ �ִ� �޼��� ���
                //playerController.activeInteract = false;    //�÷��̾� ��Ʈ�ѷ� Ŭ������ �ִ� boolŸ�� ����
                Debug.Log("��ȣ�ۿ���� ��Ȱ��ȭ");
            }

        }

    }
}
