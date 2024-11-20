using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float walkinSpeed = 7;
    public float runSpeed = 10;
    public Transform cameraTranceform;

    float horizontalAngle;
    float verticalAngle;

    InputAction moveAction;
    InputAction lookAction;

    PlayerController playerController;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        // 나중에 커서이미지로 바꿔보자. 시점 변환을 위한 마우스이기 때문에 없어도?
        Cursor.visible = true;

        //actions는 InputActionAsset타입 이다.
        //인풋시스템의 동작을 관리하는 변수명임
        InputActionAsset inputActions = GetComponent<PlayerInput>().actions;

        moveAction = inputActions.FindAction("Move");
        lookAction = inputActions.FindAction("Look");

        playerController = GetComponent<PlayerController>();

        horizontalAngle = transform.localEulerAngles.y;
        verticalAngle = 0;


    }

    void Update()
    {
        
    }
}
