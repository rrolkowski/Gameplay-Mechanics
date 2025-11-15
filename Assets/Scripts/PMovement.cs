using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PMovement : MonoBehaviour
{
    private Rigidbody rb;

    private Vector2 movementInput;
    private Vector2 mousePosition;
    private Vector2 lastMousePosition;
    private Vector3 targetVelocity;

    [Header("References")]
    [SerializeField] private PStamina pStamina;
     private Statistics statistics;

    [Header("Movement/Look Settings")]
    [SerializeField] float rotationSpeed = 20f;
    [SerializeField] float baseMoveSpeed = 5f;
    [SerializeField] float moveSpeed;
    [SerializeField] float acceleration = 20f;

    [Header("Roll Settings")]
    [SerializeField] float rollSpeed = 10f;
    [SerializeField] float rollDuration = 0.6f;
    [SerializeField] float rollCooldown = 1.5f;
    public bool isRolling = false;
    private bool canRoll = true;
    public bool wasRollingTriggered = false;

    [Header("Double-Tap Roll Settings")]
    [SerializeField] private float doubleTapTime = 0.3f; // Maksymalny czas na drugi tap
    private float lastRollPressTime = -1f;

    [Header("Objects")]
    [SerializeField] Camera mainCamera;
    [SerializeField] Animator anim;
    [SerializeField] Collider col;

    [Header("Animation Smoothing")]
    [Range(0, 1f)]
    public float HorizontalAnimSmoothTime = 0.2f;
    [Range(0, 1f)]
    public float VerticalAnimTime = 0.2f;
    [Range(0, 1f)]
    public float StartAnimTime = 0.3f;
    [Range(0, 1f)]
    public float StopAnimTime = 0.15f;

    private float speed;
    private float allowPlayerRotation = 0.1f;

    private bool _isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody>() ?? throw new MissingComponentException("Rigidbody is missing");
        col = GetComponent<Collider>() ?? throw new MissingComponentException("Collider is missing");
        mainCamera = mainCamera ?? Camera.main ?? throw new MissingReferenceException("Camera is missing");
        anim = anim ?? GetComponent<Animator>() ?? throw new MissingComponentException("Animator is missing");
    }
    void Start()
    {
        statistics = GetComponent<StatisticsProvider>().GetStatistics();

        statistics.StatPointsChanged.AddListener(UpdateMovementSpeed);
    }
    private void OnDestroy()
    {
        statistics.StatPointsChanged.RemoveListener(UpdateMovementSpeed);
    }
    void Update()
    {
        if (!isRolling)
        {
            UpdateTargetVelocity();
            UpdateAnimation();
        }
    }

    void FixedUpdate()
    {
        if (!isRolling)
        {
            HandleMovement();
            HandleRotation();
        }
    }

    void UpdateTargetVelocity()
    {
        Vector3 movement = new Vector3(movementInput.x, 0, movementInput.y).normalized;
        targetVelocity = movement * moveSpeed;

        if (!_isGrounded)
        {
            rb.linearVelocity += Physics.gravity * Time.fixedDeltaTime;
        }
    }

    void UpdateMovementSpeed()
    {
        moveSpeed = baseMoveSpeed + (statistics.statPoints[BaseStatType.DEXTERITY] * 0.1f);
    }
    public float GetMoveSpeed()
    {
        return (moveSpeed / baseMoveSpeed) * 100f;
    }
    public float GetRawMoveSpeed()
    {
        return moveSpeed;
    }

    public void SetMoveSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }

    void HandleMovement()
    {
        float currentYVelocity = rb.linearVelocity.y;
        Vector3 horizontalVelocity = Vector3.Lerp(new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z), targetVelocity, acceleration * Time.fixedDeltaTime);
        rb.linearVelocity = new Vector3(horizontalVelocity.x, currentYVelocity, horizontalVelocity.z);
    }

    void HandleRotation()
    {
        if (movementInput.sqrMagnitude > 0.01f)
        {
            Vector3 movementDirection = new Vector3(movementInput.x, 0, movementInput.y);
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!isRolling)
            movementInput = context.ReadValue<Vector2>();
        else
        {
            movementInput = Vector2.zero;
        }
    }

    public void OnRoll(InputAction.CallbackContext context)
    {
        if (context.started && !isRolling)
        {
            if (Time.time - lastRollPressTime < doubleTapTime)
            {
                if(pStamina.GetCurrentStamina() >= pStamina.GetRollCost())//20)
                StartCoroutine(Roll());
                wasRollingTriggered = true;
            }
            lastRollPressTime = Time.time;
        }
    }
    IEnumerator Roll()
    {
        if (isRolling || !canRoll)
            yield break;

        isRolling = true;
        canRoll = false;

        anim.SetTrigger("Roll");

        Vector3 rollDirection = movementInput.sqrMagnitude > 0.01f ? new Vector3(movementInput.x, 0, movementInput.y).normalized : transform.forward;

        float elapsedTime = 0f;

        if (col)
        {
            col.enabled = false;
        }

        while (elapsedTime < rollDuration)
        {
            rb.linearVelocity = rollDirection * rollSpeed;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (col)
        {
            col.enabled = true;
        }

        isRolling = false;        
        movementInput = Vector3.zero;

        yield return new WaitForSeconds(rollCooldown);
        canRoll = true;
    }

    void UpdateAnimation()
    {
        speed = new Vector2(movementInput.x, movementInput.y).sqrMagnitude;

        if (speed > allowPlayerRotation)
        {
            anim.SetFloat("Blend", speed, StartAnimTime, Time.deltaTime);
        }
        else
        {
            anim.SetFloat("Blend", speed, StopAnimTime, Time.deltaTime);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = false;
        }
    }

}
