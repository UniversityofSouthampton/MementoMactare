using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance;
    //Set up PlayerHealth as a singleton
    //(https://techhub.wsagames.com/guides//1_Unity/1_Coding/1_Advanced-CSharp#programming-design-patterns)
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] private int maxHealth;
    private int health;
    
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        health = maxHealth;
    }

    public void InflictDamage(int amount)
    {
        //sets health to the maximum between (health - amount) but not below zero
        health = Mathf.Max(0,health-amount);
        
        UpdateHealthUI();
        
        if (health == 0) GameOverSequence();
    }

    public void ResetHealth()
    {
        health = maxHealth;
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        
    }

    private void GameOverSequence()
    {
        
    }
}
