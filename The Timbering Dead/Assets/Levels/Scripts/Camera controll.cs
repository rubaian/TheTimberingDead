using UnityEngine;

public class PlayerCamera : MonoBehaviour  // Changed from Camera to PlayerCamera
{
    internal static object main;
    [SerializeField] private float speed;
    private UnityEngine.Vector3 velocity = UnityEngine.Vector3.zero;
    [SerializeField] private Transform player;

    private void Update()
    {
        // Camera follows the player, keeping the same distance on the Z-axis
        transform.position = new UnityEngine.Vector3(player.position.x, player.position.y, transform.position.z);
    }
}
