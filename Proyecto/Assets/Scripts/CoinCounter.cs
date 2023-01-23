using UnityEngine;
using TMPro;

/// <summary>
/// 	Clase para gestionar el contador de monedas de la interfaz de usuario.
/// </summary>
public class CoinCounter : MonoBehaviour
{
    /// <summary>
    /// 	Texto a escribir en la GUI.
    /// </summary>
    public TextMeshProUGUI text;

    /// <summary>
    /// 	Método para establecer la cantidad de monedas.
    /// </summary>
    public void SetCoins(int quantity)
    {
        text.text = quantity.ToString();
    }
}
