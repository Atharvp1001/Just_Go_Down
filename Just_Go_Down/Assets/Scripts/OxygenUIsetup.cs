// Simple UI setup script
using UnityEngine;
using UnityEngine.UI;

public class OxygenUISetup : MonoBehaviour
{
    [SerializeField] private BreathingSystem breathingSystem;
    [SerializeField] private Slider lungSlider;
    [SerializeField] private Slider tankSlider;
    [SerializeField] private Text lungText;
    [SerializeField] private Text tankText;

    private void Update()
    {
        if (breathingSystem != null)
        {
            // Update sliders
            if (lungSlider != null)
                lungSlider.value = breathingSystem.GetLungPercentage();

            if (tankSlider != null)
                tankSlider.value = breathingSystem.GetTankPercentage();

            // Update text displays
            if (lungText != null)
                lungText.text = $"Lungs: {(breathingSystem.GetLungPercentage() * 100):F0}%";

            if (tankText != null)
                tankText.text = $"Tank: {(breathingSystem.GetTankPercentage() * 100):F0}%";
        }
    }
}
