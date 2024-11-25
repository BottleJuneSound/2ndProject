using UnityEngine;

public class NPC : MonoBehaviour
{
    public GameObject pressPanel;

    void Start()
    {
        pressPanel.SetActive(false);
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

        }

    }
}
