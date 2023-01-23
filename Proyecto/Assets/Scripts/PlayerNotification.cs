using UnityEngine;

/// <summary>
/// 	Clase para enviar notificaciones de posición al notificador
/// </summary>
public class PlayerNotification : MonoBehaviour
{
    /// <summary>
    /// 	Notificador de posición al que se enviarán las posiciones
    /// </summary>
    public PositionNotificator notificator;

    /// <summary>
    /// 	Prefab del decoy a crear cuando sea necesario
    /// </summary>
    public GameObject decoyPrefab;

    void Update()
    {
        NotifyPosition();
    }

    /// <summary>
    /// 	Envía al notificador la posición actual del jugador.
    ///     Si el notificador ha sido cedido a un señuelo (notificator == null),
    ///     no se envía la posición.
    /// </summary>
    void NotifyPosition()
    {
        notificator?.NotifyPosition(transform.position);
    }

    /// <summary>
    /// 	Crea un señuelo (decoy) en la posición actual del jugador.
    ///     El señuelo se encargará de notificar la posición del jugador
    ///     durante un tiempo determinado. Si el señuelo actual aun no ha
    ///     expirado, no se crea el nuevo señuelo.
    /// </summary>
    public void CreateDecoy()
    {
        Debug.Log("Creating decoy...");

        if (notificator == null)
        {
            Debug.Log("Notificator is null, aborting decoy creation");
            return;
        }

        var decoy = Instantiate(
            decoyPrefab,
            transform.position,
            Quaternion.identity    
        );

        decoy.GetComponent<DecoyBehaviour>().SetupDecoy(
            5f,         // Duration
            gameObject, // Parent
            notificator
        );
        
        notificator = null;
        Debug.Log($"Decoy successfully created at {transform.position}");
    }
}
