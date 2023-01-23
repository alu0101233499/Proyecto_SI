using UnityEngine;

// Controlador de personaje e interacciones con el entorno.
public class PlayerController : MonoBehaviour
{
    // GUI

    public HealthBar healthbar;
    public StaminaBar stamina_bar;
    public HighJumpBar high_jump_bar;
    public Decoy decoy_bar;
    public CoinCounter coin_counter;
    public MedKitCounter medkit_counter;

    // Sistema notificador de eventos.
    public delegate void command();
    public event command Hardness;
    public event command OutSelection;
    public event command ChangeHardness;
    public event command Restart;
    public event command Shop;
    public event command Out;

    // Raycast para saber donde está mirando el jugador.
    private RaycastHit ray;

    // Pintura.
    public GameObject paint;

    // Señuelo.
    public GameObject bait;

    // Señuelo auxiliar.
    private GameObject bait2;

    // Constantes.
    const float SPIKE_DAMAGE = 25f;
    const float POISON_DAMAGE = 0.8f;
    const float BOMB_DAMAGE = 50f;
    const float AMOGOS_DAMAGE = 100f;

    // Controlador de personaje.
    private CharacterController controller;

    // Cámara principal.
    private Camera mainCamera;

    // Puntos de estado.
    private float healthPoints, maxHealth;

    // Estado del jugador.
    private bool isAlive;

    // Items.
    private int medicKits, coins;

    // Vectores de movimiento.
    private Vector3 movement, jump, fall;

    // Velocidades de movimiento.
    private float movementSpeed, rotationSpeed, jumpSpeed, gravityForce;

    // Tiempos de enfríamiento.
    private float jumpTimer, stamina, highJumpTimer, baitTimer;

    // Habilidades disponibles.
    private bool jumpState, runState, highJumpState, baitState;

    // Variables auxiliares para la rotación y el movimiento.
    private float pitch, yaw, directionY;

    // Variable para señalar que el botón de correr está pulsado.
    private bool isHeld;

    // Variable para señalar que el jugador está dentro de una tienda.
    private bool isInStore;
    
    // Variable para señalar que el jugador está dentro de la zona de selección
    // de dificultad.
    private bool isInSelectionArea;

    // Variable que define si el usuario está en el campo de pruebas.
    private bool isInTesting;

    void Start()
    {
        // Cursor invisible.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Obtención del controlador de personaje y la cámara.
        controller = GetComponent<CharacterController>();
        mainCamera = Camera.main;

        // Establece el estado del jugador.
        healthPoints = 200f;
        maxHealth = 200f;
        isAlive = true;
        isInStore = false;
        isInSelectionArea = false;
        isInTesting = false;
        medicKits = 0;
        coins = 0;

        coin_counter.SetCoins(0);
        medkit_counter.SetMedKits(0);

        healthbar.SetMaxHealth(maxHealth);
        stamina_bar.SetMaxStamina(100f);

        // Establece las velocidades.
        movementSpeed = 6f;
        rotationSpeed = 600f;
        jumpSpeed = 2.5f;
        gravityForce = 5f;

        // Establece los tiempos de enfríamiento iniciales.
        jumpTimer = 0f;
        highJumpTimer = 0f;
        baitTimer = 0f;
        stamina = 100f;

        // Establece las habilidades disponibles al inicio.
        jumpState = true;
        runState = true;
        highJumpState = false;
        baitState = false;

        // Establece las variables auxiliares de movimiento y rotación.
        pitch = 0f;
        yaw = 0f;
        directionY = 0;

        decoy_bar.SetDecoyCooldownTime(30f);
    }

