using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    [SerializeField] Animator transitionAnim;

    private bool isActive = true; // متغير للتحكم في تشغيل أو إيقاف الكود

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // دالة لتعطيل أو تفعيل الكود
    public void SetActive(bool active)
    {
        isActive = active;
    }

    public void NextLevel()
    {
        if (isActive) // التحقق من أن الكود نشط قبل التنفيذ
        {
            StartCoroutine(LoadLevel());
        }
    }

    IEnumerator LoadLevel()
    {
        if (isActive) // التحقق من أن الكود نشط قبل التنفيذ
        {
            transitionAnim.SetTrigger("End");
            yield return new WaitForSeconds(1);
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
            transitionAnim.SetTrigger("Start");
        }
    }
}