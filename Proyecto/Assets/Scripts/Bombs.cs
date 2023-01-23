using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 	Clase para gestionar el movimiento de las bombas dentro de NavMesh.
/// </summary>
public class Bombs : MonoBehaviour
{
    /// <summary>
    /// 	Objeto del jugador, para obtener su posición.
    /// </summary>
    public GameObject player;

    /// <summary>
    /// 	Componente de navegación de la bomba.
    /// </summary>
    private NavMeshAgent agent;

    /// <summary>
    /// 	Variable booleana que define si la bomba se encuentra activa.
    /// </summary>
    private bool isActive;

    /// <summary>
    /// 	Temporizador de la bomba.
    /// </summary>
    private float counter;

    /// <summary>
    /// 	Establece las configuraciones iniciales del agente y
    ///     lo coloca en estado apagado.
    /// </summary>
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 10f;
        isActive = false;
        counter = 20;

        foreach (Renderer render in GetComponentsInChildren<Renderer>())
            render.enabled = false;

        foreach (Light light in GetComponentsInChildren<Light>())
            light.enabled = false;
    }

    void Update()
    {
        // En caso de que el jugador esté más cerca de 10 casillas se activa.
        if (Vector3.Distance(transform.position, player.transform.position) <= 10)
        {
            foreach (Renderer render in GetComponentsInChildren<Renderer>())
                render.enabled = true;

            foreach (Light light in GetComponentsInChildren<Light>())
                light.enabled = true;

            isActive = true;
        }

        // Si está activo, se aproxima al jugador.
        if (isActive == true) 
        {
            counter = counter - Time.deltaTime;
            agent.destination = player.transform.position;

            if (counter <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 	Método público para configurar la velocidad.
    /// </summary>
    public void SetSpeed(float speed)
    {
        agent.speed = speed;
    }

    /// <summary>
    /// 	Método público para retornar la velocidad.
    /// </summary>
    public float GetSpeed()
    {
        return agent.speed;
    }
}