    void Update()
    {
        Vector3 forward = mainCamera.transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(mainCamera.transform.position, forward, Color.green);

        // Función de movimiento por defecto.
        Move();

        // Función de rotación por defecto.
        Rotate();

        // Función de salto bajo con la tecla SPACE.
        if ((Input.GetButtonDown("Jump") || (jumpTimer > 0)) && (jumpState == true))
        {
            if (jumpTimer <= 0)
            {
                if (isInTesting == true) jumpTimer = 1;
                else jumpTimer = 2.5f;
                directionY = jumpSpeed; 
            }
            Jump();
        }

        // Función de sprint con la tecla LEFT SHIFT.
        if (Input.GetButtonDown("Run") && (stamina > 0) && (runState == true)) isHeld = true;
        if ((isHeld == true) && (stamina >= 1)) Run();
        if (Input.GetButtonUp("Run") && (runState == true)) isHeld = false;

        // Función de salto alto con la tecla E.
        if ((Input.GetButtonDown("High Jump") || (highJumpTimer > 0)) && (highJumpState == true))
        {
            if (highJumpTimer <= 0)
            {
                if (isInTesting == true)
                {
                    high_jump_bar.SetHighJumpCooldownTime(3f);
                    highJumpTimer = 3;
                }
                else
                {
                    high_jump_bar.SetHighJumpCooldownTime(20f);
                    highJumpTimer = 20;
                } 
                directionY = jumpSpeed * 2.5f; 
            }
            if (high_jump_bar.Ready()) high_jump_bar.Use();
            Jump();
        }

        // Función de curación con la tecla R.
        if (Input.GetButtonDown("Heal") && (medicKits > 0))
        {
            Heal();
            medicKits = medicKits - 1;
            medkit_counter.SetMedKits(medicKits);
        }

        // Función de compra en la tienda.
        if (isInStore == true)
        {
            Buy();
        }

        if (isInSelectionArea == true)
        {
            Switch();
        }

        // Función de reinicio en la tecla M.
        if (Input.GetButtonDown("Restart") || (isAlive == false))
        {
            Restart();
        }

        // Función de pintura.
        if ((Physics.Raycast(mainCamera.transform.position, forward, out ray)) && (Input.GetButtonDown("Fire1"))) 
        {
            Instantiate(paint, ray.point, paint.transform.rotation);
        }

        // Función de señuelo en la tecla Q.
        if (Input.GetButtonDown("Bait") && (baitState == true) && (baitTimer <= 0))
        {
            baitTimer = 30;
            Bait();
            decoy_bar.Use();
        }

        // Función de gravedad.
        Fall();

        // Función de enfríamiento de habilidades.
        CooldownEffect();
    }

    // Permite el movimiento con las teclas WS (Arriba-Abajo) y AD (Izquierda-Derecha).
    void Move()
    {
        // Obtención de los valores de movimiento y normalización.
        movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        movement = Vector3.ClampMagnitude(movement, 1);

        // Transformación del movimiento respecto al sistema de coordenadas global.
        Vector3 transformedMovement = transform.TransformDirection(movement * movementSpeed);

        // Movimiento.
        controller.Move(transformedMovement * Time.deltaTime);
    }

    // Permite la rotación con el ratón.
    void Rotate()
    {
        // Obtención de los valores de rotación.
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * rotationSpeed;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * rotationSpeed;

        // Obtención del aumento del cabeceo y guiñada.
        yaw = yaw + (mouseX * 0.8f);
        pitch = pitch - (mouseY * 0.8f);

        // Limitación del cabeceo para mejorar el confort en RV.
        pitch = Mathf.Clamp(pitch, -75f, 75f);

        // Rotación suavizada de la cámara y el jugador.
        mainCamera.transform.eulerAngles = new Vector3(pitch, yaw, 0);
        transform.eulerAngles = new Vector3(0, yaw, 0);
    }

    // Permite realizar un salto.
    void Jump()
    {
        // Establece el valor del salto en base a la dirección en Y.
        // Es importante para hacer que funcione la fuerza de la gravedad
        // a la vez que el salto.
        jump = new Vector3(0, 0, 0);
        directionY = directionY - Time.deltaTime;
        jump.y = directionY;

        // Salto.
        controller.Move(jump * jumpSpeed * Time.deltaTime);
    }

    // Permite aplicar la fuerza de la gravedad.
    void Fall()
    {
        fall = new Vector3(0, 0, 0);
        directionY = directionY - (gravityForce * Time.deltaTime);
        fall.y = directionY;

        // Caída.
        controller.Move(fall * Time.deltaTime);
    }

    // Permite moverse más rápido.
    void Run()
    {
        // Obtención del valor de movimiento.
        movement = controller.transform.forward * Input.GetAxis("Vertical");

        // Movimiento.
        controller.Move(movement * movementSpeed * 1.2f * Time.deltaTime);

        // Reducción de la estamina.
        stamina = stamina - (Time.deltaTime * 25);
        stamina_bar.Reduce();
    }

    // Permite al jugador hacerse daño con elementos dañinos.
    void Hurt(float value)
    {
        // Reducción de puntos de vida.
        healthPoints = healthPoints - value;

        healthbar.SetHealth(healthPoints);

        // Eliminación del jugador si sus puntos de vida bajan a 0 o menos.
        if (healthPoints < 1) isAlive = false;
    }

