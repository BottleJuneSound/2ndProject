using Unity.VisualScripting;
using UnityEngine;

public class NPC : MonoBehaviour
{
    //public GameObject pressPanel;
    public NpcNeedSkill currentNeedSkill;
    public ItemSkill giftItemSkill;
    public bool nowInteracting = false;
    public bool hasUsedSkill = false;
    public ItemManager itemManager;
    public BossHPManager bossHPManager;

    public enum NpcNeedSkill
    {
        medicine,
        blood,
        pray
    }
    public enum ItemSkill
    {
        light,
        matche,
        potion
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
        currentNeedSkill = (NpcNeedSkill)Random.Range(0, 3);
        giftItemSkill = (ItemSkill)Random.Range(0, 2);
        Debug.Log($"NPC ��ȣ�ۿ� ����: {gameObject.name} + {currentNeedSkill}");
        Debug.Log($"NPC ��ȣ�ۿ� ����: {gameObject.name} + {giftItemSkill}");
    }

    void Update()
    {
        if (nowInteracting && playerController.interactiveAction.IsPressed() && playerController.pressPanel.activeSelf)
        {
            playerController.popupPanel.SetActive(true);
            //Debug.Log("�۵�Ȯ�� " + playerController.activeInteract);
        }

        if (nowInteracting && !hasUsedSkill && 
            (playerController.skillMAction.IsPressed() ||
            playerController.skillNAction.IsPressed() ||
            playerController.skillBAction.IsPressed()) &&
            playerController.popupPanel.activeSelf)
        {
            //Debug.Log(CheckSkillMatch());
            bool isSkillMatch = CheckSkillMatch();  //�޼��带 ������ ��ȯ�Ͽ� �ߺ� ȣ���� �ذ���.
            hasUsedSkill = true;    //�̰͵� �־�� �ߺ�ȣ�� ������ �� ����

            // NPC�� ���ϴ� ��ų�� �´��� Ȯ�� if(CheckSkillMatch()) 
            if (isSkillMatch)
            {
                Debug.Log($"���� �Դϴ�. NPC: {gameObject.name}");
                

                if(giftItemSkill == ItemSkill.light)
                {
                    itemManager.GetLightItem();
                    isSkillMatch = false;   // CheckSkillMatch() �ʱ�ȭ
                    gameObject.tag = "ClearNPC";
                    //Debug.Log(gameObject.tag);
                }
                else if (giftItemSkill == ItemSkill.matche)
                {
                    itemManager.GetMatcheItem();
                    isSkillMatch = false;
                    gameObject.tag = "ClearNPC";
                    //Debug.Log(gameObject.tag);


                }
                else if (giftItemSkill == ItemSkill.potion)
                {
                    itemManager.GetPotionItem();
                    isSkillMatch = false;
                    gameObject.tag = "ClearNPC";
                    //Debug.Log(gameObject.tag);

                }
                else
                {
                    Debug.Log("���� �� ���� �ֳ�?");   // ������ ���߸� �Ϸ� ����???
                    return;
                }

                //hasUsedSkill = true;
                Invoke("SetQuiz", 3f);
            }
            else if(!isSkillMatch)
            {
                bossHPManager.BossHPAdd(); // ���� Ÿ��Ƚ�� ������ �ʿ��� �Լ�
                Debug.Log($"�߸��� ��ȣ�ۿ��Դϴ�. NPC: {gameObject.name}");
                //hasUsedSkill = true;
                Invoke("SetQuiz", 3f);
            }

           
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
        if (currentNeedSkill == NpcNeedSkill.medicine && playerController.skillMAction.IsPressed())
        {
            //playerController.onSkillM = false;
            return true;
        }
        if (currentNeedSkill == NpcNeedSkill.blood && playerController.skillBAction.IsPressed())
        {
            //playerController.onSkillB = false;
            return true;
        }
        if (currentNeedSkill == NpcNeedSkill.pray && playerController.skillNAction.IsPressed())
        {
            //playerController.onSkillN = false;
            return true;
        } 
        return false;
    }

    public void SetQuiz()
    {
        hasUsedSkill = false;
        playerController.pressPanel.SetActive(false);
        playerController.popupPanel.SetActive(false);
        playerController.importAlarm.SetActive(false);
        playerController.OffInteractive();
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
        }
    }

    public void OnTriggerExit(Collider npcCollider)
    {
        if (npcCollider.gameObject.tag == "Player")
        {
            nowInteracting = false;

            playerController.pressPanel.SetActive(false);
            playerController.popupPanel.SetActive(false);
            playerController.importAlarm.SetActive(false);
            // ��ų ��� ���� ����
            hasUsedSkill = false; // ��ȣ�ۿ��� ������ �� ��ų ��� ���θ� �ʱ�ȭ

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
