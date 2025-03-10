using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Animator anim;

    [Header("Move")]
    public float moveSpeed;
    public float runSpeed;
    public float jumpForce;
    public float runStaminaPer;
    public bool isRun;
    public LayerMask groundLayerMask;
    public Vector2 moveDir;

    public Vector2 mousePos;

    [Header("Look")]
    public Transform mainCam;
    public float minRotX;
    public float maxRotX;
    public float mouseSensitive;
    public float cameraDistance;
    public bool isLockCursor = false;
    public bool pointOfView;
    float camRotx;
    [SerializeField] Vector2 mouseDelta;

    // InputAction
    [Header("InputAction")]
    [SerializeField] InputActionAsset inputAsset;
    InputActionMap inputPlayer;
    InputAction move;
    InputAction jump;
    InputAction look;
    InputAction run;
    InputAction interaction;
    InputAction attack;
    InputAction inventory;
    InputAction viewPoint;
    [HideInInspector] public event Action actionInteract;
    [HideInInspector] public event Action actionAttack;
    [HideInInspector] public event Action actionInventory;

    Rigidbody rigid;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();

        ToggleCursor();
        InitInputAction();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        // 마우스커서가 잠겨있다면 카메라 움직임
        if (isLockCursor)
            CameraLook();

        mousePos = Input.mousePosition;
    }

    void InitInputAction()
    {
        inputPlayer = inputAsset.FindActionMap("Player");

        move = inputPlayer.FindAction("Move");
        move.performed += OnMove;
        move.canceled += OnMove;

        jump = inputPlayer.FindAction("Jump");
        jump.started += OnJump;

        look = inputPlayer.FindAction("Look");
        look.performed += onLook;
        look.canceled += onLook;

        run = inputPlayer.FindAction("Run");
        run.performed += OnRun;
        run.canceled += OnRun;

        interaction = inputPlayer.FindAction("Interaction");
        interaction.started += OnInteract;

        attack = inputPlayer.FindAction("Attack");
        attack.performed += OnAttack;

        inventory = inputPlayer.FindAction("Inventory");
        inventory.started += OnInventory;

        viewPoint = inputPlayer.FindAction("ViewPoint");
        viewPoint.started += OnChangeViewPoint;

        inputPlayer.Enable();
    }

    void Move()
    {
        // 달리는 상태일 때 스테미나 감소
        if (isRun)
        {
            isRun = GameManager.Instance.player.condition.UseStamina(runStaminaPer * Time.fixedDeltaTime);
        }

        Vector3 dir = (transform.forward * moveDir.y) + (transform.right * moveDir.x);
        dir *= isRun ? runSpeed : moveSpeed;
        dir.y = rigid.velocity.y;

        rigid.velocity = dir;

        // 애니메이션 설정
        anim.SetBool("Move", dir.magnitude > 0.1f);
        anim.SetBool("Run", dir.magnitude > 0.1f ? isRun : false);
    }

    void OnMove(InputAction.CallbackContext context)
    {
        moveDir = context.ReadValue<Vector2>();
    }
    
    void OnRun(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isRun = true;
        }
        else
        {
            isRun = false;
        }
    }

    void OnJump(InputAction.CallbackContext context)
    {
        if (IsGround())
        {
            rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            anim.SetTrigger("Jump");
        }
    }

    bool IsGround()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
       
        if (Physics.Raycast(ray, 0.2f, groundLayerMask))
        {
             return true;
        }

        return false;
    }

    void CameraLook()
    {
        camRotx += mouseDelta.y * mouseSensitive;
        camRotx = Mathf.Clamp(camRotx, minRotX, maxRotX);
        mainCam.localEulerAngles = new Vector3(-camRotx, 0f, 0f);

        transform.eulerAngles += new Vector3(0f, mouseDelta.x * mouseSensitive, 0f);
    }

    void onLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void Bounce(float bounseForce)
    {
        rigid.AddForce(Vector3.up * bounseForce, ForceMode.Impulse);
    }

    void OnInteract(InputAction.CallbackContext context)
    {
        actionInteract?.Invoke();
    }

    void OnInventory(InputAction.CallbackContext context)
    {
        actionInventory?.Invoke();
        ToggleCursor();
    }

    void ToggleCursor()
    {
        Cursor.lockState = isLockCursor ? CursorLockMode.None : CursorLockMode.Locked;
        isLockCursor = !isLockCursor;
    }

    void OnAttack(InputAction.CallbackContext context)
    {
        actionAttack?.Invoke();
    }

    void OnChangeViewPoint(InputAction.CallbackContext context)
    {
        pointOfView = !pointOfView;

        Camera cam = Camera.main;
        cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y, pointOfView ? 0f : -cameraDistance);
    }
}
