
// using UnityEngine;
// public class Bullet : MonoBehaviour
// {
//     public float Speed = 20f;
//     public float Damage = 10f;
//     public float Range = 5f;
//     public Vector3 TargetPosition { get; set; }

//     void Start()
//     {
//         Destroy(gameObject, Range / Speed);
//     }

//     void Update()
//     {
//         transform.position = Vector3.MoveTowards(transform.position, TargetPosition, Speed * Time.deltaTime);
//     }

//     void OnTriggerEnter(Collider other)
//     {
//         if (other.CompareTag("monster"))
//         {
//             Monster monster = other.GetComponent<Monster>();
//             if (monster?.Health != null)
//             {
//                 monster.Health -= Damage;
//             }
//             Destroy(gameObject);
//         }
//     }
//     public void Seek(Transform target, float range)
//     {
//         TargetPosition = target.position;
//         Range = range;
//     }
// }