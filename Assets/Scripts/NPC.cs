using UnityEngine;

public class NPC : MonoBehaviour
{
    //public GameObject pressPanel;
    public npcNeedSkill currentNeedSkill;
    public bool nowInteracting = false;
    public enum npcNeedSkill
    {
        medicine,
        blood,
        pray
    }


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
        // NPC�� ���ϴ� ��ȣ�ۿ��� �������� ����(����)
        currentNeedSkill = (npcNeedSkill)Random.Range(0, 3);
        //Debug.Log(currentNeedSkill);

    }

    void Update()
    {
        if (nowInteracting && playerController.interactiveAction.IsPressed() && playerController.pressPanel.activeSelf)
        {
            playerController.popupPanel.SetActive(true);
            //Debug.Log("�۵�Ȯ�� " + playerController.activeInteract);
        }

        if (nowInteracting && (playerController.skillMAction.IsPressed() ||
            playerController.skillNAction.IsPressed() ||
            playerController.skillBAction.IsPressed()) &&
            playerController.popupPanel.activeSelf)
        {
            //Debug.Log("���ǹ� ���");

            // NPC�� ���ϴ� ��ų�� �´��� Ȯ��
            if (CheckSkillMatch() == true)
            {
                Debug.Log($"���� �Դϴ�. NPC: {gameObject.name}");
                return;

            }
            Debug.Log($"�߸��� ��ȣ�ۿ��Դϴ�. NPC: {gameObject.name}");  // ������ �����쿡�� �ش� �αװ� ��µǴ� ������ ������ ����. ���� �ʿ�

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

    // ���� NPC�� �䱸�ϴ� ��ų�� �÷��̾��� ��ų�� ��ġ�ϴ��� Ȯ��
    private bool CheckSkillMatch()
    {
        //Debug.Log(playerController.onSkillM);

        if (currentNeedSkill == npcNeedSkill.medicine && playerController.onSkillM)
        {
            playerController.onSkillM = false;
            return true;
        }
        else if (currentNeedSkill == npcNeedSkill.blood && playerController.onSkillB)
        {
            playerController.onSkillB = false;
            return true;
        }
        else if (currentNeedSkill == npcNeedSkill.pray && playerController.onSkillN)
        {
            playerController.onSkillN = false;
            return true;
        }
        return false;
    }

    public void OnTriggerEnter(Collider npcCollider)
    {

        if (npcCollider.gameObject.tag == "Player")
        {
            nowInteracting = true;
            if (playerController.pressPanel.activeSelf)
            {
                //Debug.Log("����!");
                return;
            }

            //Debug.Log(playerController.activeInteract);
            playerController.activeInteract = false;
            playerController.pressPanel.SetActive(true);
            //Debug.Log("����ִ�!");

            ////�ùٸ� ��ų�� �۵��ϰ� �ִ��� Ȯ���ϴ� ��� ����ϱ�
            //npcNeedSkill npcNeedSkills;
            //npcNeedSkills = npcNeedSkill.medicine;

            //switch (npcNeedSkills)
            //{
            //    case npcNeedSkill.medicine:
            //        break;

            //    case npcNeedSkill.blood:
            //        break;
            //    case npcNeedSkill.pray:
            //        break;
            //}

        }

    }

    public void OnTriggerExit(Collider npcCollider)
    {
        if (npcCollider.gameObject.tag == "Player")
        {
            nowInteracting = false;

            playerController.pressPanel.SetActive(false);
            playerController.popupPanel.SetActive(false);

            //Debug.Log("�����");

            if (!playerController.pressPanel.activeSelf) //��ȣ�ۿ���·� ����� ��츦 ���
            {
                playerController.OffInteractive();    //�÷��̾� ��Ʈ�ѷ� Ŭ������ �ִ� �޼��� ���
                //playerController.activeInteract = false;    //�÷��̾� ��Ʈ�ѷ� Ŭ������ �ִ� boolŸ�� ����
                //Debug.Log("��ȣ�ۿ���� ��Ȱ��ȭ");
            }

        }

    }
}
