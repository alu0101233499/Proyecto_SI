using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 	Clase para gestionar el la barra de estamina.
/// </summary>
public class StaminaBar : MonoBehaviour
{
    /// <summary>
    /// 	Incorpora un slider.
    /// </summary>
    public Slider slider;

    /// <summary>
    /// 	Método para establecer la estamina máxima.
    /// </summary>
    public void SetMaxStamina(float max_stamina)
    {
        slider.maxValue = max_stamina;
        slider.value = max_stamina;
    }

    /// <summary>
    /// 	Método para regenerar la estamina.
    /// </summary>
    public void Regen()
    {
        slider.value = slider.value + (8 * Time.deltaTime);
    }

    /// <summary>
    /// 	Método para reducir la estamina.
    /// </summary>
    public void Reduce()
    {
        slider.value = slider.value - (25 * Time.deltaTime);
    }
}
