using UnityEngine;
using UnityEngine.UI;

public class SelectionArrow : MonoBehaviour
{
    [SerializeField] private RectTransform[] buttons; // مصفوفة تحتوي على جميع الأزرار
    [SerializeField] private AudioClip changeSound;
    [SerializeField] private AudioClip interactSound;
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
        // تغيير موقع السهم بناءً على ضغطات الأسهم
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            ChangePosition(-1);
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            ChangePosition(1);

        // التفاعل مع الزر المحدد عند الضغط على Enter أو E
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.E))
            Interact();
    }

    private void ChangePosition(int _change)
    {
        currentPosition += _change;

        // التأكد من عدم الخروج عن حدود المصفوفة
        if (currentPosition < 0)
            currentPosition = buttons.Length - 1;
        else if (currentPosition >= buttons.Length)
            currentPosition = 0;

        Debug.Log("Current Position: " + currentPosition); // تتبع الموضع الحالي للسهم

        AssignPosition();
    }

    private void AssignPosition()
    {
        // التأكد من أن السهم يتحرك إلى الموضع الصحيح
        if (buttons.Length > 0 && buttons[currentPosition] != null)
        {
            arrow.position = new Vector3(arrow.position.x, buttons[currentPosition].position.y);
        }
        else
        {
            Debug.LogWarning("Button at position " + currentPosition + " is missing!");
        }
    }

    private void Interact()
    {
        // التأكد من أن الزر يحتوي على مكون Button قبل محاولة الضغط عليه
        Button button = buttons[currentPosition].GetComponent<Button>();
        if (button != null)
        {
            button.onClick.Invoke();
        }
        else
        {
            Debug.LogWarning("No Button component found on: " + buttons[currentPosition].name);
        }
    }
}
