using UnityEngine;
using UnityEngine.UI;

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
    private int _health;
    [SerializeField] private Image healthSymbol;
    [SerializeField] private Image healthBarFill;
    
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        _health = maxHealth;
    }

    public void InflictDamage(int amount)
    {
        //sets _health to the maximum between (_health - amount) but not below zero
        _health = Mathf.Max(0,_health-amount);
        
        UpdateHealthUI();
        
        if (_health == 0) GameOverSequence();
    }

    public void ResetHealth()
    {
        _health = maxHealth;
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        healthBarFill.fillAmount = 1.0f / maxHealth * _health;
        if (_health == 0)
        {
            healthSymbol.color = new Color(0.2627f, 0.2627f, 0.2627f, 1);
        }
    }

    private void GameOverSequence()
    {
        
    }
}
