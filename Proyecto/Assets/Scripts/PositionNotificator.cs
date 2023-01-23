using UnityEngine;

/// <summary>
/// 	Scriptable object que recibe una posición y notifica a todos los suscriptores
/// </summary>
[CreateAssetMenu(fileName = "PositionNotificator")]
public class PositionNotificator : ScriptableObject
{
    public delegate void PositionChanged(Vector3 position);

    /// <summary>
    /// 	Evento que se dispara cuando se emite una señal
    ///     (Ej. cuando el Player se mueve)
    /// </summary>
    public event PositionChanged OnSignalEmitted;

    /// <summary>
    /// 	Función de ejecución de la señal
    /// </summary>
    public void NotifyPosition(Vector3 position)
    {
        OnSignalEmitted?.Invoke(position);
    }
}
