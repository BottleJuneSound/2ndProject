using UnityEngine;

public class NPC : MonoBehaviour
{
    //public GameObject pressPanel;

    [SerializeField]
    private PlayerController playerController;

    void Start()
    {
        playerController.pressPanel.SetActive(false);
        playerController.popupPanel.SetActive(false);

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
        if (playerController.interactiveAction.IsPressed() && playerController.pressPanel.activeSelf)
        {
            playerController.popupPanel.SetActive(true);
            Debug.Log("�۵�Ȯ�� " + playerController.activeInteract);

        }
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
            if (playerController.pressPanel.activeSelf)
            {
                Debug.Log("����!");
                return;
            }

            Debug.Log(playerController.activeInteract);
            playerController.activeInteract = false;
            playerController.pressPanel.SetActive(true);
            Debug.Log("����ִ�!");



        }

    }

    public void OnTriggerExit(Collider npcCollider)
    {
        if (npcCollider.gameObject.tag == "Player")
        {
            playerController.pressPanel.SetActive(false);
            playerController.popupPanel.SetActive(false);

            Debug.Log("�����");

            if(!playerController.pressPanel.activeSelf) //��ȣ�ۿ���·� ����� ��츦 ���
            {
                playerController.OffInteractive();    //�÷��̾� ��Ʈ�ѷ� Ŭ������ �ִ� �޼��� ���
                //playerController.activeInteract = false;    //�÷��̾� ��Ʈ�ѷ� Ŭ������ �ִ� boolŸ�� ����
                Debug.Log("��ȣ�ۿ���� ��Ȱ��ȭ");
            }

        }

    }
}
