using UnityEngine;

/// <summary>
/// 	Clase para gestionar el menú de tienda.
/// </summary>
public class ShopMenu : MonoBehaviour
{
    /// <summary>
    /// 	Notificador del sistema de eventos.
    /// </summary>
    public PlayerController player;

    /// <summary>
    /// 	Objeto donde se escribirán los elementos del menú de tienda.
    /// </summary>
    public GameObject shopMenuUI;

    void Start()
    {
        // Suscripciones al sistema de eventos.
        player.Shop += Shop;
        player.Out += Out;
    }


    void Update() {}

    /// <summary>
    /// 	Método para mostrar el menú de tienda.
    /// </summary>
    public void Shop()
    {
        shopMenuUI.SetActive(true);
    }

    /// <summary>
    /// 	Método para eliminar el menú de tienda.
    /// </summary>
    public void Out()
    {
        shopMenuUI.SetActive(false);
    }
}
