using UnityEngine;
using TMPro;

/// <summary>
/// 	Clase para gestionar el menú de dificultad.
/// </summary>
public class SelectionAreaMenu : MonoBehaviour
{
    /// <summary>
    /// 	Sistema de eventos a los que está suscrito el objeto.
    /// </summary>
    public SystemCommands system;
    public PlayerController player;

    /// <summary>
    /// 	Sistema de eventos a los que está suscrito el objeto.
    /// </summary>
    public GameObject hardnessMenuUI;

    /// <summary>
    /// 	Texto a escribir.
    /// </summary>
    public TMP_Text text;

    void Start()
    {
        // Notificadores de eventos.
        player.Hardness += Hardness;
        player.OutSelection += OutSelection;
    }

    void Update()
    {}

    /// <summary>
    /// 	Función para cambiar la dificultad.
    /// </summary>
    public void Hardness()
    {
        hardnessMenuUI.SetActive(true);
        if (system.isNormal) text.text = "Press 1 to change difficulty\nCurrent Difficulty: Normal";
        else text.text = "Press 1 to change difficulty\nCurrent Difficulty: Hard";
    }

    /// <summary>
    /// 	Función para apagar el menú.
    /// </summary>
    public void OutSelection()
    {
        hardnessMenuUI.SetActive(false);
    }
}
