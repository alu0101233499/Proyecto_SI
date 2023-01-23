using UnityEngine;
using System.Linq;

/// <summary>
/// 	Clase pública para añadir una funcionalidad de concatenación
///     a los arrays.
/// </summary>
public static class Extensions
{
    public static T[] Append<T>(this T[] array, T item)
    {
        if (array == null) {
            return new T[] { item };
        }
        return array.Concat(new T[] { item }).ToArray();
    }
}

/// <summary>
/// 	Clase para controlar las torretas.
/// </summary>
public class TurretController : MonoBehaviour
{
	/// <summary>
	/// 	Variables para definir etiquetas de los objetivos.
	/// </summary>
	private string bombTag = "Bombs";
	private string enemyTag = "Enemigo";
	private string targetTag;

	/// <summary>
	/// 	Variables para definir ciertos atributos en punto flotante.
	/// </summary>
	private float range, turnSpeed, previousSpeed;

	/// <summary>
	/// 	Variables para definir ciertos cuerpos físicos de las torretas
	///     y sus objetivos.
	/// </summary>
	public Transform firePoint, partToRotate, target;

	/// <summary>
	/// 	Láser de la torreta.
	/// </summary>
	public LineRenderer lineRenderer;

	/// <summary>
	/// 	Bombas objetivo.
	/// </summary>
	private Bombs targetBombs;

	/// <summary>
	/// 	Agente inteligente "Espantaviejas de Tamaimo".
	/// </summary>
	private AgentMovement enemy;

	/// <summary>
	/// 	Configuraciones iniciales.
	/// </summary>
	void Start ()
    {
		InvokeRepeating("UpdateTarget", 0f, 0.5f);

        lineRenderer.enabled = false;
		range = 10f;
		turnSpeed = 10f;
		previousSpeed = 7f;
	}
	
	/// <summary>
	/// 	Mantiene actualizado al objetivo en todo momento.
	/// </summary>
	void UpdateTarget ()
	{
		GameObject[] bombs = GameObject.FindGameObjectsWithTag(bombTag);
		GameObject amogos = GameObject.FindGameObjectWithTag(enemyTag);
		GameObject[] enemies = bombs.Append(amogos);
		
		float shortestDistance = Mathf.Infinity;
		GameObject nearestEnemy = null;

		// Se realiza una gestión de objetivos cuando hay varios en rango.
		foreach (GameObject enemy in enemies)
		{
			float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
			if (distanceToEnemy < shortestDistance)
			{
				shortestDistance = distanceToEnemy;
				nearestEnemy = enemy;
			}
		}

		// Se maneja un objetivo de tipo bomba.
		if (nearestEnemy != null && shortestDistance <= range && nearestEnemy.tag == "Bombs")
		{
			target = nearestEnemy.transform;
			targetBombs = nearestEnemy.GetComponent<Bombs>();
			targetTag = target.tag;
		}
		// Se maneja un objetivo de tipo enemigo.
		else if (nearestEnemy != null && shortestDistance <= range && nearestEnemy.tag == "Enemigo")
		{
			target = nearestEnemy.transform;
			enemy = nearestEnemy.GetComponent<AgentMovement>();
			targetTag = target.tag;
		}
		// No existe objetivo.
        else target = null;
	}

	/// <summary>
	/// 	Gestiona los objetivos en todo instante.
	/// </summary>
	void FixedUpdate () {
		// Si los enemigos salen del rango, se reestablecen sus velocidades.
		if (target == null)
		{
            if (lineRenderer.enabled)
            {
                lineRenderer.enabled = false;

				if (targetTag == "Bombs") targetBombs.SetSpeed(7f);
				if (targetTag == "Enemigo")
				{
					previousSpeed = previousSpeed + previousSpeed * 0.2f;
					enemy.SetSpeed(previousSpeed);
				} 
            }
			return;
		}

		// En caso de que estén en rango, se fija el visor en ellos.
		LockOnTarget();

		// Se crea un rayo hacia el objetivo.
		Ray ray = new Ray(firePoint.transform.position, firePoint.TransformDirection(Vector3.forward));
		RaycastHit hit;

		// Se desencadena un rayo que afecta de forma distinta a cada tipo de objetivo.
		if (Physics.Raycast(ray, out hit, range))
		{
          	if (hit.collider.tag == bombTag) Laser(1);	
			if (hit.collider.tag == enemyTag) Laser(2);	
 		}
	}

	/// <summary>
	/// 	Método para fijar la torreta en el objetivo.
	/// </summary>
	void LockOnTarget ()
	{
		Vector3 dir = target.position - transform.position;
		Quaternion lookRotation = Quaternion.LookRotation(dir);
		Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
		partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
	}

	/// <summary>
	/// 	Método para producir un rayo hacia el objetivo.
	/// </summary>
	void Laser (int type)
	{
		float distanceToEnemy = Vector3.Distance(transform.position, target.transform.position);
		
		float speed = (distanceToEnemy * 6f) / range;
		if (speed < 2f) speed = 2f;

		if (type == 1) targetBombs.SetSpeed(speed);
		if (type == 2) enemy.SetSpeed(speed * 1.5f);

		if (!lineRenderer.enabled) lineRenderer.enabled = true;

		lineRenderer.SetPosition(0, firePoint.position);
		lineRenderer.SetPosition(1, target.position);

		Vector3 dir = firePoint.position - target.position;
	}

	void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, 0f);
	}
}
