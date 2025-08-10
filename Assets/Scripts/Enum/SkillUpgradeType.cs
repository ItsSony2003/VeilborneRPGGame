using UnityEngine;

public enum SkillUpgradeType
{
    // ----------- Dash Tree ------------
    Dash, // Dash avoid damage, VEILSTEP
    Dash_CloneOnStart, // Create a clone when dash starts
    Dash_CloneOnStartAndArrival, // create a clone when dash starts and ends
    Dash_ShardOnStart, // create a shard when dash starts
    Dash_ShardOnStartAndArrival, // create a shard when dash starts and ends
}
