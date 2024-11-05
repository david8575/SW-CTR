using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obtuseTriangleEnemy : MonoBehaviour
{
    public float speed = 1.0f; 
    public float detectionRange = 5.0f; 
    public float chargeSpeed = 4.0f; 
    public int maxHealth = 2; 

    private Transform player;
    private int currentHealth;
    private bool isAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive)
            return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        if (distanceToPlayer <= detectionRange) {
            transform.position = Vector2.MoveTowards(transform.position, player.position, chargeSpeed * Time.deltaTime);
        }
        else {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (transform.position.x > 3.0f || transform.position.x < -3.0f) {
                speed = -speed; 
            }
        }
    }

    public void TakeDamage(){
        currentHealth--;
        if (!isAlive){
            return;
        }

        if (currentHealth <= 0){
            isAlive = false;
            Destroy(gameObject); 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.CompareTag("Player")){
            TakeDamage();
        }
    }
}
