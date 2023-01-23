using UnityEngine;

/// <summary>
/// 	Clase para gestionar la GUI.
/// </summary>
public class Player : MonoBehaviour
{
    /// <summary>
    /// 	Atributos para definir la vida actual y máxima.
    /// </summary>
    public int health, max_health;

    /// <summary>
    /// 	Variables para manejar la cantidad de monedas y botiquines.
    /// </summary>
    public int coins, medkits;

    /// <summary>
    /// 	Atributo para manejar la barra de vida.
    /// </summary>
    public HealthBar healthbar;

    /// <summary>
    /// 	Atributo para manejar la barra de estamina.
    /// </summary>
    public StaminaBar stamina;

    /// <summary>
    /// 	Atributo para manejar la barra de salto.
    /// </summary>
    public HighJumpBar high_jump_bar;

    /// <summary>
    /// 	Atributo para manejar la barra de copia.
    /// </summary>
    public Decoy decoy;

    /// <summary>
    /// 	Atributo para manejar la cartera.
    /// </summary>
    public CoinCounter coin_counter;

    /// <summary>
    /// 	Atributo para manejar los botiquines.
    /// </summary>
    public MedKitCounter medkit_counter;

    /// <summary>
    /// 	Configuraciones iniciales.
    /// </summary>
    void Start()
    {
        max_health = 200;
        health = 200;
        coins = 0;

        healthbar.SetMaxHealth(max_health);
        stamina.SetMaxStamina(100);
        high_jump_bar.SetHighJumpCooldownTime(4);
        high_jump_bar.Enable();
        decoy.SetDecoyCooldownTime(6);
        decoy.Enable();
        coin_counter.SetCoins(0);
        medkit_counter.SetMedKits(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) TakeDamage(20);
        if (Input.GetKey(KeyCode.LeftShift)) stamina.Reduce();
        else stamina.Regen();

        if (Input.GetKeyDown(KeyCode.E) && high_jump_bar.Ready()) high_jump_bar.Use();

        if (Input.GetKeyDown(KeyCode.Q) && decoy.Ready()) decoy.Use();

        if (Input.GetKeyDown(KeyCode.C))
        {
            coins++;
            coin_counter.SetCoins(coins);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            medkits--;
            medkit_counter.SetMedKits(medkits);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            medkits++;
            medkit_counter.SetMedKits(medkits);
        }
    }

    /// <summary>
    /// 	Método para disminuir la vida actual.
    /// </summary>
    void TakeDamage(int damage)
    {
        health -= damage;
        healthbar.SetHealth(health);
    }
}
