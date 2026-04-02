using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody2D))]
public class GrappleController : MonoBehaviour
{
    public enum State
    {
        Idle,
        RopeFlying,
        Pulling,
        Returning
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

    [SerializeField] private float minPullMultiplier = 0.6f;
    [SerializeField] private float maxPullMultiplier = 1.4f;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float maxTiltAngle = 35f;

    [Header("Return")]
    [SerializeField] private float returnSmoothTime = 0.2f;
    [SerializeField] private float returnMaxSpeed = 20f;

    [Header("Tension")]
    [SerializeField] private float dangerousRopeLength = 20f;
    [SerializeField] private float breakDelay = 0.25f;

    [Header("Perfect Dock")]
    [SerializeField] private float perfectAngleThreshold = 20f;

    [Header("Input")]
    [SerializeField] private float inputBufferTime = 0.2f;

    private Rigidbody2D rb;
    private Camera cam;

    private State state = State.Idle;
    private Transform target;

    private Vector2 ropeTipPosition;
    private float tension01;
    private float grappleStartY;

    private float returnVelocityY;
    private float cooldownTimer;
    private float breakTimer;
    private float currentAngle;

    private float lastAngleAtDock;
    private float anglePullMultiplier;

    private bool bufferedInput;
    private float bufferTimer;

    private Transform lastDockedTarget;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    void Update()
    {
        HandleInput();
        UpdateBuffer();
    }

    void FixedUpdate()
    {
        TryConsumeBufferedInput();

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
        }
    }

    // ================= INPUT =================

    void HandleInput()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        // Нажатие на UI кнопку
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        bufferedInput = true;
        bufferTimer = inputBufferTime;
    }

    void UpdateBuffer()
    {
        if (!bufferedInput)
            return;

        bufferTimer -= Time.deltaTime;

        if (bufferTimer <= 0f)
        {
            bufferedInput = false;
        }
    }

    void TryConsumeBufferedInput()
    {
        if (!bufferedInput)
            return;

        if (!CanStartGrapple())
            return;

        if (TryFindTarget(out Transform foundTarget))
        {
            bufferedInput = false;
            StartGrapple(foundTarget);
        }
    }

    // ================= ROPE =================

    void UpdateRopeFlying()
    {
        if (target == null)
        {
            state = State.Idle;
        }
        ropeTipPosition = Vector2.MoveTowards(
            ropeTipPosition,
            target.position,
            ropeFlySpeed * Time.fixedDeltaTime
        );

        UpdateTension();

        if (Vector2.Distance(ropeTipPosition, target.position) <= 0.05f)
            StartPull();
    }

    bool TryFindTarget(out Transform foundTarget)
    {
        foundTarget = null;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.forward, Vector3.zero);

        if (!plane.Raycast(ray, out float enter))
            return false;

        Vector3 hitPoint = ray.GetPoint(enter);
        Vector2 tapWorld = new(hitPoint.x, hitPoint.y);

        Vector2 inputDir = tapWorld - (Vector2)playerAnchor.position;

        if (inputDir.x <= 0f)
            return false;

        float maxAssistRadius = maxDistance;

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            playerAnchor.position,
            maxAssistRadius,
            dockLayer
        );

        Transform best = null;
        float bestScore = -999f;

        foreach (var h in hits)
        {
            Transform t = h.transform;

            if (t == lastDockedTarget)
                continue;

            Vector2 toTarget = (Vector2)t.position - (Vector2)playerAnchor.position;
            float distance = toTarget.magnitude;

            Vector2 dir = toTarget.normalized;

            float dot = Vector2.Dot(inputDir.normalized, dir);

            if (dot < 0.5f) // угол допуска
                continue;

            float distanceScore = 1f - (distance / maxAssistRadius);

            float score = dot * 0.7f + distanceScore * 0.3f;

            if (score > bestScore)
            {
                bestScore = score;
                best = t;
            }
        }

        foundTarget = best;
        return best != null;
    }

    void StartGrapple(Transform newTarget)
    {
        target = newTarget;
        ropeTipPosition = playerAnchor.position;
        tension01 = 0f;
        grappleStartY = rb.position.y;

        state = State.RopeFlying;
    }

    // ================= PULL =================

    void StartPull()
    {
        state = State.Pulling;

        Vector2 toTarget = (Vector2)target.position - rb.position;
        Vector2 dir = toTarget.normalized;

        float angle = Vector2.Angle(Vector2.right, dir);
        lastAngleAtDock = angle;

        float normalized = Mathf.Clamp01(angle / 90f);

        float slowdownFactor = 1f - normalized * normalized;

        float newX = rb.linearVelocity.x * slowdownFactor;

        rb.isKinematic = true;

        rb.linearVelocity = new Vector2(newX, rb.linearVelocity.y);

        anglePullMultiplier = Mathf.Lerp(
            maxPullMultiplier,
            minPullMultiplier,
            normalized
        );

        if (playerMovement != null)
            playerMovement.enabled = false;
    }

    void UpdatePulling()
    {
        Vector2 current = rb.position;
        Vector2 end = target.position;

        Vector2 toTarget = end - current;
        float distance = toTarget.magnitude;
        Vector2 dir = toTarget.normalized;

        float accel = pullAcceleration * anglePullMultiplier;

        rb.linearVelocity += dir * accel * Time.fixedDeltaTime;

        float maxSpeed = maxPullSpeed * anglePullMultiplier;

        if (rb.linearVelocity.magnitude > maxSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;

        UpdateRotation(dir);

        float dot = Vector2.Dot(rb.linearVelocity.normalized, toTarget.normalized);

        if (distance <= reachThreshold || dot < 0.1f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.position = new Vector2(rb.position.x, end.y);

            OnDocked();
        }
    }

   void OnDocked()
    {
        lastDockedTarget = target;

        bool isPerfect = lastAngleAtDock <= perfectAngleThreshold;

        float dockY = target.position.y;

        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        // ⭐ фиксируем позицию
        rb.position = new Vector2(rb.position.x, dockY);
        transform.position = new Vector2(transform.position.x, dockY);

        Dock dock = target.GetComponent<Dock>();
        dock?.OnDocked(this, isPerfect);

        EndPull(dockY);
    }

    void EndPull(float dockY)
    {
        // задаём правильную скорость
        rb.linearVelocity = new Vector2(
            GameManager.Instance.CurrentSpeed,
            0f
        );

        // ⭐ ещё раз фиксируем Y
        rb.position = new Vector2(rb.position.x, dockY);
        transform.position = new Vector2(transform.position.x, dockY);

        if (playerMovement != null)
            playerMovement.enabled = true;

        rb.simulated = true;
        target = null;
        state = State.Idle;
    }

    // ================= ROTATION =================

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

    // ================= TENSION =================

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
                BreakRope();
        }
        else
            breakTimer = 0f;
    }

    void BreakRope()
    {
        target = null;
        tension01 = 0f;
        breakTimer = 0f;

        rb.linearVelocity = Vector2.zero;

        if (playerMovement != null)
            playerMovement.enabled = true;

        state = State.Returning;
    }

    // ================= RETURN =================

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
            FinishReturning();
    }

    void FinishReturning()
    {
        rb.position = new Vector2(rb.position.x, grappleStartY);
        returnVelocityY = 0f;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

        state = State.Idle;
        target = null;
    }

    bool CanStartGrapple()
    {
        return state == State.Idle;
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