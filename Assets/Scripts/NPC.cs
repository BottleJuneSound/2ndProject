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
        //    Debug.Log("æ»¥Í∞Ì¿÷¥Ÿ");
        //}
        //if (!gameObject.CompareTag("Player"))
        //{
        //    pressPanel.SetActive(false);
        //    Debug.Log("æ»¥Í∞Ì¿÷¥Ÿ");
        //}
        //if (gameObject.CompareTag("Player"))
        //{
        //    pressPanel.SetActive(true);
        //    Debug.Log("¥Í∞Ì¿÷¥Ÿ!");
        //}

    }

    public void OnTriggerEnter(Collider npcCollider)
    {
        if (npcCollider.gameObject.tag == "Player")
        {
            pressPanel.SetActive(true);
            Debug.Log("¥Í∞Ì¿÷¥Ÿ!");

        }

    }

    public void OnTriggerExit(Collider npcCollider)
    {
        if (npcCollider.gameObject.tag == "Player")
        {
            pressPanel.SetActive(false);
            Debug.Log("π˛æÓ≥µ¥Ÿ");

        }

    }
}
