using UnityEngine;

public class BossZoneInfo : MonoBehaviour
{
    public PlayerController player;
    public bool nowInteracting = false;

    void Start()
    {
        player.pressPanel.SetActive(false);
        player.popupPanel.SetActive(false);

        if (player == null)
        {
            player = GetComponent<PlayerController>();
        }
    }

    void Update()
    {
        if (nowInteracting && player.interactiveAction.IsPressed() && player.pressPanel.activeSelf)
        {
            player.popupPanel.SetActive(true);
        }
    }

    public void OnTriggerEnter(Collider npcCollider)
    {

        if (npcCollider.gameObject.tag == "Player")
        {
            player.interCurrentText = "[F]를 눌러 \n보스방의 입장 조건을 확인하세요.";
            player.interText.text = player.interCurrentText;
            nowInteracting = true;
            if (player.pressPanel.activeSelf)
            {
                return;
            }
            player.activeInteract = false;
            player.pressPanel.SetActive(true);
        }
    }

    public void OnTriggerExit(Collider npcCollider)
    {
        if (npcCollider.gameObject.tag == "Player")
        {
            nowInteracting = false;

            player.pressPanel.SetActive(false);
            player.popupPanel.SetActive(false);

            if (!player.pressPanel.activeSelf) //상호작용상태로 벗어났을 경우를 대비
            {
                player.OffInteractive();
            }

        }
    }
}
