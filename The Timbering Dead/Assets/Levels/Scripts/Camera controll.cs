using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float speed;
    private Vector3 velocity = Vector3.zero;
    [SerializeField] private Transform player;
    private bool isCameraActive = true; // للتحكم في حركة الكاميرا

    private void Update()
    {
        if (isCameraActive)
        {
            // الكاميرا تتبع اللاعب، مع الحفاظ على نفس المسافة على المحور Z
            transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
        }
    }

    public void StopCamera()
    {
        isCameraActive = false; // تعطيل حركة الكاميرا
    }

    public void StartCamera()
    {
        isCameraActive = true; // تفعيل حركة الكاميرا
    }
}