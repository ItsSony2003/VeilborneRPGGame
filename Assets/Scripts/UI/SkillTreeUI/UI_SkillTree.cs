using UnityEngine;

public class UI_SkillTree : MonoBehaviour
{
    [SerializeField] private int skillPoints;
    [SerializeField] private UI_TreeConnectionHandler[] parentNodes;
    private UI_TreeNode[] treeNodes;
    public Player_SkillManager skillManager {  get; private set; }

    private void Start()
    {
        UpdateAllConnections();
    }

    public void UnlockDefaultSkills()
    {
        treeNodes = GetComponentsInChildren<UI_TreeNode>(true);
        skillManager = FindAnyObjectByType<Player_SkillManager>();

        foreach (var node in treeNodes)
            node.UnlockDefaultSkill();
    }

    [ContextMenu("Reset All Skills")]
    public void RefundAllSkills()
    {
        UI_TreeNode[] skillNodes = GetComponentsInChildren<UI_TreeNode>();

        foreach (var node in skillNodes)
            node.Refund();
    }   
    
    public bool EnoughSkillPoints(int cost) => skillPoints >= cost;
    public void ReduceSkillPoints(int cost) => skillPoints -= cost;
    public void AddSkillPoints(int points) => skillPoints += points;

    [ContextMenu("Update All Skill Connections")]
    public void UpdateAllConnections()
    {
        foreach (var node in parentNodes)
        {
            node.UpdateAllConnections();
        }
    }
}
