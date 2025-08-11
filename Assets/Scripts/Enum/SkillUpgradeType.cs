using UnityEngine;

public enum SkillUpgradeType
{
    None,

    // ----------- Veilstep (Dash) Tree: has Clone and Shard ------------
    Dash, // Dash avoid damage, VEILSTEP
    Dash_CloneOnStart, // Create a clone when dash starts
    Dash_CloneOnStartAndArrival, // create a clone when dash starts and ends
    Dash_ShardOnStart, // create a shard when dash starts
    Dash_ShardOnStartAndArrival, // create a shard when dash starts and ends

    // ----------- Wraithcore (Shard) Tree ------------
    Shard, // Shard explodes when an enemy touch it or time is up
    Shard_MoveToEnemy, // Shard will move towards nearest enemy
    Shard_TripleCast, // Shard can have up to N charges, can cast them all in a row
    Shard_Teleport, // you can swap places with the last shard you created
    Shard_TeleportAndHeal // when you swap place with shard, HP % is same as it was when u first created it
}
