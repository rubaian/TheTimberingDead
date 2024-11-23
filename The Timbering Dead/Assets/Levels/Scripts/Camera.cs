using UnityEngine;
public class Camera : MonoBehaviour
{
    [SerializeField] private float speed;
    private UnityEngine.Vector3 velocity=UnityEngine.Vector3.zero;
    [SerializeField] private Transform player;

    private void Update()
    {
        transform.position=new UnityEngine.Vector3(player.position.x , transform.position.y, transform.position.z);
    }
}