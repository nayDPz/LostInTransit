﻿using Moonstorm;
using RoR2;
using R2API;
using RoR2.Items;
using Moonstorm.Components;

namespace LostInTransit.Buffs
{
    [DisabledContent]
    public class DiceAtk : BuffBase
    {
        public override BuffDef BuffDef { get; } = LITAssets.LoadAsset<BuffDef>("DiceAtk");

        public class DiceAtkBehavior : BaseBuffBodyBehavior, IBodyStatArgModifier
        {
            [BuffDefAssociation(useOnServer = true, useOnClient = true)]
            public static BuffDef GetBuffDef() => LITContent.Buffs.DiceAtk;

            public void ModifyStatArguments(RecalculateStatsAPI.StatHookEventArgs args)
            {
                args.attackSpeedMultAdd += (Items.BlessedDice.atkAmount / 100);
            }
        }
    }
}