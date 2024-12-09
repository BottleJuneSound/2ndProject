using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    public InputAction spaceBarStartAction;
    public bool isStartScene;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
 
        isStartScene = true;

        //�� �κ� �� �������. �� ������Ʈ�� �÷��̾� ��ǲ�� �����;��Ѵ�.
        InputActionAsset inputActions = GetComponent<PlayerInput>().actions;

        spaceBarStartAction = inputActions.FindAction("SpaceBarStart");
        //spaceBarStartAction.Enable();

    }

    // Update is called once per frame
    void Update()
    {
        if (spaceBarStartAction.WasPressedThisFrame() && isStartScene)
        {
            isStartScene = false;
            SceneManager.LoadScene("GameScene");
        }
    }
}
