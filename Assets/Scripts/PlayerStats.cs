using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] public int maxHealth;
    [SerializeField] public int currentHealth;

    [SerializeField] public int maxHunger;
    [SerializeField] public int currentHunger;
    [SerializeField] public int maxExhaustion;
    [SerializeField] public int currentExhaustion;

    void Update()
    {
        if (currentHealth >= maxHealth) { currentHealth = maxHealth; }
        if (currentHunger >= maxHunger) { currentHunger = maxHunger; }
        if (currentExhaustion >= maxExhaustion) { currentExhaustion = maxExhaustion; }

        if (currentHealth <= 0)
        {
            EventManager.OnGameOver.Invoke("health");
        }
        if (currentExhaustion <= 0)
        {
            EventManager.OnGameOver.Invoke("exhaustion");
        }
        if (currentHunger <= 0)
        {
            EventManager.OnGameOver.Invoke("hunger");
        }
    }
    private void Awake()
    {
        EventManager.ApplyDamage.AddListener(RecieveDmg);
        EventManager.ApplyHeal.AddListener(HealHealth);
        EventManager.ApplyExhaustion.AddListener(ApplyExhaustion);
        EventManager.ResetExhaustion.AddListener(ResetExhaustion);
        EventManager.ApplyHunger.AddListener(ApplyHunger);
        EventManager.HealHunger.AddListener(HealHunger);
        EventManager.OnRespawn.AddListener(Respawn);
    }
    private void Start()
    {
        HealFully();
        ResetExhaustion();
        ResetHunger();
    }
    
    private void RecieveDmg(int damage)
    {
        currentHealth = currentHealth - damage;
        EventManager.UpdateHealthBar.Invoke(currentHealth);
    }

    private void HealHealth(int healAmount)
    {
        currentHealth = currentHealth + healAmount;
        EventManager.UpdateHealthBar.Invoke(currentHealth);
    }

    private void ApplyExhaustion(int exhaustionValue)
    {
        currentExhaustion = currentExhaustion - exhaustionValue;
        EventManager.UpdateExhaustionBar.Invoke(currentExhaustion);
    }

    private void ApplyHunger(int hungerValue)
    {
        currentHunger = currentHunger - hungerValue;
        EventManager.UpdateHungerBar.Invoke(currentHunger);
    }

    private void HealHunger(int hungerValue)
    {
        currentHunger = currentHunger + hungerValue;
        EventManager.UpdateHungerBar.Invoke(currentHunger);
    }

    private void HealFully()
    {
        currentHealth = maxHealth;
        EventManager.UpdateHealthBar.Invoke(currentHealth);
    }

    private void ResetHunger()
    {
        currentHunger = maxHunger;
        EventManager.UpdateHungerBar.Invoke(currentHunger);
    }

    private void ResetExhaustion()
    {
        currentExhaustion = maxExhaustion;
        EventManager.UpdateExhaustionBar.Invoke(currentExhaustion);
    }

    void Respawn()
    {
        ResetHunger();
        ResetExhaustion();
        HealFully();
    }
}
