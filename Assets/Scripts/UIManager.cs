using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI remainingBlocksText;

    private void OnEnable()
    {
        LevelEditor.OnBlockCountChanged += UpdateRemainingBlocksText;
    }
    private void OnDisable()
    {
        LevelEditor.OnBlockCountChanged -= UpdateRemainingBlocksText;
    }

    private void UpdateRemainingBlocksText(int remainingBlocks)
    {
        remainingBlocksText.text = $"Remaining Blocks: {remainingBlocks}";
    }

}
