using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel; // نافذة الإعدادات
    [SerializeField] private Slider volumeSlider; // شريط التحكم في الصوت

    private void Start()
{
    // تأكد من أن settingsPanel غير مفعل عند بدء اللعبة
    settingsPanel.SetActive(false); // تأكد من أن نافذة الإعدادات مخفية

    // تحقق من أن volumeSlider معين
    if (volumeSlider == null)
    {
        Debug.LogError("Volume Slider is not assigned in the inspector!");
        return;
    }

    // استرجاع مستوى الصوت المخزن مسبقًا
    float savedVolume = PlayerPrefs.GetFloat("Volume", 0.5f);
    volumeSlider.value = savedVolume;
    AudioListener.volume = savedVolume; // ضبط مستوى الصوت العام

    // ربط تغيير شريط الصوت بتعديل مستوى الصوت
    volumeSlider.onValueChanged.AddListener(ChangeVolume);
}
    // بدء اللعبة
    public void StartGame()
    {
        SceneManager.LoadScene("Level 1");
    }

    // فتح قائمة الإعدادات
    public void OpenSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
        else
            Debug.LogError("Settings Panel is not assigned!");
    }

    // إغلاق قائمة الإعدادات
    public void CloseSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
        else
            Debug.LogError("Settings Panel is not assigned!");
    }

    // تغيير مستوى الصوت
    public void ChangeVolume(float volume)
    {
        AudioListener.volume = volume; // ضبط مستوى الصوت العام في اللعبة
        PlayerPrefs.SetFloat("Volume", volume); // حفظ الإعداد
        PlayerPrefs.Save();
    }

    // إنهاء اللعبة
    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}