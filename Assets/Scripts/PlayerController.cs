using TMPro;
using Unity.Burst.CompilerServices;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    public GameObject playerObject;
    public GameObject pressPanel;
    public GameObject popupPanel;
    public GameObject loadM;
    public GameObject loadN;
    public GameObject loadB;
    public GameObject buttonM;
    public GameObject buttonN;
    public GameObject buttonB;

    public TMP_Text npcText;

    public Transform playerAngle;
    public StateMachine stateMachine;

    public float walkingSpeed = 2;
    public float rotationSpeed = 1;
    public float terminalSpeed = 50;
    public float verticalSpeed = 0;

    [SerializeField]
    CinemachineInputAxisController cineCam;
    InputAction moveAction;
    InputAction runAction;
    public InputAction skillMAction;
    public InputAction skillNAction;
    public InputAction skillBAction;
    InputAction closePopupAction;
    public InputAction interactiveAction;

    public string oriText;
    public string currentText;

    public bool activeInteract = false;
    public bool onSkillM = false;
    public bool onSkillN = false;
    public bool onSkillB = false;
    //public bool onSkillFail = false;

    public bool skillActive = false;
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
        runAction = inputActions.FindAction("Sprint");
        skillMAction = inputActions.FindAction("SkillM");
        skillNAction = inputActions.FindAction("SkillN");
        skillBAction = inputActions.FindAction("SkillB");
        interactiveAction = inputActions.FindAction("Interact");
        closePopupAction = inputActions.FindAction("Exit");

        characterController = GetComponent<CharacterController>();
        stateMachine.Initialize(stateMachine.idleState);
        oriText = npcText.text;

        if (cineCam == null)
        {
            cineCam = GetComponent<CinemachineInputAxisController>();
            cineCam.enabled = true;
            Debug.Log("���� �ó׸ӽ� null�ƴ�!");
        }

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
                if(skillActive) return;
                onSkillM = true;
                OnMedicine();

            }

            if (skillNAction.IsPressed() && pressPanel.activeSelf)
            {
                if (skillActive) return;
                onSkillN = true;
                OnPray();
            }

            if (skillBAction.IsPressed() && pressPanel.activeSelf)
            {
                if (skillActive) return;
                onSkillB = true;
                OnBloodWithdrawal();
            }

            if(closePopupAction.IsPressed() && pressPanel.activeSelf)
            {
                ClosePopup();
            }

        }

        //��ȣ�ۿ� Text ���뺯���ϴ� ������Ʈ
        if (npcText.gameObject.activeSelf && !skillActive)
        {
            if (onSkillM)
            {
                //buttonM.GetComponent<Image>().color = Color.clear / Time.deltaTime;       // ���������� a�� ���� ��ų �� �ִ� ��� ã��
                skillActive = true;
                currentText = "�๰ġ�Ḧ �����Ͽ����ϴ�.";
                npcText.text = currentText;
                Invoke("ResetAllSkill", 3f);
            }
            else if (onSkillN)
            {
                skillActive = true;
                currentText = "�⵵�� �����Ͽ����ϴ�.";
                npcText.text = currentText;
                Invoke("ResetAllSkill", 3f);
            }
            else if (onSkillB)
            {
                skillActive = true;
                currentText = "����ġ�Ḧ �����Ͽ����ϴ�.";
                npcText.text = currentText;
                Invoke("ResetAllSkill", 3f);
            }
            //else if (onSkillFail)
            //{
            //    skillActive = true;
            //    currentText = "ȯ�ڰ� ȥ���������մϴ�.";
            //    npcText.text = currentText;
            //    Invoke("ResetAllSkill", 3f);
            //}
            else
            {
                npcText.text = oriText;
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

    public void ResetAllSkill() //��ų ���� �ʱ�ȭ
    {
        if (skillActive)
        {
            onSkillM = false;
            onSkillN = false;
            onSkillB = false;
            //onSkillFail = false;
            loadM.gameObject.SetActive(false);
            loadN.gameObject.SetActive(false);
            loadB.gameObject.SetActive(false);




            skillActive = false;
            npcText.text = oriText;
        }


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

    public void OnBloodWithdrawal() // ��ȣ�ۿ� ��ų ����, ��ȣ�ۿ� ���ۺκ����� ���ƿ�.
    {
        loadB.gameObject.SetActive(true);
        Debug.Log("����ġ�� ����!");
        OnInteractive();
        //activeInteract = false;

    }

    public void OnMedicine()
    {
        loadM.gameObject.SetActive(true);
        Debug.Log("�๰ġ�� ����!");
        OnInteractive();
        //activeInteract = false;
    }

    public void OnPray()
    {
        loadN.gameObject.SetActive(true);
        Debug.Log("�⵵ġ�� ����!");
        OnInteractive();
        //activeInteract = false;

    }

    public void ClosePopup()    // â�� ��Ȱ��ȭ �ϰ� ���ͷ�Ƽ�� ���� �޼��� ����
    {
        popupPanel.SetActive(false);
        OffInteractive();

    }

    public void OnInteractive()
    {
        if(activeInteract == true) return;

        Debug.Log("��ȣ�ۿ��� �����մϴ�.");
        activeInteract = true;
        cineCam.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    public void OffInteractive()
    {
        if(activeInteract == false) return;

        Debug.Log("��ȣ�ۿ��� ����Ǿ����ϴ�.");
        if (interactiveAction.IsPressed()) activeInteract = false;
        cineCam.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }


    private void OnTriggerEnter(Collider playerTrigger)
    {
        if (playerTrigger.gameObject.tag == "NPC")
        {
            npcText.gameObject.SetActive(true);

        }
    }
    private void OnTriggerExit(Collider playerTrigger)
    {
        if (playerTrigger.gameObject.tag == "NPC")
        {
            npcText.gameObject.SetActive(false);

        }
    }


}
