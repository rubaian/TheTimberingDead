using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioClip gameOverSound;
    private AudioSource audioSource;

    private void Awake()
    {
        gameOverScreen.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    // تفعيل شاشة الموت
    public void GameOver()
    {
        Debug.Log("Game Over screen activated!");
        gameOverScreen.SetActive(true);

        // تشغيل الصوت
        if (gameOverSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(gameOverSound);
        }

        // تعطيل SceneController
        if (SceneController.instance != null)
        {
            SceneController.instance.SetActive(false); // تعطيل الكود الثاني
        }

        // إيقاف أي رسوم متحركة أو عمليات أخرى
        StopAllCoroutines();
    }

    // إعادة تشغيل المستوى الحالي
    public void Restart()
    {
        // تفعيل SceneController مرة أخرى
        if (SceneController.instance != null)
        {
            SceneController.instance.SetActive(true); // تفعيل الكود الثاني
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // العودة إلى القائمة الرئيسية
    public void MainMenu()
    {
        // تفعيل SceneController مرة أخرى
        if (SceneController.instance != null)
        {
            SceneController.instance.SetActive(true); // تفعيل الكود الثاني
        }
        SceneManager.LoadScene(0);
    }

    // الخروج من اللعبة
    public void Quit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}