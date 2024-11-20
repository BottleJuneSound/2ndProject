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
        // ���߿� Ŀ���̹����� �ٲ㺸��. ���� ��ȯ�� ���� ���콺�̱� ������ ���?
        Cursor.visible = true;

        //actions�� InputActionAssetŸ�� �̴�.
        //��ǲ�ý����� ������ �����ϴ� ��������
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
