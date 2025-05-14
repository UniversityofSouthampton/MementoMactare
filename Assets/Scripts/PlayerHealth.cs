using UnityEngine;
using UnityEngine.SceneManagement;
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

    private bool inGameOverSequence = false;
    
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
        else
        {
            healthSymbol.color = new Color(0.4549f, 0f, 0f, 1);
        }
    }

    private void GameOverSequence()
    {
        //If already in game over sequence, cancel further code
        //This to prevent a scenario where button spam tries to load game over scene multiple times
        if (inGameOverSequence) return;
        
        inGameOverSequence = true;
        PlayerAnimatorManager.instance.PlayAttackAnimation("Defeat");
        AudioManager.instance.PlaySound("Player Defeated", volume: 0.5f);
        
        //After one second, run "GoToGameOverScene" method
        Invoke("GoToGameOverScene", 1);
    }
    void GoToGameOverScene()
    {
        SceneManager.LoadSceneAsync("Game Over");
        AudioManager.instance.PlaySound("Game Over", volume: 0.5f);
        inGameOverSequence = false;
    }
}
