using UnityEngine;

/// <summary>
/// 	Clase para representar el comportamiento de un señuelo (decoy)
/// </summary>
public class DecoyBehaviour : MonoBehaviour
{
    /// <summary>
    /// 	Edad máxima del señuelo en segundos
    /// </summary>
    public float lifeTime;

    /// <summary>
    /// 	Edad actual del señuelo en segundos
    /// </summary>
    public float age;

    /// <summary>
    /// 	Notificador al que envía la posición del señuelo
    /// </summary>
    public PositionNotificator notificator;

    /// <summary>
    /// 	gameObject del jugador que ha lanzado el señuelo, al
    /// 	que se le asignará el notificador cuando el señuelo
    /// 	se destruya
    /// </summary>
    public GameObject parent;

    void Start()
    {
        age = 0f;
        lifeTime = 10f;
    }

    void Update()
    {
        age += Time.deltaTime;
        if (age >= lifeTime)
        {
            parent.GetComponent<PlayerNotification>().notificator = notificator;
            Debug.Log("Decoy destroyed");
            Destroy(gameObject);
        }
    }


    /// <summary>
    /// 	Asigna los valores iniciales del señuelo par que empiece a atraer
    /// </summary>
    public void SetupDecoy(float lifeTime, GameObject parent, PositionNotificator notificator)
    {
        this.lifeTime = lifeTime;
        this.notificator = notificator;
        this.parent = parent;
        notificator.NotifyPosition(transform.position);
    }
}
