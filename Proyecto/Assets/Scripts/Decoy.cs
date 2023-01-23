using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 	Clase para gestionar el comportamiento del tiempo de enfríamiento
///     de la copia en la GUI.
/// </summary>
public class Decoy : MonoBehaviour
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
    /// 	Variable booleana para señalar si la copia está activada.
    /// </summary>
    private bool decoy_enabled;

    /// <summary>
    /// 	Tiempo de enfríamiento actual.
    /// </summary>
    private float current_cooldown;

    /// <summary>
    /// 	Función para activar la habilidad en la GUI.
    /// </summary>
    public void Enable()
    {
        decoy_enabled = true;
        fill.color = gradient.Evaluate(1f);
    }

    /// <summary>
    /// 	Función para establecer el tiempo de enfríamiento de la habilidad.
    /// </summary>
    public void SetDecoyCooldownTime(float seconds)
    {
        slider.maxValue = seconds;
        slider.value = seconds;
    }

    /// <summary>
    /// 	Función para señalar que la habilidad está disponible para su uso.
    /// </summary>
    public bool Ready()
    {
        return slider.value == slider.maxValue;
    }

    /// <summary>
    /// 	Función para señalar que la habilidad ha sido usada.
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
        decoy_enabled = false;
        current_cooldown = 0;
    }

    void Update()
    {
        if (!decoy_enabled) return;

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
