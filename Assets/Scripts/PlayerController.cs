using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Player;
    private GameManager _gameManager;

    [SerializeField]
    private GameObject bombPrefab;

    [HideInInspector]
    public bool jump = false; // Условие, определющие должен ли игрок прыгать
    private Rigidbody2D rb;
    private Transform groundCheck; // Позиция, определяющая нхождение игрока на земле
    private bool grounded = false; // Проверка находится ли игрок на земле

    public float MoveForce = 3f; // Сила начальной скорости
    public float MaxSpeed = 2.5f; // Максимальная скорость  
    public float jumpForce = 3f; // Сила прыжка
    public bool IsForward = true; // Направление трансформа
    Vector3 Direction = new Vector3(1, 0, 0); // Vector right
    public GameObject Bullet, StartBullet; // Наш снаряд которым будем стрелять и точка, где он создаётся
    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthSlider;

    private Animator anim;
    bool isDead;
    bool damaged;

    void Awake()
    {
        Player = this;
        groundCheck = transform.Find("groundCheck");
        rb = GetComponent<Rigidbody2D>(); // Получаем компонент 
        anim = GetComponent<Animator>();
        _gameManager = FindObjectOfType<GameManager>();
        currentHealth = startingHealth;
    }

    void Update()
    {
        //Нажали ПКМ - бросили бомбу
        if (Input.GetMouseButtonUp(1))
            ThrowBomb();

        if (Input.GetButtonDown("Fire1")) // Если нажата кнопка выстрела
        {
            var bullet = Instantiate(Bullet, StartBullet.transform.position, StartBullet.transform.rotation); // Создаём Bullet в точке StartBullet
            bullet.transform.right *= Direction.x;
        }

        anim.SetFloat("Velocity", rb.velocity.x);
    }

    private void ThrowBomb()
    {
        Instantiate(bombPrefab, StartBullet.transform.position, StartBullet.transform.rotation);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthSlider.value = currentHealth;
        anim.SetTrigger("Hurt");
        if (currentHealth <= 0 && !isDead)
            Die();
    }

    private void Die()
    {
        isDead = true;
        // anim.SetTrigger("Die");
       
        //_gameManager.ShowLosePanel();
        //Destroy(gameObject);
    }

    void FixedUpdate()
    {
        // Игрок на земле если groundCheck находится на слое земли
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        // Если кнопка прыжка нажата и игрок на земле, он должен прыгнуть
        if (Input.GetButton("Jump") && grounded)
        {
            jump = true;
        }

        float h = Input.GetAxis("Horizontal"); // Ловим нажатие на кнопки отвечающие за движение по горизонтали

        //Если скорость меньше максимальной ускоряем игрока с помощью AddForce
        if (h * rb.velocity.x < MaxSpeed)
            rb.AddForce(Vector2.right * h * MoveForce);

        //Если скорость больше максимальной - уменьшаем до максимальной
        if (Mathf.Abs(rb.velocity.x) > MaxSpeed)
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * MaxSpeed, rb.velocity.y);

        //Если нужно, прыгаем
        if (jump)
        {
            rb.AddForce(new Vector2(0f, jumpForce));
            jump = false;
        }

        if (rb.velocity.x < 0 && IsForward) // Поворот
            Flip();
        if (rb.velocity.x > 0 && !IsForward)
            Flip();


    }

    void Flip()
    {
        IsForward = !IsForward;
        Direction.x *= -1f;

        Vector3 v = transform.localScale;
        v.x *= -1;
        transform.localScale = v;
    }
}
