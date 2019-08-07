using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField]
    private float _expRadius;
    [SerializeField]
    private float _expForce;
    [SerializeField]
    private LayerMask _layerMask;

    private bool _canExplode;

    public GameObject BOOM; // Объект, который спаунится после взрыва
    private GameObject CreateBoom;

    private void Start()
    {
        //Активируем мину через 2 сек
        Invoke("CanExplode", 0.7f);
    }

    private void CanExplode()
    {
        _canExplode = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Если не активирована - не взрываем
        if (!_canExplode)
            return;

        //Если не касается нужных слоев - не взрываем
        if (!collision.otherCollider.IsTouchingLayers(_layerMask))
            return;

        //Собираем все цели в массив
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, _expRadius, _layerMask);

        foreach (var t in targets)
        {
            //Направление взрывной волны для цели
            var expDir = (t.transform.position - transform.position).normalized;
            t.GetComponent<Rigidbody2D>().AddForce(expDir * _expForce, ForceMode2D.Impulse);
        }
        Destroy(gameObject);
        CreateBoom = Instantiate(BOOM, transform.position, transform.rotation); // Спауним объект, который симулирует взрыв
        Destroy(CreateBoom, 0.1f);
    }
}
