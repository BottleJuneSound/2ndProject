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
            player.interCurrentText = "[F]�� ���� \n�������� ���� ������ Ȯ���ϼ���.";
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

            if (!player.pressPanel.activeSelf) //��ȣ�ۿ���·� ����� ��츦 ���
            {
                player.OffInteractive();
            }

        }
    }
}
