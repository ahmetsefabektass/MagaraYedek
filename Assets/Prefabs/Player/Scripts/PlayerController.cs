using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    [SerializeField] List<GameObject> wheels;
    public float wheelSpeedMultiplier = 100f;
    [SerializeField] GameObject bodyTarget;
    public Inputs inputs;
    public CharacterController characterController;
    public float moveInput;
    public Vector3 movementVector;
    Vector2 rotationInput;
    public float rotationSpeed = 1f;
    Vector3 move;
    Vector3 jump;
    public float moveSpeed = 0;
    public bool hasJumped = false;
    public float verticalVelocity = 0f;
    public bool hasLanded = false;
    public float jumpForce = 9f;
    public AnimationCurve speedCurve;
    public float speedBooster = 1f;
    public float speedBoosterDuration = 5f;

    float accelerationTime = 0f;
    float accelerationTimeMax = 1f;
    public bool HasInteracted { get; set; } = false;
    private bool CanInteract = true;
    public float interactionRange = 10f;
    public float ChargeValue { get; set; } = 100f;
    public float chargeDecreaseSpeed = 1f;
    [HideInInspector] public bool chargingByVendor = false;
    public Animator animator;
    Slider slider;
    CinemachineFreeLook freeLookCamera;
    CinemachineBasicMultiChannelPerlin topRigNoise;
    CinemachineBasicMultiChannelPerlin midRigNoise;
    CinemachineBasicMultiChannelPerlin botRigNoise;
    bool canNoise = true;
    float targetFov;
    [SerializeField] float minFov = 30f;
    [SerializeField] float maxFov = 80f;
    [SerializeField] float minFrequency = 1f;
    [SerializeField] float maxFrequency = 7f;
    
    GameObject currentPlatform;
    Quaternion lastPlatformRotation;
    Vector3 deltaPos;
    bool isDead = false;

    [SerializeField] MenuUI menu;

    private void Awake()
    {
        CanInteract = true;

        freeLookCamera = GetComponentInChildren<CinemachineFreeLook>();
        topRigNoise = freeLookCamera.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        midRigNoise = freeLookCamera.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        botRigNoise = freeLookCamera.GetRig(2).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        characterController = GetComponent<CharacterController>();
        inputs = new Inputs();
        animator = GetComponent<Animator>();
        slider = GetComponentInChildren<Slider>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Start()
    {
        if (currentPlatform != null)
            lastPlatformRotation = currentPlatform.transform.rotation;

        accelerationTimeMax = speedCurve.keys[speedCurve.length - 1].time;

        inputs.Player.Jump.started += Jump_performed;

        inputs.Player.Movement.started += ctx =>
        {
            if (HasInteracted || isDead) return;
            bodyTarget.transform.DOLocalRotate(new Vector3(-20, 0, 0), 1f);
        };
        inputs.Player.Movement.canceled += ctx =>
        {
            if (HasInteracted || isDead) return;
            bodyTarget.transform.DOLocalRotate(new Vector3(0, 0, 0), 1f);
        };
        inputs.Player.Esc.started += ctx =>
        {
            menu.ShowMenu();
        };
    }
    private  void Jump_performed(InputAction.CallbackContext obj)
    {
        if (characterController.isGrounded && !hasJumped && !HasInteracted)
        {
            CanInteract = false;
            animator.SetTrigger("Jumped");
        }
    }

    void Update()
    {
        if (!chargingByVendor)
        {
            ChargeValue = Mathf.Clamp(ChargeValue - Time.deltaTime * chargeDecreaseSpeed, 0, 100);
        }
        slider.value = ChargeValue;

        //if (ChargeValue <= 0 && !isDead)
        //{
        //  isDead = true;
        //  StartCoroutine(Die());
        //  return;
        //}
        if(IsDead())
        {
            StartCoroutine(Die());
            return;
        }

        if (isDead) return;

        CheckInteraction();
        if (HasInteracted) return;

        PlatFormMovement();
        HandleMovement();
        HandleRotation();
        Gravity();

       move = transform.TransformDirection(Vector3.forward) * moveSpeed * speedBooster;
       jump = Vector3.up * verticalVelocity;
       movementVector = move + jump;

       characterController.Move(movementVector * Time.deltaTime);
    }
    void PlatFormMovement()
    {
        if (currentPlatform != null && !characterController.isGrounded)
        {
            currentPlatform = null;
        }
        else
        {
            if (currentPlatform != null)
            {
                Quaternion currentRot = currentPlatform.transform.rotation;
                Quaternion delta = currentRot * Quaternion.Inverse(lastPlatformRotation);
                lastPlatformRotation = currentRot;
                transform.rotation = delta * transform.rotation;
                Vector3 offset = transform.position - currentPlatform.transform.position;
                Vector3 rotatedOffset = delta * offset;
                Vector3 targetPos = currentPlatform.transform.position + rotatedOffset;
                characterController.Move(targetPos - transform.position);
            }
        }
    }
    void CheckInteraction()
    {
        if (inputs.Player.Interact.triggered && !HasInteracted && CanInteract)
        {
            HasInteracted = true;
            StartCoroutine(Interaction());
        }
    }

    void Gravity()
    {
        if (!characterController.isGrounded)
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
        if (hasJumped && characterController.isGrounded && verticalVelocity < 0)
        {
            hasJumped = false;
            animator.SetBool("Landed", true);
            CanInteract = true;
        }
    }

    void HandleMovement()
    {
        moveInput = inputs.Player.Movement.ReadValue<float>();
        bool moving = Mathf.Abs(moveInput) > 0.01f;

        accelerationTime = Mathf.MoveTowards(
            accelerationTime,
            moving ? accelerationTimeMax : 0f,
            Time.deltaTime
        );

        moveSpeed = speedCurve.Evaluate(accelerationTime);

        if (move != Vector3.zero)
        {
            animator.SetFloat("idleToWalk", Mathf.Lerp(animator.GetFloat("idleToWalk"), 1, Time.deltaTime * 5f));
            for (int i = 0; i < wheels.Count; i++)
            {
                if (i == 0)
                {
                    wheels[i].transform.Rotate(-Vector3.up, moveSpeed * wheelSpeedMultiplier * Time.deltaTime);
                }
                else
                {
                    wheels[i].transform.Rotate(Vector3.up, moveSpeed * wheelSpeedMultiplier * Time.deltaTime);
                }
            }
        }
        else
        {
            animator.SetFloat("idleToWalk", Mathf.Lerp(animator.GetFloat("idleToWalk"), 0, Time.deltaTime * 5f));
        }
        HandleCameraNoise();
    }
    void HandleCameraNoise()
    {
        targetFov = Mathf.Lerp(minFov, maxFov, moveSpeed / 10f);
        freeLookCamera.m_Lens.FieldOfView = Mathf.Lerp(freeLookCamera.m_Lens.FieldOfView, targetFov, Time.deltaTime * 2f);

        float targetFrequency = Mathf.Lerp(minFrequency, maxFrequency, moveSpeed / 10f);
        if(!canNoise)return;

        if (topRigNoise) topRigNoise.m_FrequencyGain = targetFrequency;
        if (midRigNoise) midRigNoise.m_FrequencyGain = targetFrequency;
        if (botRigNoise) botRigNoise.m_FrequencyGain = targetFrequency;
    }
    void HandleRotation()
    {
        rotationInput = inputs.Player.Rotation.ReadValue<Vector2>();
        transform.Rotate(Vector3.up, rotationInput.x * rotationSpeed * Time.deltaTime);
        if (characterController.isGrounded && rotationInput != Vector2.zero && moveInput == 0)
        {
            animator.SetBool("turning", true);
            wheels.ForEach(wheel => wheel.transform.Rotate(Vector3.up, 300 * Time.deltaTime));
        }
        else
        {
            animator.SetBool("turning", false);
        }
    }
    IEnumerator Interaction()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactionRange);
        Interactable nearestInteractable = null;
        float nearestDistance = float.MaxValue;

        foreach (var hitCollider in hitColliders)
        {
            Interactable interactable = hitCollider.GetComponent<Interactable>();
            if (interactable != null && interactable.isInRange)
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestInteractable = interactable;
                }
            }
            if (nearestInteractable != null && nearestInteractable.CanInteract && characterController.isGrounded)
            {
                IEnumerator prepare = PrepareForInteractionCoroutine(nearestInteractable.interactionPoint);
                canNoise = false;
                bodyTarget.transform.DOLocalRotate(new Vector3(0, 0, 0), 1f).onComplete += () =>
                {
                    StartCoroutine(prepare);
                };

                if (topRigNoise) topRigNoise.m_FrequencyGain = 0;
                if (midRigNoise) midRigNoise.m_FrequencyGain = 0;
                if (botRigNoise) botRigNoise.m_FrequencyGain = 0;

                yield return prepare;
                nearestInteractable.Interact(this);
                canNoise = true;
                yield break;
            }
            else
            {
                HasInteracted = false;
                yield break;
            }
        }

        yield break;
    }

    IEnumerator PrepareForInteractionCoroutine(Transform interactionPoint)
    {
        float moveSpeedToPoint = 2f;

        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        Vector3 targetPos = interactionPoint.position;
        targetPos.y = startPos.y;

        Quaternion targetRot = interactionPoint.rotation;

        float distance = Vector3.Distance(startPos, targetPos);
        float duration = distance / moveSpeedToPoint;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            Vector3 newPos = Vector3.Lerp(startPos, targetPos, t);
            characterController.Move(newPos - transform.position);

            transform.rotation = Quaternion.Slerp(startRot, targetRot, t);

            yield return null;
        }

        transform.position = targetPos;
        transform.rotation = targetRot;

        yield return new WaitForSeconds(0.5f);
        moveSpeed = 0;
        accelerationTime = 0f;
    }
    private bool IsDead()
    {
        if((ChargeValue <= 0 && !isDead) || GameManager.Instance.Timer <= 0)
        {
            isDead = true;
            return true;
        }
        return false;
    }
    public void SetJumpAnimationIntroFinished()
    {
        animator.SetBool("Landed", false);
        verticalVelocity = jumpForce;
        hasJumped = true;
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("RotatingPlatform") && characterController.isGrounded)
        {
            currentPlatform = hit.gameObject;
            lastPlatformRotation = currentPlatform.transform.rotation;
        }
    }
    public void ActivateSpeedBoost()
    {
        speedBooster = 1.5f;
        StartCoroutine(ResetSpeedBoost());
    }
    IEnumerator ResetSpeedBoost()
    {
        yield return new WaitForSeconds(speedBoosterDuration);
        speedBooster = 1f;
    }
    IEnumerator Die()
    {
        bodyTarget.transform.DOLocalRotate(new Vector3(0, 0, 0), 1f);
        animator.SetTrigger("Death");
        moveSpeed = 0;
        accelerationTime = 0f;
        canNoise = false;
        if (topRigNoise) topRigNoise.m_FrequencyGain = 0;
        if (midRigNoise) midRigNoise.m_FrequencyGain = 0;
        if (botRigNoise) botRigNoise.m_FrequencyGain = 0;
        GameManager.Instance.LoadDeathScene();
        yield return null;
    }
    private void OnEnable()
    {
        inputs.Enable();
    }
    private void OnDisable()
    {
        inputs.Disable();
    }
}
