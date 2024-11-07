using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class equilateralTriangleEnemy : MonoBehaviour
{
    public float maxHealth = 100.0f;
    public float speed = 1.0f;
    public float chargeSpeed = 4.0f;
    public GameObject miniTrianglePrefab;
    public GameObject shockwavePrefab;
    public GameObject laserPrefab;
    public GameObject rightTrianglePrefab;

    private float currentHealth;
    private Transform player;
    private bool isAlive = true;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(AttackRoutine()); 
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAlive){
            return;
        }
    }

    IEnumerator AttackRoutine(){
        while(isAlive){
            if (currentHealth > maxHealth * 0.8f){
                BasicAttack();
            }
            else if (currentHealth > maxHealth * 0.5f){
                ShockwaveAttack();
            }
            else if (currentHealth > maxHealth * 0.3f){
                LaserAttack();
            }
            else{
                SpawnRightTriangles();
            }

            yield return new WaitForSeconds(Random.Range(2f, 5f));
        }
    }

    void BasicAttack(){
        int attackType = Random.Range(0,3);

        switch (attackType){
            case 0:
                break;
            case 1:
                StartCoroutine(ChargeAttack());
                break;
            case 2:
                SummonMiniTriangles();
                break;
        }
    }

    IEnumerator ChargeAttack(){
        Vector2 chargeDirection = (player.position - transform.position).normalized;
        transform.position += (Vector3)chargeDirection * chargeSpeed * Time.deltaTime;
        yield return new WaitForSeconds(1f);
    }

    void SummonMiniTriangles(){
        Instantiate(miniTrianglePrefab, transform.position, Quaternion.identity);
    }

    void ShockwaveAttack(){
        Instantiate(shockwavePrefab, transform.position, Quaternion.identity);
        StartCoroutine(ChargeAttack());
    }

    void LaserAttack(){
        GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
        StartCoroutine(RotateLaser(laser));
    }

    IEnumerator RotateLaser(GameObject laser)
    {
        while (currentHealth <= maxHealth * 0.5f && currentHealth > maxHealth * 0.3f)
        {
            laser.transform.Rotate(Vector3.forward * 20 * Time.deltaTime);
            yield return null;
        }
        Destroy(laser);
    }

    void SpawnRightTriangles(){
        Vector3 spawnOffset = Vector3.up * 0.5f;
        Instantiate(rightTrianglePrefab, transform.position + spawnOffset, Quaternion.identity);
        Instantiate(rightTrianglePrefab, transform.position - spawnOffset, Quaternion.identity);
    }

    public void TakeDamage(float damage){
        currentHealth -= damage;
        if (currentHealth <= 0 && isAlive){
            isAlive = false;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.CompareTag("PlayerWeapon")){
            TakeDamage(10); 
        }
    }
}
