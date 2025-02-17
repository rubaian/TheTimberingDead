using UnityEngine;
using UnityEngine.UI;

public class SelectionArrow : MonoBehaviour
{
    [SerializeField] private RectTransform[] buttons;
    [SerializeField] private AudioClip changeSound;
    [SerializeField] private AudioClip interactSound;
    [SerializeField] private AudioSource audioSource;
    private RectTransform arrow;
    private int currentPosition;

    private void Awake()
    {
        arrow = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        currentPosition = 0;
        ChangePosition(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            ChangePosition(-1);
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            ChangePosition(1);

        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.E))
            Interact();
    }

    private void ChangePosition(int _change)
    {
        currentPosition += _change;

        if (currentPosition < 0)
            currentPosition = buttons.Length - 1;
        else if (currentPosition >= buttons.Length)
            currentPosition = 0;

        AssignPosition();
        PlaySound(changeSound);
    }

    private void AssignPosition()
    {
        if (buttons == null || buttons.Length == 0 || buttons[currentPosition] == null)
        {
            Debug.LogError("Buttons array is not set or contains null elements!");
            return;
        }
        arrow.position = new Vector3(arrow.position.x, buttons[currentPosition].position.y);
    }

    private void Interact()
    {
        if (buttons[currentPosition] == null)
        {
            Debug.LogError("Button reference is missing!");
            return;
        }

        Button button = buttons[currentPosition].GetComponent<Button>();
        if (button != null)
        {
            button.onClick.Invoke();
            PlaySound(interactSound);
        }
        else
        {
            Debug.LogWarning("No Button component found on: " + buttons[currentPosition].name);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}