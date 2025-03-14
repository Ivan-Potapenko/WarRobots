using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game {

    public class WeaponCone : MonoBehaviour {

        [Header("��������� ��������")]
        public float range = 10f;       // ��������� ��������
        public float widthNear = 2f;    // ������ �������� ����
        public float widthFar = 6f;     // ������ �������� ����

        void OnDrawGizmos() {
            Gizmos.color = Color.green;

            // ����������� �����
            Vector3 forward = transform.forward;
            Vector3 right = transform.right;

            // ����������� ����� ��������
            Vector3 start = transform.position;
            Vector3 nearLeft = start - right * (widthNear / 2);
            Vector3 nearRight = start + right * (widthNear / 2);
            Vector3 farLeft = start + forward * range - right * (widthFar / 2);
            Vector3 farRight = start + forward * range + right * (widthFar / 2);

            // ������ ��������
            Gizmos.DrawLine(nearLeft, nearRight);
            Gizmos.DrawLine(nearLeft, farLeft);
            Gizmos.DrawLine(nearRight, farRight);
            Gizmos.DrawLine(farLeft, farRight);
        }

        // �������� ��������� ����� ������ ��������
        public bool IsPointInTrapezoid(Vector3 point) {
            Vector3 forward = transform.forward;
            Vector3 right = transform.right;

            Vector3 start = transform.position;
            Vector3 dirToPoint = point - start;
            dirToPoint.y = 0; // ���������� ������

            float dist = Vector3.Dot(dirToPoint, forward); // �������� ����� �� ��� Z
            if (dist < 0 || dist > range) return false; // �� ��������� ���������

            // ������������� ������ � ����������� �� ����������
            float currentWidth = Mathf.Lerp(widthNear, widthFar, dist / range) / 2;

            float sideOffset = Vector3.Dot(dirToPoint, right); // �������� �� ��� X
            return Mathf.Abs(sideOffset) <= currentWidth;
        }
    }
}