using HarmonyLib;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Stations;
using LBoL.Core;
using LBoL.EntityLib.EnemyUnits.Character;
using LBoL.EntityLib.EnemyUnits.Normal;
using LBoL.EntityLib.EnemyUnits.Normal.Ravens;
using LBoL.EntityLib.Stages.NormalStages;
using LBoL.Presentation;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using test.EnemyUnits;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using LBoLEntitySideloader.ReflectionHelpers;

namespace test.EnemyGroups
{
    public sealed class StSShieldSpearEnemyGroupDef : EnemyGroupTemplate
    {
        public override IdContainer GetId() => "StSShieldSpear";
        public override EnemyGroupConfig MakeConfig()
        {
            AddFormation("Surround", new Dictionary<int, Vector2>() {
                { 0, new Vector2(-4, 0.5f) },
                { 1, new Vector2(4, 0.5f) },
            });
            var config = new EnemyGroupConfig(
                Id: "",
                Name: "StSShieldSpear",
                FormationName: "Surround",
                Enemies: new List<string>() { nameof(StSShield), nameof(StSSpear) },
                EnemyType: EnemyType.Elite,
                DebutTime: 1f,
                RollBossExhibit: false,
                PlayerRoot: new Vector2(0f, 0.5f),
                PreBattleDialogName: "",
                PostBattleDialogName: ""
            );
            return config;
        }
        [HarmonyPatch(typeof(FinalStage), nameof(FinalStage.CreateMap))]
        class FinalStage_CreateMap_Patch
        {
            static AccessTools.FieldRef<GameMap, MapNode[,]> nodesRef = AccessTools.FieldRefAccess<GameMap, MapNode[,]>(ConfigReflection.BackingWrap(nameof(GameMap.Nodes)));
            static void Postfix(FinalStage __instance, GameMap __result)
            {
                __instance.EliteEnemyPool.Add("StSShieldSpear", 1f);
                __result.Nodes[3, 0] = new MapNode(__result, 3, 0, 1)
                {
                    StationType = StationType.EliteEnemy,
                    AdjacencyList = { 0 }
                };
                __result.Nodes.OfType<MapNode>().AddItem(new MapNode(__result, 4, 0, 1)
                {
                    StationType = StationType.Boss,
                    AdjacencyList = { 0 }
                });
                /*__result.Nodes[4, 0] = new MapNode(__result, 4, 0, 1)
                {
                    StationType = StationType.Boss,
                    AdjacencyList = { 0 }
                };*/
            }
        }
    }
}