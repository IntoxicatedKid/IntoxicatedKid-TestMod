using HarmonyLib;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.EntityLib.EnemyUnits.Character;
using LBoL.EntityLib.EnemyUnits.Normal;
using LBoL.EntityLib.EnemyUnits.Normal.Ravens;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using test.EnemyUnits;
using UnityEngine;

namespace test.EnemyGroups
{
    public sealed class AyaBossEnemyGroupDef : EnemyGroupTemplate
    {
        public override IdContainer GetId() => nameof(AyaBossEnemyGroupDef);
        public override EnemyGroupConfig MakeConfig()
        {
            var config = new EnemyGroupConfig(
                Id: "",
                Name: "AyaBoss",
                FormationName: VanillaFormations.Single,
                Enemies: new List<string>() { nameof(AyaBoss) },
                EnemyType: EnemyType.Boss,
                DebutTime: 1f,
                RollBossExhibit: true,
                PlayerRoot: new Vector2(-4f, 0.5f),
                PreBattleDialogName: "",
                PostBattleDialogName: ""
            );
            return config;
        }
    }
}