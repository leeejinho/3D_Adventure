using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed;
    public float runSpeed;
    private bool isRun;
    public float jumpForce;
    public LayerMask groundLayerMask;
    private Vector2 moveDir;

    [Header("Look")]
    public Transform transfromCam;
    public float minRotX;
    public float maxRotX;
    public float mouseSensitive;
    private float camRotx;
    [SerializeField] private Vector2 mouseDelta;

    // InputAction
    [Header("InputAction")]
    [SerializeField] private InputActionAsset inputAsset;
    private InputActionMap inputPlayer;
    private InputAction move;
    private InputAction jump;
    private InputAction look;
    private InputAction run;

    private Rigidbody rigid;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;

        InitInputAction();
    }

    void Update()
    {
        Move();
    }

    private void LateUpdate()
    {
        CameraLook();
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

        inputPlayer.Enable();
    }

    void Move()
    {
        Vector3 dir = (transform.forward * moveDir.y) + (transform.right * moveDir.x);
        dir *= isRun ? runSpeed : moveSpeed;
        dir.y = rigid.velocity.y;

        rigid.velocity = dir;
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
        else if (context.canceled)
        {
            isRun = false;
        }
    }

    void OnJump(InputAction.CallbackContext context)
    {
        if (IsGround())
        {
            rigid.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    bool IsGround()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
       
        if (Physics.Raycast(ray, 0.1f, groundLayerMask))
        {
             return true;
        }

        return false;
    }

    void CameraLook()
    {
        camRotx += mouseDelta.y * mouseSensitive;
        camRotx = Mathf.Clamp(camRotx, minRotX, maxRotX);
        transfromCam.localEulerAngles = new Vector3(-camRotx, 0f, 0f);

        transform.eulerAngles += new Vector3(0f, mouseDelta.x * mouseSensitive, 0f);
    }

    void onLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }
}
