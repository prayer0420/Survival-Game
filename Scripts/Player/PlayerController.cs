using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed;
    private Vector2 curMovementInput;
    public float jumpForce;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform camContainer;
    public float minXLok;
    public float maxXLok;
    private float camCurXRot;
    public float lookSensitvity;

    private Vector2 mouseDelta;

    private Rigidbody rb;

    private bool canLook = true;

    private bool isMoving = false;

    


    private void Awake()
    {
        rb = GetComponent<Rigidbody>(); 
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        if (curMovementInput.magnitude > 0 && IsGround())
        {
            if (!isMoving)
            {
                isMoving = true;
                StartCoroutine(PlayFootstepSound());
            }
        }
        else
        {
            isMoving = false;
        }
    }

    private IEnumerator PlayFootstepSound()
    {
        while (isMoving)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.footstepClip);
            yield return new WaitForSeconds(0.5f); 
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        if(canLook)
        {
            Look();
        }
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started && IsGround()) 
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode.Impulse); 
        }
    }   
    
    public void OnLookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }
    
    public void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= (moveSpeed + PlayerManager.Instance.Player.buffManager.SpeedUp);
        dir.y = rb.velocity.y;
        rb.velocity = dir;  
    }

    public void Look()
    {
        camCurXRot += mouseDelta.y * lookSensitvity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLok, maxXLok);
        camContainer.localEulerAngles = new Vector3(-camCurXRot,0,0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitvity, 0);
    }

    bool IsGround()
    {
        Ray[]rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        for(int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.5f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }

    public void ToggleCursor(bool value)
    {
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !value;
    }
}
