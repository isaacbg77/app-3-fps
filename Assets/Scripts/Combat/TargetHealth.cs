using TMPro;
using UnityEngine;

public class TargetHealth : EnemyHealth
{
    [SerializeField] private TextMeshProUGUI targetText;

    protected override void HandleDeath()
    {
        base.HandleDeath();
        UIManager.Instance.ShowMessage(targetText, 5f);
    }
}
