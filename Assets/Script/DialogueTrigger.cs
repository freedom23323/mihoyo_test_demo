using UnityEngine;
using TMPro; // 1. 引入 TextMeshPro 命名空间

public class DialogueTrigger : MonoBehaviour
{
    [System.Serializable]
    public struct DialogueLine
    {
        public string speakerName;   // 说话者的名字
        [TextArea(2, 5)] 
        public string text;          // 对话台词内容
    }

    [Header("UI 元素引用 (TextMeshPro)")]
    // 如果你的“按F提示”是CanvasUI，用 TextMeshProUGUI；如果是挂在场景物体上的3D字，用 TextMeshPro
    [SerializeField] private GameObject hintText;         
    [SerializeField] private GameObject dialogueCanvas;   // 整个对话框的UI父物体
    [SerializeField] private TextMeshProUGUI nameText;    // 2. 改为 TMP 专属的 UI 组件
    [SerializeField] private TextMeshProUGUI contentText; // 2. 改为 TMP 专属的 UI 组件

    [Header("对话内容设置")]
    [SerializeField] private DialogueLine[] dialogueLines; 

    private bool isPlayerInZone = false;  
    private bool isDialogActive = false;  
    private int currentLineIndex = 0;     

    void Start()
    {
        if (hintText != null) hintText.SetActive(false);
        if (dialogueCanvas != null) dialogueCanvas.SetActive(false);
    }

    void Update()
    {
        if (isPlayerInZone)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (!isDialogActive)
                {
                    OpenDialogue();
                }
                else
                {
                    DisplayNextLine();
                }
            }
        }
    }

    private void OpenDialogue()
    {
        if (dialogueLines.Length == 0) return;

        isDialogActive = true;
        currentLineIndex = 0;

        if (dialogueCanvas != null) dialogueCanvas.SetActive(true);
        if (hintText != null) hintText.SetActive(false);

        UpdateDialogueUI();
    }

    private void DisplayNextLine()
    {
        currentLineIndex++;

        if (currentLineIndex >= dialogueLines.Length)
        {
            CloseDialogue();
        }
        else
        {
            UpdateDialogueUI();
        }
    }

    private void UpdateDialogueUI()
    {
        // 赋值方式和以前一样，直接赋予 .text 即可
        if (nameText != null) 
            nameText.text = dialogueLines[currentLineIndex].speakerName;
            
        if (contentText != null) 
            contentText.text = dialogueLines[currentLineIndex].text;
    }

    private void CloseDialogue()
    {
        isDialogActive = false;
        if (dialogueCanvas != null) dialogueCanvas.SetActive(false);
        
        if (isPlayerInZone && hintText != null) 
        {
            hintText.SetActive(true);
        }
    }

    // ─── 范围检测 ───

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            if (!isDialogActive && hintText != null)
            {
                hintText.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
            CloseDialogue();
            if (hintText != null) hintText.SetActive(false);
        }
    }
}