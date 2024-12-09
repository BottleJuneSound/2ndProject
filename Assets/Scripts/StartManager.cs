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

        //이 부분 잘 기억하자. 겟 컴포넌트로 플레이어 인풋을 가져와야한다.
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
