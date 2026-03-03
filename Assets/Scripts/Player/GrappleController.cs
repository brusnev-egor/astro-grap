using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GrappleController : MonoBehaviour
{
    public enum State
    {
        Idle,
        RopeFlying,
        Pulling,
        Returning,
        Cooldown
    }

    [Header("References")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Transform playerAnchor;
    [SerializeField] private Transform playerVisual;

    [Header("Raycast")]
    [SerializeField] private float maxDistance = 40f;
    [SerializeField] private LayerMask dockLayer;

    [Header("Rope Flying")]
    [SerializeField] private float ropeFlySpeed = 30f;

    [Header("Pulling")]
    [SerializeField] private float pullAcceleration = 60f;
    [SerializeField] private float maxPullSpeed = 25f;
    [SerializeField] private float reachThreshold = 0.05f;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float maxTiltAngle = 35f;

    [Header("Return")]
    [SerializeField] private float returnSmoothTime = 0.2f;
    [SerializeField] private float returnMaxSpeed = 20f;

    [Header("Tension")]
    [SerializeField] private float dangerousRopeLength = 20f;
    [SerializeField] private float breakDelay = 0.25f;

    [Header("Cooldown")]
    [SerializeField] private float cooldownTime = 0.15f;

    [Header("Perfect Dock")]
    [SerializeField] private float perfectThreshold = 0.85f;

    private Rigidbody2D rb;
    private Camera cam;

    private State state = State.Idle;
    private Transform target;

    private Vector2 ropeTipPosition;
    private float tension01;
    private float grappleStartY;

    private float returnVelocityY;
    private float currentPullSpeed;
    private float cooldownTimer;
    private float breakTimer;
    private float currentAngle;
    private float lastTensionAtDock;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    void Update()
    {
        if (state == State.Idle)
            HandleInput();
    }

    void FixedUpdate()
    {
        switch (state)
        {
            case State.RopeFlying:
                UpdateRopeFlying();
                break;

            case State.Pulling:
                UpdatePulling();
                break;

            case State.Returning:
                UpdateReturning();
                ReturnToNeutralRotation();
                break;

            case State.Idle:
                ReturnToNeutralRotation();
                break;

            case State.Cooldown:
                UpdateCooldown();
                ReturnToNeutralRotation();
                break;
        }
    }

    // ======================
    // INPUT
    // ======================

    void HandleInput()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.forward, Vector3.zero);

        if (!plane.Raycast(ray, out float enter))
            return;

        Vector3 hitPoint = ray.GetPoint(enter);
        Vector2 tapWorld = new Vector2(hitPoint.x, hitPoint.y);
        Vector2 dir = tapWorld - (Vector2)playerAnchor.position;

        if (dir.x <= 0f)
            return;

        float rayDistance = dir.magnitude;

        RaycastHit2D hit = Physics2D.Raycast(
            playerAnchor.position,
            dir.normalized,
            rayDistance,
            dockLayer
        );

        if (!hit)
            return;

        target = hit.transform;
        ropeTipPosition = playerAnchor.position;
        tension01 = 0f;
        grappleStartY = rb.position.y;

        state = State.RopeFlying;
    }

    // ======================
    // ROPE FLYING
    // ======================

    void UpdateRopeFlying()
    {
        if (target == null)
        {
            EnterCooldown();
            return;
        }

        ropeTipPosition = Vector2.MoveTowards(
            ropeTipPosition,
            target.position,
            ropeFlySpeed * Time.fixedDeltaTime
        );

        UpdateTension();

        if (Vector2.Distance(ropeTipPosition, target.position) <= 0.05f)
        {
            StartPull();
        }
    }

    // ======================
    // PULLING
    // ======================

    void StartPull()
    {
        state = State.Pulling;

        lastTensionAtDock = tension01;
        currentPullSpeed = GameManager.Instance.CurrentSpeed + maxPullSpeed * 0.5f;

        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true;

        if (playerMovement != null)
            playerMovement.enabled = false;
    }

    void UpdatePulling()
    {
        if (target == null)
        {
            EndPull();
            return;
        }

        Vector2 current = rb.position;
        Vector2 end = target.position;

        Vector2 toTarget = end - current;
        float distance = toTarget.magnitude;
        Vector2 dir = toTarget.normalized;

        // ускорение
        rb.linearVelocity += dir * pullAcceleration * Time.fixedDeltaTime;

        if (rb.linearVelocity.magnitude > maxPullSpeed)
        {
            rb.linearVelocity =
                rb.linearVelocity.normalized * maxPullSpeed;
        }

        UpdateRotation(dir);

        // ⭐ КЛЮЧЕВОЕ
        // если мы уже движемся в сторону, противоположную target,
        // значит пролетели станцию
        float dot = Vector2.Dot(rb.linearVelocity.normalized, toTarget.normalized);

        if (distance <= reachThreshold || dot < 0f)
        {
            rb.position = end;
            rb.linearVelocity = Vector2.zero;
            OnDocked();
        }
    }

    void OnDocked()
    {
        bool isPerfect = lastTensionAtDock >= perfectThreshold;

        rb.MovePosition(new Vector2(rb.position.x, target.position.y));

        Dock dock = target.GetComponent<Dock>();
        dock?.OnDocked(this, isPerfect);

        EndPull();
    }

    void EndPull()
    {
        rb.isKinematic = false;
        // rb.linearVelocity = Vector2.zero;

        if (playerMovement != null)
            playerMovement.enabled = true;

        EnterCooldown();
    }

    // ======================
    // ROTATION
    // ======================

    void UpdateRotation(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.001f)
            return;

        float targetAngle =
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        targetAngle = Mathf.Clamp(targetAngle, -maxTiltAngle, maxTiltAngle);

        currentAngle = Mathf.Lerp(
            currentAngle,
            targetAngle,
            Time.fixedDeltaTime * rotationSpeed
        );

        playerVisual.localRotation =
            Quaternion.Euler(0f, 0f, currentAngle);
    }

    void ReturnToNeutralRotation()
    {
        currentAngle = Mathf.Lerp(
            currentAngle,
            0f,
            Time.fixedDeltaTime * rotationSpeed
        );

        playerVisual.localRotation =
            Quaternion.Euler(0f, 0f, currentAngle);
    }

    // ======================
    // TENSION
    // ======================

    void UpdateTension()
    {
        float ropeLength =
            Vector2.Distance(playerAnchor.position, ropeTipPosition);

        tension01 = Mathf.Clamp01(
            ropeLength / dangerousRopeLength
        );

        if (tension01 >= 1f)
        {
            breakTimer += Time.fixedDeltaTime;

            if (breakTimer >= breakDelay)
            {
                BreakRope();
            }
        }
        else
        {
            breakTimer = 0f;
        }
    }

    void BreakRope()
    {
        target = null;
        tension01 = 0f;
        breakTimer = 0f;

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = Vector2.zero;

        if (playerMovement != null)
            playerMovement.enabled = true;

        state = State.Returning;
    }

    // ======================
    // RETURN
    // ======================

    void UpdateReturning()
    {
        float newY = Mathf.SmoothDamp(
            rb.position.y,
            grappleStartY,
            ref returnVelocityY,
            returnSmoothTime,
            returnMaxSpeed,
            Time.fixedDeltaTime
        );

        float yVelocity = (newY - rb.position.y) / Time.fixedDeltaTime;

        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            yVelocity
        );

        if (Mathf.Abs(rb.position.y - grappleStartY) < 0.05f)
        {
            FinishReturning();
        }
    }

    void FinishReturning()
    {
        rb.position = new Vector2(rb.position.x, grappleStartY);
        returnVelocityY = 0f;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

        state = State.Cooldown;
        cooldownTimer = cooldownTime;
    }

    // ======================
    // COOLDOWN
    // ======================

    void EnterCooldown()
    {
        state = State.Cooldown;
        cooldownTimer = cooldownTime;
        target = null;
        tension01 = 0f;
    }

    void UpdateCooldown()
    {
        cooldownTimer -= Time.fixedDeltaTime;

        if (cooldownTimer <= 0f)
            state = State.Idle;
    }

    // ======================
    // VISUAL API
    // ======================

    public bool TryGetRopeVisual(
        out Vector2 end,
        out float tension
    )
    {
        end = Vector2.zero;
        tension = 0f;

        if (state == State.RopeFlying && target != null)
        {
            end = ropeTipPosition;
            tension = tension01;
            return true;
        }

        if (state == State.Pulling && target != null)
        {
            end = target.position;
            tension = tension01;
            return true;
        }

        return false;
    }
}