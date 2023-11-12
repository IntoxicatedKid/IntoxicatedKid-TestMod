using Cysharp.Threading.Tasks.Triggers;
using HarmonyLib;
using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.JadeBoxes;
using LBoL.Core.Randoms;
using LBoL.Core.Stations;
using LBoL.Core.Units;
using LBoL.EntityLib.Adventures;
using LBoL.EntityLib.Adventures.Stage2;
using LBoL.EntityLib.Stages.NormalStages;
using LBoL.Presentation;
using LBoL.Presentation.Effect;
using LBoL.Presentation.UI.Widgets;
using LBoL.Presentation.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using test.EnemyGroups;
using test.EnemyUnits;
using test.Exhibits;
using UnityEngine;

namespace test.JadeBoxes
{
    public sealed class InterdimensionalObjectJadeBoxDef : JadeBoxTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(InterdimensionalObjectJadeBox);
        }

        public override LocalizationOption LoadLocalization()
        {
            return new DirectLocalization(new Dictionary<string, object>() {
                { "Name", "Interdimensional Object" },
                { "Description", "Hexagon will always appear."}
            });
        }
        public override JadeBoxConfig MakeConfig()
        {
            var config = DefaultConfig();
            return config;
        }
    }
    [EntityLogic(typeof(InterdimensionalObjectJadeBoxDef))]
    public sealed class InterdimensionalObjectJadeBox : JadeBox
    {
        protected override void OnAdded()
        {
            GameRun._stages.TryGetValue(1).AdventurePool.Remove(typeof(BuduSuanming), true);
            GameRun._stages.TryGetValue(3).Boss = Library.GetEnemyGroupEntry("Hexagon");
        }
    }
}
