using UnityEngine;

public class UI : MonoBehaviour
{
    public UI_SkillToolTip skillTip;

    private void Awake()
    {
        skillTip = GetComponentInChildren<UI_SkillToolTip>();
    }
}
