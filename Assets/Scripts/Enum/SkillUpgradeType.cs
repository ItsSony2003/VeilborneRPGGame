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
    Shard_DetectEnemy, // Shard will move towards nearest enemy
    Shard_MultiShard, // Shard can have up to N charges, can cast them all in a row
    Shard_Teleport, // you can swap places with the last shard you created
    Shard_TeleportAndHealthRewind, // when you swap place with shard, HP % is same as it was when u first created it

        // ----------- Riftfang (Swordthrow) Tree ------------
    SwordThrow, // Throw sowrd to damage enemies
    SwordThrow_Spin, // sword will spin at one position like a chainsaw to damage enemy
    SwordThrow_Ricochet, // sword can ricochet and hit other enemies
    SwordThrow_Pierce, // can pierce through enemies in a line
}
