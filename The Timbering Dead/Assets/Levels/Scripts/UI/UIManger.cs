using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioClip gameOverSound;
    private AudioSource audioSource; // مصدر الصوت

    private void Awake()
    {
        gameOverScreen.SetActive(false); // تأكد من إخفاء شاشة الموت عند البداية
        audioSource = GetComponent<AudioSource>(); // الحصول على مكون AudioSource
    }

    // تفعيل شاشة الموت
    public void GameOver()
    {
        Debug.Log("Game Over screen activated!"); // تأكيد تفعيل شاشة الموت
        gameOverScreen.SetActive(true);
        
        // تشغيل الصوت
        if (gameOverSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(gameOverSound); // تشغيل صوت الموت
        }
    }

    // إعادة تشغيل المستوى الحالي
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // العودة إلى القائمة الرئيسية
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    // الخروج من اللعبة
    public void Quit()
    {
        Application.Quit(); // الخروج من اللعبة (يعمل فقط في البناء)

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // الخروج من وضع اللعب في المحرر
        #endif
    }
}