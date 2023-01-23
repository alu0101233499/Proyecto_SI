using UnityEngine;
using TMPro;

/// <summary>
/// 	Clase para gestionar el contador de botiquines en la GUI.
/// </summary>
public class MedKitCounter : MonoBehaviour
{
    /// <summary>
    /// 	Texto a escribir en la GUI.
    /// </summary>
    public TextMeshProUGUI text;

    /// <summary>
    /// 	Método para establecer la cantidad de botiquines.
    /// </summary>
    public void SetMedKits(int quantity)
    {
        text.text = quantity.ToString();
    }
}
