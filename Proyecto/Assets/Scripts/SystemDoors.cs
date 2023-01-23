using UnityEngine;

/// <summary>
/// 	Clase para gestionar el sistema de cerrado de compuertas.
/// </summary>
public class SystemDoors : MonoBehaviour
{
    /// <summary>
    /// 	Notificador del sistema de eventos.
    /// </summary>
    public SystemCommands system;

    /// <summary>
    /// 	Componente Rigidbody de la compuerta.
    /// </summary>
    private Rigidbody selfRigidbody;

    void Start()
    {
        selfRigidbody = GetComponent<Rigidbody>();
        system.CloseDoor += Close;
    }

    void Update() {}

    /// <summary>
    /// 	Método para el cerrado de compuertas.
    /// </summary>
    void Close()
    {
        selfRigidbody.useGravity = true;
    }
}
