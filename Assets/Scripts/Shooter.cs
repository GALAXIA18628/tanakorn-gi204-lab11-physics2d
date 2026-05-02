using UnityEngine;
using UnityEngine.InputSystem;

public class Shooter : MonoBehaviour
{
    public Transform shootPoint;
    public GameObject target;
    public Rigidbody2D bulletPrefab;

    void Start()
    {
        Cursor.visible = false;

    }

    void Update()
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        worldPos.z = 0;
        target.transform.position = worldPos;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(screenPos);
            Debug.DrawRay(ray.origin, ray.direction * 5, Color.red, 5f);

            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (hit.collider != null)
            {
                //target.transform.position = new Vector2(hit.point.x, hit.point.y);
                Debug.Log($"Hit {hit.collider.gameObject.name}");
                
                //หา velocity ที่จะทำให้กระสุนไปถึงจุดที่ยิง
                Vector2 projectileVelocity = CalculateProjectileValocity(shootPoint.position, hit.point, 1f);
                
                //เสกกระสุนและยิงออกไปด้วย velocity ที่คำนวณได้
                Rigidbody2D shootBullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
                shootBullet.linearVelocity = projectileVelocity;
            }
        }
    }

    Vector2 CalculateProjectileValocity(Vector2 origin, Vector2 target, float time)
    {
        Vector2 direction = target - origin;
        return new Vector2(direction.x / time, (direction.y / time) + 0.5f * Mathf.Abs(Physics2D.gravity.y) * time);
    }
}
