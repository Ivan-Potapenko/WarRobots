using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game {

    public class WeaponCone : MonoBehaviour {

        [Header("Настройки трапеции")]
        public float range = 10f;       // Дальность трапеции
        public float widthNear = 2f;    // Ширина ближнего края
        public float widthFar = 6f;     // Ширина дальнего края

        void OnDrawGizmos() {
            Gizmos.color = Color.green;

            // Направление вперёд
            Vector3 forward = transform.forward;
            Vector3 right = transform.right;

            // Определение точек трапеции
            Vector3 start = transform.position;
            Vector3 nearLeft = start - right * (widthNear / 2);
            Vector3 nearRight = start + right * (widthNear / 2);
            Vector3 farLeft = start + forward * range - right * (widthFar / 2);
            Vector3 farRight = start + forward * range + right * (widthFar / 2);

            // Рисуем трапецию
            Gizmos.DrawLine(nearLeft, nearRight);
            Gizmos.DrawLine(nearLeft, farLeft);
            Gizmos.DrawLine(nearRight, farRight);
            Gizmos.DrawLine(farLeft, farRight);
        }

        // Проверка попадания точки внутрь трапеции
        public bool IsPointInTrapezoid(Vector3 point) {
            Vector3 forward = transform.forward;
            Vector3 right = transform.right;

            Vector3 start = transform.position;
            Vector3 dirToPoint = point - start;
            dirToPoint.y = 0; // Игнорируем высоту

            float dist = Vector3.Dot(dirToPoint, forward); // Проекция точки на ось Z
            if (dist < 0 || dist > range) return false; // За пределами дальности

            // Интерполируем ширину в зависимости от расстояния
            float currentWidth = Mathf.Lerp(widthNear, widthFar, dist / range) / 2;

            float sideOffset = Vector3.Dot(dirToPoint, right); // Проекция на ось X
            return Mathf.Abs(sideOffset) <= currentWidth;
        }
    }
}