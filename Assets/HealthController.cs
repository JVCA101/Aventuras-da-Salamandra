using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthController : MonoBehaviour
{
    [SerializeField] private Image[] hearts;
    [SerializeField] private float damageCooldown = 1.5f;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;
    [SerializeField] private bool isPlayer1 = true;

    public int health = 3;
    private int numOfHearts = 3;
    private float timeDmg;
    private bool gotDamaged;
    private AudioSource damageTakenSound;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        health = numOfHearts;
        timeDmg = 0f;
        gotDamaged = false;
        damageTakenSound = GetComponents<AudioSource>()[0];
        if(PlayerPrefs.GetInt("player2") == 0 && !isPlayer1){
            for(int i =0; i < numOfHearts; i++)
                hearts[i].color = new Color(1f, 1f, 1f, 0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < numOfHearts; i++)
            if(i < health)
                hearts[i].sprite = fullHeart;
            else
                hearts[i].sprite = emptyHeart;
        
        if(gotDamaged)
            timeDmg += Time.deltaTime;
        if(timeDmg >= damageCooldown){
            timeDmg = 0f;
            spriteRenderer.color = originalColor;
            gotDamaged = false;
        }
    }

    private void TakeDamage()
    {
        Debug.Log("Colidiu");
        damageTakenSound.Play();
        health--;
        if(health<=0){
            SceneManager.LoadScene("DeathScene");
        }
        spriteRenderer.color = new Color(1f, 1f, 1f, 0.9f);
        gotDamaged = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && !gotDamaged)
        {
            TakeDamage();
        }
    }
}
