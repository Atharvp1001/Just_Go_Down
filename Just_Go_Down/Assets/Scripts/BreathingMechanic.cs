using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BreathingSystem : MonoBehaviour
{
    [Header("Lung Settings")]
    [SerializeField] private float maxLungCapacity = 100f;
    [SerializeField] private float passiveLungDrain = 2f;
    [SerializeField] private float movementLungDrainMultiplier = 1.5f;
    [SerializeField] private float currentLungOxygen = 100f;

    [Header("O2 Tank Settings")]
    [SerializeField] private float maxTankCapacity = 100f;
    [SerializeField] private float tankDrainPerSecond = 5f;
    [SerializeField] private float lungRefillRate = 15f;
    [SerializeField] private float currentTankOxygen = 100f;

    [Header("Effects")]
    [SerializeField] private float lowOxygenThreshold = 20f;
    [SerializeField] private float damagePerSecond = 5f;
    [SerializeField] private float movementPenaltyMultiplier = 0.5f;

    [Header("UI References")]
    [SerializeField] private Slider lungBar;
    [SerializeField] private Slider tankBar;
    [SerializeField] private Image lungFillImage;
    [SerializeField] private Image tankFillImage;

    [Header("References")]
    [SerializeField] private UnderwaterMovement movementScript;

    private bool isBreathingFromTank = false;
    private bool isMoving = false;
    private float originalSwimSpeed;

    // ADD THIS
    private Swimming inputActions;

    private void Awake()
    {
        // ADD THIS - Initialize input actions
        inputActions = new Swimming();
    }

    private void OnEnable()
    {
        // ADD THIS - Enable input
        inputActions.Underwater.Enable();
    }

    private void OnDisable()
    {
        // ADD THIS - Disable input
        inputActions.Underwater.Disable();
    }

    private void Start()
    {
        currentLungOxygen = maxLungCapacity;
        currentTankOxygen = maxTankCapacity;

        if (movementScript != null)
        {
            originalSwimSpeed = movementScript.SwimSpeed;
        }

        UpdateUI();
    }

    private void Update()
    {
        // ADD THIS - Read breath input directly
        isBreathingFromTank = inputActions.Underwater.Breathe.IsPressed();

        HandleLungDepletion();
        HandleLowOxygenEffects();
        UpdateUI();
    }

    // REMOVE OR COMMENT OUT - Not needed anymore
    // public void OnBreathe(InputAction.CallbackContext context)
    // {
    //     if (context.performed)
    //     {
    //         isBreathingFromTank = true;
    //     }
    //     else if (context.canceled)
    //     {
    //         isBreathingFromTank = false;
    //     }
    // }

    public void SetMoving(bool moving)
    {
        isMoving = moving;
    }

    private void HandleLungDepletion()
    {
        if (isBreathingFromTank && currentTankOxygen > 0)
        {
            float breathAmount = lungRefillRate * Time.deltaTime;
            float tankCost = tankDrainPerSecond * Time.deltaTime;

            if (currentTankOxygen < tankCost)
            {
                tankCost = currentTankOxygen;
                breathAmount = tankCost * (lungRefillRate / tankDrainPerSecond);
            }

            currentLungOxygen = Mathf.Min(currentLungOxygen + breathAmount, maxLungCapacity);
            currentTankOxygen = Mathf.Max(currentTankOxygen - tankCost, 0);
        }
        else
        {
            float drainRate = passiveLungDrain;

            if (isMoving)
            {
                drainRate *= movementLungDrainMultiplier;
            }

            currentLungOxygen = Mathf.Max(currentLungOxygen - drainRate * Time.deltaTime, 0);
        }
    }

    private void HandleLowOxygenEffects()
    {
        if (currentLungOxygen <= lowOxygenThreshold && currentLungOxygen > 0)
        {
            if (lungFillImage != null)
            {
                lungFillImage.color = Color.Lerp(Color.red, Color.cyan, currentLungOxygen / lowOxygenThreshold);
            }

            if (movementScript != null)
            {
                movementScript.SwimSpeed = originalSwimSpeed * movementPenaltyMultiplier;
            }
        }
        else if (currentLungOxygen <= 0)
        {
            if (movementScript != null)
            {
                movementScript.SwimSpeed = originalSwimSpeed * 0.3f;
            }

            if (lungFillImage != null)
            {
                float flash = Mathf.PingPong(Time.time * 2, 1);
                lungFillImage.color = Color.Lerp(Color.black, Color.red, flash);
            }
        }
        else
        {
            if (lungFillImage != null)
            {
                lungFillImage.color = Color.cyan;
            }

            if (movementScript != null)
            {
                movementScript.SwimSpeed = originalSwimSpeed;
            }
        }
    }

    private void UpdateUI()
    {
        if (lungBar != null)
        {
            lungBar.value = currentLungOxygen / maxLungCapacity;
        }

        if (tankBar != null)
        {
            tankBar.value = currentTankOxygen / maxTankCapacity;
        }
    }

    public void RefillTank(float amount)
    {
        currentTankOxygen = Mathf.Min(currentTankOxygen + amount, maxTankCapacity);
    }

    public void AddTankCapacity(float amount)
    {
        maxTankCapacity += amount;
    }

    public float GetLungPercentage()
    {
        return currentLungOxygen / maxLungCapacity;
    }

    public float GetTankPercentage()
    {
        return currentTankOxygen / maxTankCapacity;
    }

    public bool IsOutOfOxygen()
    {
        return currentLungOxygen <= 0 && currentTankOxygen <= 0;
    }
}
