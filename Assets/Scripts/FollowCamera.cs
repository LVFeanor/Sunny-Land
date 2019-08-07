using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [Header("Мин и макс границы движения по Х")]
    public float MinX = 0f;
    public float MaxX = 10f;

    // Чтобы избежать рывков, двигаем камеру в LateUpdate, когда всё остальное на сцене уже завершило движение
    private void LateUpdate()
    {
        // Если игрока нет на сцене, не двигаемся
        if (!PlayerController.Player)
            return;

        // Получаем нужнное значение X в границах от MinX до MaxX
        var posX = Mathf.Clamp(PlayerController.Player.transform.position.x, MinX, MaxX);

        // Двигаем позицию камеры
        transform.position = new Vector3(posX, 0, -10);

    }
}
