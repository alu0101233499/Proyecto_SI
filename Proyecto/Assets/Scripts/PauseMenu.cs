using UnityEngine;

/// <summary>
/// 	Clase para gestionar el menú de pausa.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    /// <summary>
    /// 	Variable que define si el juego se encuentra pausado.
    /// </summary>
    public static bool isPaused;

    /// <summary>
    /// 	Objetos que definen el menú de pausa.
    /// </summary>
    public GameObject pauseMenuUI, menuUI;

    /// <summary>
    /// 	Configuraciones iniciales.
    /// </summary>
    void Start()
    {
        isPaused = false;
    }

    void Update()
    {
        // Si se pulsa el botón de ESC, se accede al menú.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused == true) Resume();
            else Pause();
        }
    }

    /// <summary>
    /// 	Método que permite reanudar el juego.
    /// </summary>
    public void Resume()
    {
        // Bloquea y esconde el cursor.
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Desactiva los menús.
        pauseMenuUI.SetActive(false);
        menuUI.SetActive(false);

        // Reanuda el tiempo.
        Time.timeScale = 1f;
        isPaused = false;
    }

    /// <summary>
    /// 	Método que permite parar el juego.
    /// </summary>
    public void Pause()
    {
        // Desbloquea y muestra el cursor.
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Activa el menú de pausa.
        pauseMenuUI.SetActive(true);

        // Detiene el tiempo.
        Time.timeScale = 0f;
        isPaused = true;
    }

    /// <summary>
    /// 	Método que permite salir del juego en la aplicación.
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }

    /// <summary>
    /// 	Método que permite abrir el menú de controles.
    /// </summary>
    public void Menu()
    {
        pauseMenuUI.SetActive(false);
        menuUI.SetActive(true);
    }
}
