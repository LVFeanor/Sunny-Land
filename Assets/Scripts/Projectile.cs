using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameObject BOOM; // Объект, который спаунится после взрыва
    private GameObject CreateBoom;
    public int Damage; // Урон, который мы нанесём
    public float Speed, LifeTime; // Скорость и время жизни снаряда
    Vector3 Dir = new Vector3(0, 0, 0); // Направление полёта

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Dir.x = Speed * transform.right.x; // Говорим, что всегда летим по оси х и в сторону поворота снаряда
        Destroy(gameObject, LifeTime); // Говорим, что этот объект уничтожится через установленное время
    }

    void FixedUpdate()
    {
        transform.position += Dir; // Движение снаряда
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy") // Если объект с которым мы столкнулись имеет тэг Enemy
        {
            MyEnemy enemy = collision.GetComponent<MyEnemy>();
            if (enemy != null)
            {
                enemy.Hurt(Damage); // Вызываем метод урона и говорим сколько урона
                CreateBoom = Instantiate(BOOM, transform.position, transform.rotation); // Спауним объект, который симулирует взрыв
                Destroy(gameObject); // Уничтожаем ракету
                Destroy(CreateBoom, 0.1f);
            }
        }

        // Если объект с которым мы столкнулись имеет layer Ground
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            CreateBoom = Instantiate(BOOM, transform.position, transform.rotation); // Спауним объект, который симулирует взрыв      
            Destroy(gameObject);// Уничтожаем пулю
            Destroy(CreateBoom, 0.1f);
        }
    }
}