    // Permite al jugador curarse.
    void Heal()
    {
        // Aumento en los puntos de vida.
        healthPoints = healthPoints + 50f;

        // Valor máximo de los puntos de vida.
        if (healthPoints > maxHealth) healthPoints = maxHealth;

        healthbar.SetHealth(healthPoints);
    }

    // Función para colocar un señuelo.
    void Bait()
    {
        /*bait2 = Instantiate(bait, transform.position, bait.transform.rotation) as GameObject;
        Destroy(bait2, 10.0f);*/
        GetComponent<PlayerNotification>().CreateDecoy();
    }

    // Permite recoger elementos.
    void Take(int type)
    {
        if (type == 1)
        {
            coins = coins + 50;
            coin_counter.SetCoins(coins);
        }
        if (type == 2)
        {
            medicKits = medicKits + 1;
            medkit_counter.SetMedKits(medicKits);
        }
    }

    // Permite comprar en una tienda.
    void Buy()
    {
        // Compra de botiquín.
        if ((Input.GetButtonDown("Number 1")) && (coins >= 25))
        {
            coins = coins - 25;
            medicKits = medicKits + 1;
        }
        // Compra de vida máxima.
        else if ((Input.GetButtonDown("Number 2")) && (coins >= 50))
        {
            coins = coins - 50;
            maxHealth = maxHealth + 50;
        }
        // Compra de salto de altura.
        else if ((Input.GetButtonDown("Number 3")) && (coins >= 200) && (highJumpState == false))
        {
            coins = coins - 200;
            highJumpState = true;
            high_jump_bar.Enable();
        }
        // Compra de sigilo.
        else if ((Input.GetButtonDown("Number 4")) && (coins >= 300) && (baitState == false))
        {
            coins = coins - 300;
            baitState = true;
            decoy_bar.Enable();
        }

        medkit_counter.SetMedKits(medicKits);
        coin_counter.SetCoins(coins);
        healthbar.SetMaxHealth(maxHealth);
    }

    // Cambia la dificultad.
    void Switch()
    {
        if (Input.GetButtonDown("Number 1"))
        {
            ChangeHardness();
        }
    }

    // Permite reducir el tiempo de enfríamiento de las habilidades.
    void CooldownEffect()
    {
        // Reducción del tiempo de enfríamiento del salto.
        if (jumpTimer > 0) jumpTimer = jumpTimer - Time.deltaTime;

        // Reducción del tiempo de enfríamiento del salto.
        if (highJumpTimer > 0) highJumpTimer = highJumpTimer - Time.deltaTime;

        // Reducción del tiempo de enfríamiento del señuelo.
        if (baitTimer > 0) baitTimer = baitTimer - Time.deltaTime;

        // Aumento de la estamina.
        if (isHeld == false && stamina < 100)
        {
            stamina = stamina + (Time.deltaTime * 8);
            stamina_bar.Regen();
        } 
    }

    // Colisiones con objetos varios.
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Spike")
        {
            Hurt(SPIKE_DAMAGE);
        }

        if (collider.gameObject.tag == "Bombs")
        {
            Hurt(BOMB_DAMAGE);
        }

        if (collider.gameObject.tag == "Enemigo")
        {
            Hurt(AMOGOS_DAMAGE);
        }

        if (collider.gameObject.tag == "Coin")
        {
            Take(1);
        }

        if (collider.gameObject.tag == "Botiquin")
        {
            Take(2);
        }

        if (collider.gameObject.tag == "Tienda")
        {
            isInStore = true;
            Shop();
        }

        if (collider.gameObject.tag == "Selection")
        {
            isInSelectionArea = true;
            Hardness();
        }

        if (collider.gameObject.tag == "Límites")
        {
            healthPoints = 0;
            isAlive = false;
        }

        if (collider.gameObject.tag == "HabilidadesDesbloqueadas")
        {
            isInTesting = true;
            highJumpState = true;
            baitState = true;
            high_jump_bar.Enable();
            decoy_bar.Enable();
        }
    }

    // Colisiones con objetos varios.
    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "Poison")
        {
            Hurt(POISON_DAMAGE);
        }
    }

    // Colisiones con objetos varios.
    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Tienda")
        {
            isInStore = false;
            Out();
        }

        if (collider.gameObject.tag == "Selection")
        {
            isInSelectionArea = false;
            OutSelection();
        }
    }
}
