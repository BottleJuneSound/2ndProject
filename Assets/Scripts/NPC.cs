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
            //이렇게 구지 때려 넣어야지 들어간다고?
            playerController = GetComponent<PlayerController>();

            //Debug.Log("아직도 null이라고?");
        }
        //playerController.activeInteract = false;
        //Debug.Log(playerController.activeInteract);


    }

    void Update()
    {
        if (playerController.interactiveAction.IsPressed() && playerController.pressPanel.activeSelf)
        {
            playerController.popupPanel.SetActive(true);
            Debug.Log("작동확인 " + playerController.activeInteract);

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

    public void OnTriggerEnter(Collider npcCollider)
    {

        if (npcCollider.gameObject.tag == "Player")
        {
            if (playerController.pressPanel.activeSelf)
            {
                Debug.Log("리턴!");
                return;
            }

            Debug.Log(playerController.activeInteract);
            playerController.activeInteract = false;
            playerController.pressPanel.SetActive(true);
            Debug.Log("닿고있다!");



        }

    }

    public void OnTriggerExit(Collider npcCollider)
    {
        if (npcCollider.gameObject.tag == "Player")
        {
            playerController.pressPanel.SetActive(false);
            playerController.popupPanel.SetActive(false);

            Debug.Log("벗어났다");

            if(!playerController.pressPanel.activeSelf) //상호작용상태로 벗어났을 경우를 대비
            {
                playerController.OffInteractive();    //플레이어 컨트롤러 클래스에 있는 메서드 사용
                //playerController.activeInteract = false;    //플레이어 컨트롤러 클래스에 있는 bool타입 변수
                Debug.Log("상호작용상태 비활성화");
            }

        }

    }
}
