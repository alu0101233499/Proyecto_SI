using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 	Clase para gestionar el tiempo de enfríamiento de la habilidad
///     de salto de altura.
/// </summary>
public class HighJumpBar : MonoBehaviour
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
    /// 	Variable booleana que define si el salto está activo.
    /// </summary>
    private bool hj_enabled;

    /// <summary>
    /// 	Variable que define el tiempo de enfríamiento actual.
    /// </summary>
    public float current_cooldown;

    /// <summary>
    /// 	Método que permite establecer el tiempo de enfríamiento.
    /// </summary>
    public void SetHighJumpCooldownTime(float seconds)
    {
        slider.maxValue = seconds;
        slider.value = seconds;
    }

    /// <summary>
    /// 	Método que permite colocar como preparada a la habilidad en la GUI.
    /// </summary>
    public bool Ready()
    {
        return slider.value == slider.maxValue;
    }

    /// <summary>
    /// 	Método que permite colocar como usable a la habilidad.
    /// </summary>
    public void Enable()
    {
        hj_enabled = true;
        fill.color = gradient.Evaluate(1f);
    }

    /// <summary>
    /// 	Método que permite usar la habilidad.
    /// </summary>
    public void Use()
    {
        current_cooldown = slider.maxValue;
    }

    /// <summary>
    /// 	Configuraciones iniciales.
    /// </summary>
    void Start()
    {
        fill.color = gradient.Evaluate(0f);
        hj_enabled = false;
        current_cooldown = 0;
    }

    void Update()
    {
        if (!hj_enabled) return;

        if (current_cooldown > 0)
        {
            current_cooldown = current_cooldown - Time.deltaTime;
            slider.value = slider.maxValue - current_cooldown;
            fill.color = gradient.Evaluate(0f);
        }
        else
        {
            current_cooldown = 0;
            fill.color = gradient.Evaluate(1f);
        }
    }
}
