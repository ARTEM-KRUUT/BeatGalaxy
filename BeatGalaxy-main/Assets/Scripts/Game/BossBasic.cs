using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBasic : MonoBehaviour
{
    public int maxHealth = 50;
    public int currentHealth;
    public float moveSpeed;
    private const float TIMER_MAX_TIME = 1f; //����� �������
    private float timerCurrentTime = TIMER_MAX_TIME;

    public HealthBar healthBarPrefab;
    public HealthBar healthBar;
    public PlanetHealth planetHealth;

    private int damage_planet = 1;
    public int attackDamage = 1;
    public int EnragedAttackDamage = 10;


    public float attackRange;
    public LayerMask attackMask;


    public Animator animator;
    private bool planetDestroyed = false;
    public float attackDuration; // ������������ �����
    private float attackTimer = 0f; // ������ ��� �������� ������������ �����
    private bool isAttacking = false; // ���� ��� �����������, ����������� �� ����� � ������ ������

    public void Attack()
    {
        if (!isAttacking && !planetDestroyed)
        {
            isAttacking = true;
            attackTimer = attackDuration;

            // ���������� �������� �����
            moveSpeed = 0;

            // ��������� �������� �����
            animator.SetTrigger("BasicAttack");
            GameObject planet = GameObject.FindWithTag("Planet");
            planet.GetComponent<PlanetHealth>().Damage(attackDamage);

            if (planet.GetComponent<PlanetHealth>().pl_health <= 0)
            {
                planetDestroyed = true; // ������������� ����, ��� ������� ����������
            }
        }
 
    }
    public void EnragedAttack()
    {
        if (!isAttacking && !planetDestroyed)
        {
            isAttacking = true;
            attackTimer = attackDuration;

            // ���������� �������� �����
            moveSpeed = 0;

            // ��������� �������� �����
            animator.SetTrigger("BasicAttack");
            GameObject planet = GameObject.FindWithTag("Planet");
            planet.GetComponent<PlanetHealth>().Damage(EnragedAttackDamage);

            if (planet.GetComponent<PlanetHealth>().pl_health <= 0)
            {
                planetDestroyed = true; // ������������� ����, ��� ������� ����������
            }
        }
    }
    void Start()
    {
        currentHealth = maxHealth;
      //  healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {

        if (isAttacking)
        {
            // ��������� ������ �����
            attackTimer -= Time.deltaTime;

            // ���� ������ ����� �����, ��������� �����
            if (attackTimer <= 0f)
            {
                isAttacking = false;

                // ������������ �������� �����
                
            }
        }
        else if(!planetDestroyed)
        {
            // ������� �����, ������ ���� �� �� �������
            if (timerCurrentTime > 0)
            {
                timerCurrentTime -= Time.deltaTime;
            }
            else
            {
                timerCurrentTime = TIMER_MAX_TIME;
                Move();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(2);
        }

    }
    //Moving forward
    void Move()
    {
        transform.Translate(-transform.up * moveSpeed);
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("damage");
        if (collider.gameObject.name == "Planet")
        {
            Debug.Log("BOOM");
            planetHealth.Damage(damage_planet);
        }

        if (collider.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("BOOM");
            LoseHealth(1);
        }
    }



    public void LoseHealth(int damage)
    {
        currentHealth -= damage;

        //  healthBar.SetHealth(currentHealth);

        if (currentHealth <= 25)
        {
            GetComponent<Animator>().SetBool("isEnraged", true);
        }
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

      //  healthBar.SetHealth(currentHealth);

        if (currentHealth <= 25)
        {
            GetComponent<Animator>().SetBool("isEnraged", true);
        }
        if (currentHealth <= 0) 
        {
            Die();
        }
    }
    void Die()
    {
        Destroy(gameObject);
    }
}
