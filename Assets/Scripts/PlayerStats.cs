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
            GlobalEventManager.OnGameOver.Invoke("health");
        }
        if (currentExhaustion <= 0)
        {
            GlobalEventManager.OnGameOver.Invoke("exhaustion");
        }
        if (currentHunger <= 0)
        {
            GlobalEventManager.OnGameOver.Invoke("hunger");
        }
    }
    private void Awake()
    {
        GlobalEventManager.ApplyDamage.AddListener(RecieveDmg);
        GlobalEventManager.ApplyHeal.AddListener(HealHealth);
        GlobalEventManager.ApplyExhaustion.AddListener(ApplyExhaustion);
        GlobalEventManager.ResetExhaustion.AddListener(ResetExhaustion);
        GlobalEventManager.ApplyHunger.AddListener(ApplyHunger);
        GlobalEventManager.HealHunger.AddListener(HealHunger);
        GlobalEventManager.OnRespawn.AddListener(Respawn);
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
        GlobalEventManager.UpdateHealthBar.Invoke(currentHealth);
    }

    private void HealHealth(int healAmount)
    {
        currentHealth = currentHealth + healAmount;
        GlobalEventManager.UpdateHealthBar.Invoke(currentHealth);
    }

    private void ApplyExhaustion(int exhaustionValue)
    {
        currentExhaustion = currentExhaustion - exhaustionValue;
        GlobalEventManager.UpdateExhaustionBar.Invoke(currentExhaustion);
    }

    private void ApplyHunger(int hungerValue)
    {
        currentHunger = currentHunger - hungerValue;
        GlobalEventManager.UpdateHungerBar.Invoke(currentHunger);
    }

    private void HealHunger(int hungerValue)
    {
        currentHunger = currentHunger + hungerValue;
        GlobalEventManager.UpdateHungerBar.Invoke(currentHunger);
    }

    private void HealFully()
    {
        currentHealth = maxHealth;
        GlobalEventManager.UpdateHealthBar.Invoke(currentHealth);
    }

    private void ResetHunger()
    {
        currentHunger = maxHunger;
        GlobalEventManager.UpdateHungerBar.Invoke(currentHunger);
    }

    private void ResetExhaustion()
    {
        currentExhaustion = maxExhaustion;
        GlobalEventManager.UpdateExhaustionBar.Invoke(currentExhaustion);
    }

    void Respawn()
    {
        ResetHunger();
        ResetExhaustion();
        HealFully();
    }
}
