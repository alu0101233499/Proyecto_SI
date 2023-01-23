using UnityEngine;

/// <summary>
/// 	Clase para gestionar el comportamiento de los botiquines.
/// </summary>
public class MedicKits : MonoBehaviour
{
    void Start() {}

    void Update() {}

    /// <summary>
    /// 	Gestión de colisiones.
    /// </summary>
    void OnTriggerEnter(Collider collider)
    {
        // Si colisiona con el jugador, desaparece.
        if (collider.gameObject.tag == "Player") Disappear();
    }

    /// <summary>
    /// 	Método para hacer desaparecer el botiquín.
    /// </summary>
    void Disappear()
    {
        gameObject.SetActive(false);
    }
}
