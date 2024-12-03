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
            //이렇게 구지 때려 넣어야지 들어간다고?
            playerController = GetComponent<PlayerController>();
            //Debug.Log("아직도 null이라고?");
        }
        //playerController.activeInteract = false;
        //Debug.Log(playerController.activeInteract);

        // NPC가 원하는 상호작용을 랜덤으로 설정(예시)
        currentNeedSkill = (NpcNeedSkill)Random.Range(0, 3);
        Debug.Log($"NPC 상호작용 정답: {gameObject.name} + {currentNeedSkill}");

    }

    void Update()
    {
        if (nowInteracting && playerController.interactiveAction.IsPressed() && playerController.pressPanel.activeSelf)
        {
            playerController.popupPanel.SetActive(true);
            //Debug.Log("작동확인 " + playerController.activeInteract);
        }

        if (nowInteracting && !hasUsedSkill && 
            (playerController.skillMAction.IsPressed() ||
            playerController.skillNAction.IsPressed() ||
            playerController.skillBAction.IsPressed()) &&
            playerController.popupPanel.activeSelf)
        {
            //Debug.Log(CheckSkillMatch());
            bool isSkillMatch = CheckSkillMatch();  //메서드를 변수로 변환하여 중복 호출을 해결함.
            Debug.Log(isSkillMatch);
            hasUsedSkill = true;    //이것도 있어야 중복호출 방지할 수 있음

            // NPC가 원하는 스킬이 맞는지 확인
            if (isSkillMatch)
            {
                Debug.Log($"정답 입니다. NPC: {gameObject.name}");
                giftItemSkill = (ItemSkill)Random.Range(0, 2);

                if(giftItemSkill == ItemSkill.light)
                {
                    itemManager.GetLightItem();
                }
                if (giftItemSkill == ItemSkill.matche)
                {
                    itemManager.GetMatcheItem();
                }
                if (giftItemSkill == ItemSkill.potion)
                {
                    itemManager.GetPotionItem();
                }
                else
                {
                    Debug.Log("리턴 할 일이 있나?");   // 정답을 맞추면 일로 오네???
                    return;
                }

                //hasUsedSkill = true;
                Invoke("SetQuiz", 3f);

                //return;

            }
            else if(!isSkillMatch)
            {
                Debug.Log($"잘못된 상호작용입니다. NPC: {gameObject.name}");
                //hasUsedSkill = true;
                Invoke("SetQuiz", 3f);
                //return;
            }

           
        }

        //if (gameObject.tag != "Player")
        //{
        //    pressPanel.SetActive(false);
        //    Debug.Log("안닿고있다");
        //}
        //if (!gameObject.CompareTag("Player"))
        //{
        //    pressPanel.SetActive(false);
        //    Debug.Log("안닿고있다");
        //}
        //if (gameObject.CompareTag("Player"))
        //{
        //    pressPanel.SetActive(true);
        //    Debug.Log("닿고있다!");
        //}

    }

    // 현재 NPC가 요구하는 스킬이 플레이어의 스킬과 일치하는지 확인
    private bool CheckSkillMatch()
    {
        //Debug.Log(playerController.onSkillM);
        if (currentNeedSkill == NpcNeedSkill.medicine && playerController.skillMAction.IsPressed())
        {
            //playerController.onSkillM = false;
            Debug.Log("11");
            return true;
        }
        if (currentNeedSkill == NpcNeedSkill.blood && playerController.skillBAction.IsPressed())
        {
            //playerController.onSkillB = false;
            Debug.Log("22");
            return true;
        }
        if (currentNeedSkill == NpcNeedSkill.pray && playerController.skillNAction.IsPressed())
        {
            //playerController.onSkillN = false;
            Debug.Log("33");
            return true;
        } 
        Debug.Log("00");
        return false;
    }

    public void SetQuiz()
    {
        hasUsedSkill = false;
    }

    public void OnTriggerEnter(Collider npcCollider)
    {

        if (npcCollider.gameObject.tag == "Player")
        {
            nowInteracting = true;
            if (playerController.pressPanel.activeSelf)
            {
                //Debug.Log("리턴!");
                return;
            }

            //Debug.Log(playerController.activeInteract);
            playerController.activeInteract = false;
            playerController.pressPanel.SetActive(true);
            //Debug.Log("닿고있다!");

            ////올바른 스킬이 작동하고 있는지 확인하는 방법 고안하기
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
            // 스킬 사용 여부 리셋
            hasUsedSkill = false; // 상호작용이 끝났을 때 스킬 사용 여부를 초기화

            //Debug.Log("벗어났다");

            if (!playerController.pressPanel.activeSelf) //상호작용상태로 벗어났을 경우를 대비
            {
                playerController.OffInteractive();    //플레이어 컨트롤러 클래스에 있는 메서드 사용
                //playerController.activeInteract = false;    //플레이어 컨트롤러 클래스에 있는 bool타입 변수
                //Debug.Log("상호작용상태 비활성화");
            }

        }

    }
}
