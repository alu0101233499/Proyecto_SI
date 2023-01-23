using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 	Clase para gestionar el movimiento del agente dentro del NavMesh.
/// </summary>
public class AgentMovement : MonoBehaviour
{
    /// <summary>
    /// 	El agente estará a la escucha del jugador y del controlador del sistema para recuperar eventos.
    /// </summary>
    public PlayerController player;
    public SystemCommands system;

    /// <summary>
    /// 	Notificador de posición al que se suscribe el agente para recibir señales de posición.
    /// </summary>
    public PositionNotificator notificator;

    /// <summary>
    /// 	Agente de navegación del propio gameObject, necesario para moverse por el NavMesh.
    /// </summary>
    public NavMeshAgent agent;

    /// <summary>
    /// 	Cooldown del agente antes de cabrearse, se resetea cuando llega a 20 segundos.
    /// </summary>
    public float angerCooldown = 0f;

    /// <summary>
    /// 	Configuración inicial.
    /// </summary>
    void Start()
    {
        // Suscripciones como observadores a los sistemas de eventos.
        player.Shop += DisableAgent;
        player.Out += EnableAgent;
        system.Enable += EnableAgent;
        notificator.OnSignalEmitted += SetAgentDestination;

        // Agente apagado hasta que se active.
        DisableAgent();
    }

    /// <summary>
    /// 	Evolución del tiempo de enfríamiento.
    /// </summary>
    void Update()
    {
        angerCooldown += Time.deltaTime;

        // Si el enfríamiento supera 20, aumenta su furia.
        if (angerCooldown >= 20f)
        {
            angerCooldown = 0f;
            AngerAgent();
        }
    }

    /// <summary>
    /// 	Activa el agente para que aplique el NavMesh.
    /// </summary>
    public void EnableAgent()
    {
        Debug.Log($"Agent was enabled...");
        agent.enabled = true;
    }

    /// <summary>
    /// 	Desactiva el agente para que no aplique el NavMesh.
    /// </summary>
    public void DisableAgent()
    {
        Debug.Log($"Agent was disabled...");
        agent.enabled = false;
    }

    /// <summary>
    /// 	Cabrea al agente, aumentando su velocidad, aceleración y velocidad angular.
    ///     Este aumento tiene un límite indicado en el código.
    /// </summary>
    void AngerAgent()
    {
        Debug.Log($"Agent was angered...");
        if (agent.speed < 14f) agent.speed += 1f;
        if (agent.angularSpeed < 240f) agent.angularSpeed += 20f;
        if (agent.acceleration < 16f) agent.acceleration += 1f;
    }

    /// <summary>
    /// 	Establece la posición de destino del agente.
    /// </summary>
    void SetAgentDestination(Vector3 position)
    {
        if (agent != null) agent.destination = position;
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
