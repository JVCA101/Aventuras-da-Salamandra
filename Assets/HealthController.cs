using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    private float maxHealth = 3f;
    private float health = 3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0f)
        {
            Debug.Log("You died!");
        }

        if(Input.GetKeyDown(KeyCode.Delete))
        {
            TakeDamage(1f);
        }
        if(Input.GetKeyDown(KeyCode.Z))
            Heal(1f);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.fillAmount = health / maxHealth;
    }

    public void Heal(float heal)
    {
        health += heal;
        health = Mathf.Clamp(health, 0f, maxHealth);
        healthBar.fillAmount = health / maxHealth;
    }
}
