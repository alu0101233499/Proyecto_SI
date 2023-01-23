using UnityEngine;

/// <summary>
/// 	Clase para gestionar el comportamiento de las monedas.
/// </summary>
public class Coin : MonoBehaviour
{
    /// <summary>
    /// 	Componente Transform del objeto.
    /// </summary>
    private Transform selfTransform;

    /// <summary>
    /// 	Velocidad de giro del objeto.
    /// </summary>
    private float rotationSpeed = 50f;

    
    void Start()
    {
        selfTransform = GetComponent<Transform>();
    }

    void Update()
    {
        selfTransform.Rotate(rotationSpeed * Time.deltaTime, 0, 0);
    }

    /// <summary>
    /// 	Gestión de colisiones.
    /// </summary>
    void OnTriggerEnter(Collider collider)
    {
        // Si colisiona con el jugador, desaparece.
        if (collider.gameObject.tag == "Player") Disappear();
    }

    /// <summary>
    /// 	Hace desaparecer a la moneda.
    /// </summary>
    void Disappear()
    {
        gameObject.SetActive(false);
    }
}
