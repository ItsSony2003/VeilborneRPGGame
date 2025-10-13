using UnityEngine;

public class ItemEffect_DataSO : ScriptableObject
{
    [TextArea]
    public string effectDescription;
    protected Player player;

    public virtual bool CanBeUsed(Player player)
    {
        return true;
    }

    public virtual void ExecuteEffect()
    {

    }

    public virtual void Apply(Player player)
    {
        this.player = player;
    }

    public virtual void Remove()
    {

    }
}
