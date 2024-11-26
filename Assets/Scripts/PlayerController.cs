using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    public GameObject playerObject;
    public GameObject pressPanel;
    public Transform playerAngle;
    public StateMachine stateMachine;

    public float walkingSpeed = 2;
    public float rotationSpeed = 1;
    public float terminalSpeed = 50;
    public float verticalSpeed = 0;

    InputAction moveAction;
    InputAction runAction;
    InputAction skillMAction;
    InputAction skillNAction;
    InputAction skillBAction;
    public InputAction interactiveAction;

    public bool activeInteract = false;
    public CharacterController characterController;    //�̰� ����Ƽ �����. �������� ��ũ��Ʈ�ƴ� ��

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
        runAction = inputActions.FindAction("Sprint");
        skillMAction = inputActions.FindAction("SkillM");
        skillNAction = inputActions.FindAction("SkillN");
        skillBAction = inputActions.FindAction("SkillB");
        interactiveAction = inputActions.FindAction("Interact");

        characterController = GetComponent<CharacterController>();
        stateMachine.Initialize(stateMachine.idleState);

    }

    void Update()
    {

        //3D���� y�� Z�� �ݿ��ϴ� ���� �ڿ�������. xyz ��ġ �ٲ��ִ� ����
        Vector2 moveVector = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(moveVector.x, 0, moveVector.y);

        // ī�޶��� ������ ������ (ȸ���� ���� ����)
        Transform camTransform = Camera.main.transform;
        Vector3 camForward = camTransform.forward; // ī�޶��� �� ����
        Vector3 camRight = camTransform.right;    // ī�޶��� ������ ����. x�� ������ �Ǹ� �ڿ������� ������ �ȴ�.

        // y�� ���� 0���� �����Ͽ� ī�޶� ���� �������θ� �̵��ϵ��� ����
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        // �Էµ� ������ ī�޶��� ȸ�� ���⿡ ���� ����
        Vector3 moveDir = (camRight * moveVector.x + camForward * moveVector.y).normalized;

        // �̵��� ������ ���� ��� (�̵� ���� ���) ĳ������ ȸ��
        if (moveDir != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);
        }

        if (moveVector.magnitude > 1)
        {
            move.Normalize();
        }

        float currentSpeed = walkingSpeed;
        if (runAction.IsPressed())
        {
            currentSpeed = walkingSpeed * 3;
        }

        move = moveDir * currentSpeed * Time.deltaTime;

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



        if (interactiveAction.IsPressed() && pressPanel.activeSelf)
        {
            OnInteractive();

        }

        if (activeInteract)
        {
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
        }


        ////���콺 �� ������
        //Vector2 lookDeep = cameraAction.ReadValue<Vector2>();
        //float zoom = lookDeep.y * mouseScroll;
        //cameraDeep += zoom;
        //cameraDeep = Mathf.Clamp(cameraDeep, -40f, -0.7f);

        //Vector3 currentDeep = cameraObject.localPosition;
        //currentDeep.z = cameraDeep;
        //cameraObject.localPosition = currentDeep;

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
        activeInteract = false;

    }

    public void OnMedicine()
    {
        Debug.Log("�๰ġ�� ����!");
        activeInteract = false;
    }

    public void OnPray()
    {
        Debug.Log("�⵵ġ�� ����!");
        activeInteract = false;

    }

    public void OnInteractive()
    {
        if(activeInteract == true) return;

        Debug.Log("��ȣ�ۿ��� �����մϴ�.");
        activeInteract = true;

    }

    public void OffInteractive()
    {
        if(activeInteract == false) return;

        Debug.Log("��ȣ�ۿ��� ����Ǿ����ϴ�.");
        if (interactiveAction.IsPressed()) activeInteract = false;
    }


}
