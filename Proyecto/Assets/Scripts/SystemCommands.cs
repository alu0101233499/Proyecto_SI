using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 	Clase para gestionar el sistema de juego.
/// </summary>
public class SystemCommands : MonoBehaviour
{
    /// <summary>
    /// 	Sistema de notificación de eventos.
    /// </summary>
    public delegate void command();
    public event command CloseDoor;
    public event command Enable;

    /// <summary>
    /// 	Sistema de gestión de animaciones.
    /// </summary>
    public Animator animator;

    /// <summary>
    /// 	Notificador del sistema de eventos.
    /// </summary>
    public PlayerController player;

    /// <summary>
    /// 	Objetos del sistema.
    /// </summary>
    private GameObject[] turrets;
    public GameObject maze, spawn, goal, testing, connectionMaze;

    /// <summary>
    /// 	Selección del nivel de dificultad.
    /// </summary>
    public bool isNormal;

    void Start()
    {
        isNormal = true;
        player.ChangeHardness += ChangeHardness;
        player.Restart += Restarting;
        turrets = GameObject.FindGameObjectsWithTag("Torreta");
    }

    void Update() {}

    /// <summary>
    /// 	Sistema de colisiones.
    /// </summary>
    void OnTriggerEnter(Collider collider)
    {
        if ((collider.gameObject.tag == "Player") && (gameObject.tag == "Entrada"))
        {
            CloseDoor();
            // Procesos de optimización, cancelando ciertos objetos dependiendo
            // de la zona en la que se entra (Laberinto).
            foreach (Transform transform in testing.transform)
            {
                if (transform.tag == "Plataformas")
                {
                    transform.gameObject.SetActive(false);
                }
                if (transform.tag == "Luces")
                {
                    transform.gameObject.SetActive(false);
                }
            }
        }

        if ((collider.gameObject.tag == "Player") && (gameObject.tag == "EntradaTesting"))
        {
            CloseDoor();
            // Procesos de optimización, cancelando ciertos objetos dependiendo
            // de la zona en la que se entra (Zona de pruebas).
            maze.SetActive(false);
            goal.SetActive(false);
            connectionMaze.SetActive(false);
        }

        if ((collider.gameObject.tag == "Player") && (gameObject.tag == "Ready"))
        {
            // Desencadena al agente inteligente.
            Enable();
        }
    }

    /// <summary>
    /// 	Cambio de dificultad.
    /// </summary>
    void ChangeHardness()
    {
        if (isNormal == true)
        {
            foreach (GameObject turret in turrets) turret.GetComponent<TurretController>().enabled = false;
            isNormal = false;
        }
        else
        {
            foreach (GameObject turret in turrets) turret.GetComponent<TurretController>().enabled = true;
            isNormal = true;
        }
    }

    /// <summary>
    /// 	Reinicio de la partida en la GUI.
    /// </summary>
    void Restarting()
    {
        animator.SetTrigger("RestartTrigger");
    }

    /// <summary>
    /// 	Reinicio de la partida en la escena.
    /// </summary>
    void OnFadeComplete()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
