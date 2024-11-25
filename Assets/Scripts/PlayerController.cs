using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float walkingSpeed = 2;
    public float mouseSens = 0.1f;
    public float mouseScroll = 0.3f;
    public Transform cameraTranceform;
    public StateMachine stateMachine;
    public Transform cameraObject;
    public Transform playerObject;
    public Transform playerAngle;
    public GameObject pressPanel;

    float horizontalAngle;
    float verticalAngle;
    float cameraDeep = 60;

    InputAction moveAction;
    InputAction lookAction;
    InputAction cameraAction;
    InputAction runAction;
    InputAction skillMAction;
    InputAction skillNAction;
    InputAction skillBAction;


    CharacterController characterController;    //�̰� ����Ƽ �����. �������� ��ũ��Ʈ�ƴ� ��

    private void Awake()
    {
        stateMachine = new StateMachine(this);  //������ null���� �߻�

    }

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
        cameraAction = inputActions.FindAction("Scroll");
        runAction = inputActions.FindAction("Sprint");
        skillMAction = inputActions.FindAction("SkillM");
        skillNAction = inputActions.FindAction("SkillN");
        skillBAction = inputActions.FindAction("SkillB");




        characterController = GetComponent<CharacterController>();
        stateMachine.Initialize(stateMachine.idleState);

        horizontalAngle = transform.localEulerAngles.y;
        verticalAngle = 0;
        cameraDeep = cameraObject.localPosition.z;

    }

    void Update()
    {
        

        Vector2 moveVector = moveAction.ReadValue<Vector2>();

        //3D���� y�� Z�� �ݿ��ϴ� ���� �ڿ�������. xyz ��ġ �ٲ��ִ� ����
        Vector3 move = new Vector3(moveVector.x, 0, moveVector.y);


        if (moveVector.magnitude > 1)
        {
            move.Normalize();
        }

        float currentSpeed = walkingSpeed;
        if (runAction.IsPressed())
        {
            currentSpeed = walkingSpeed * 3;
        }

        move = move * currentSpeed * Time.deltaTime;
        move = transform.TransformDirection(move);  //������ ������ �ٶ󺸴� �������� �ʱ�ȭ


        // A�� D Ű�� ȸ�� ó�� (����: -90, ������: +90)
        if (moveVector.x < 0)  // A Ű�� ������ ��
        {
            playerAngle.Rotate(0, -90 * 2 * Time.deltaTime, 0);  // -90 ȸ�� (����)
        }
        else if (moveVector.x > 0)  // D Ű�� ������ ��
        {
            playerAngle.Rotate(0, 90 * 2 * Time.deltaTime, 0);  // +90 ȸ�� (������)
        }


        if (!moveAction.IsPressed())
        {
            OnIdle();
        }
        else if (moveAction.IsPressed() && !runAction.IsPressed())
        {
            OnWalk();
        }
        else if (moveAction.IsPressed() && runAction.IsPressed())
        {
            OnRun();
        }


        stateMachine.Execute();
        characterController.Move(move);

        if (skillMAction.IsPressed() && pressPanel.activeSelf)
        {
            OnMedicine();
        }

        if (skillNAction.IsPressed() && pressPanel.activeSelf)
        {
            OnPray();
        }

        if (skillBAction.IsPressed() && pressPanel.activeSelf)
        {
            OnBloodWithdrawal();
        }



        //���콺 �¿� ������
        Vector2 look = lookAction.ReadValue<Vector2>();

        float turnPlayer = look.x * mouseSens;
        horizontalAngle += turnPlayer;
        //horizontalAngle = Mathf.Clamp(horizontalAngle, -70f, 70f);

        if (horizontalAngle >= 360) horizontalAngle -= 360;
        if (horizontalAngle < 0) horizontalAngle += 360;

        Vector3 currentAngle = playerObject.localEulerAngles;
        currentAngle.y = horizontalAngle;
        playerObject.localEulerAngles = currentAngle;

        //���콺 ���� ������
        float turnCam = look.y * mouseSens;
        verticalAngle -= turnCam;
        verticalAngle = Mathf.Clamp(verticalAngle, -80f, 80f);

        currentAngle = cameraObject.localEulerAngles;
        currentAngle.x = verticalAngle;
        cameraObject.localEulerAngles = currentAngle;


        //���콺 �� ������
        Vector2 lookDeep = cameraAction.ReadValue<Vector2>();
        float zoom = lookDeep.y * mouseScroll;
        cameraDeep += zoom;
        cameraDeep = Mathf.Clamp(cameraDeep, -40f, -0.7f);

        Vector3 currentDeep = cameraObject.localPosition;
        currentDeep.z = cameraDeep;
        cameraObject.localPosition = currentDeep;

    }

    public void OnIdle()
    {
        stateMachine.TransitionTo(stateMachine.idleState);
    }
    public void OnWalk()
    {
        stateMachine.TransitionTo(stateMachine.walkState);

    }
    public void OnRun()
    {
        stateMachine.TransitionTo(stateMachine.runState);
    }

    public void OnBloodWithdrawal()
    {
        Debug.Log("����ġ�� ����!");

    }

    public void OnMedicine()
    {
        Debug.Log("�๰ġ�� ����!");
    }

    public void OnPray()
    {
        Debug.Log("�⵵ġ�� ����!");

    }


}
