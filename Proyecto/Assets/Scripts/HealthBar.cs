using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 	Clase para gestionar la barra de vida de la GUI.
/// </summary>
public class HealthBar : MonoBehaviour
{
    /// <summary>
    /// 	Incorpora un slider.
    /// </summary>
    public Slider slider;

    /// <summary>
    /// 	Incorpora un gradiente.
    /// </summary>
    public Gradient gradient;

    /// <summary>
    /// 	Incorpora una imagen.
    /// </summary>
    public Image fill;

    /// <summary>
    /// 	Función para establecer la vida máxima.
    /// </summary>
    public void SetMaxHealth(float max_health)
    {
        slider.maxValue = max_health;

        fill.color = gradient.Evaluate(slider.value / slider.maxValue);
    }

    /// <summary>
    /// 	Función para establecer la vida actual.
    /// </summary>
    public void SetHealth(float health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.value / slider.maxValue);
    }
}
