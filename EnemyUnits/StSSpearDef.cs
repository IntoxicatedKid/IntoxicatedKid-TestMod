using Cysharp.Threading.Tasks;
using DG.Tweening;
using HarmonyLib;
using JetBrains.Annotations;
using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.Stations;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.Cards.Neutral.Red;
using LBoL.EntityLib.Cards.Other.Enemy;
using LBoL.EntityLib.EnemyUnits.Character;
using LBoL.EntityLib.EnemyUnits.Normal;
using LBoL.EntityLib.Exhibits.Shining;
using LBoL.EntityLib.JadeBoxes;
using LBoL.EntityLib.Stages.NormalStages;
using LBoL.EntityLib.StatusEffects.Enemy;
using LBoL.EntityLib.StatusEffects.Others;
using LBoL.Presentation;
using LBoL.Presentation.UI.Panels;
using LBoL.Presentation.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.ReflectionHelpers;
using LBoLEntitySideloader.Resource;
using LBoLEntitySideloader.Utils;
using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using test.JadeBoxes;
using test.PlayerUnits;
using test.Stages;
using UnityEngine;
using static MonoMod.Cil.RuntimeILReferenceBag.FastDelegateInvokers;
using static test.BepinexPlugin;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.GridLayoutGroup;


namespace test.EnemyUnits
{
    public sealed class StSSpearDef : EnemyUnitTemplate
    {
        public override IdContainer GetId() => nameof(StSSpear);
        public override LocalizationOption LoadLocalization()
        {
            var locFiles = new LocalizationFiles(embeddedSource);
            locFiles.AddLocaleFile(Locale.En, "Resources.EnemyUnitsEn.yaml");
            return locFiles;
        }
        public override EnemyUnitConfig MakeConfig()
        {
            var enemyUnitConfig = new EnemyUnitConfig(
                Id: "StSSpear",
                RealName: true,
                OnlyLore: false,
                BaseManaColor: null,
                Order: 10,
                ModleName: null,
                NarrativeColor: "#ffffff",
                Type: EnemyType.Elite,
                IsPreludeOpponent: false,
                HpLength: null,
                MaxHpAdd: null,
                MaxHp: 180,
                Damage1: 4,
                Damage2: 8,
                Damage3: null,
                Damage4: null,
                Power: 2,
                Defend: null,
                Count1: 2,
                Count2: 4,
                MaxHpHard: 190,
                Damage1Hard: 5,
                Damage2Hard: 9,
                Damage3Hard: null,
                Damage4Hard: null,
                PowerHard: 2,
                DefendHard: null,
                Count1Hard: 2,
                Count2Hard: 4,
                MaxHpLunatic: 200,
                Damage1Lunatic: 6,
                Damage2Lunatic: 10,
                Damage3Lunatic: null,
                Damage4Lunatic: null,
                PowerLunatic: 2,
                DefendLunatic: null,
                Count1Lunatic: 2,
                Count2Lunatic: 4,
                PowerLoot: new MinMax(25, 25),
                BluePointLoot: new MinMax(25, 25),
                Gun1: new string[] { "Sunny1" },
                Gun2: new string[] { "YoumuKan" },
                Gun3: new string[] { "森罗" },
                Gun4: new string[] { "森罗" }
            );
            return enemyUnitConfig;
        }
    }
    [EntityLogic(typeof(StSSpearDef))]
    public sealed class StSSpear : EnemyUnit
    {
        [UsedImplicitly]
        public override string Name
        {
            get
            {
                return "Spire Spear";
            }
        }
        private MoveType Next { get; set; }
        protected override void OnEnterBattle(BattleController battle)
        {
            CountDown = 1;
            Next = MoveType.BurnStrike;
            ReactBattleEvent(Battle.BattleStarted, new System.Func<GameEventArgs, IEnumerable<BattleAction>>(OnBattleStarted));
        }
        private IEnumerable<BattleAction> OnBattleStarted(GameEventArgs arg)
        {
            if (!Battle.Player.HasStatusEffect<StSSurroundedSeDef.StSSurroundedSe>())
            {
                yield return new ApplyStatusEffectAction<StSSurroundedSeDef.StSSurroundedSe>(Battle.Player, null, null, null, null, 0f, true);
            }
            yield break;
        }
        private IEnumerable<BattleAction> BuffActions()
        {
            yield return new EnemyMoveAction(this, GetMove(1), true);
            foreach (EnemyUnit enemyUnit in AllAliveEnemies)
            {
                yield return new ApplyStatusEffectAction<Firepower>(enemyUnit, new int?(Power), null, null, null, 0.2f, true);
            }
            yield break;
        }
        protected override IEnumerable<IEnemyMove> GetTurnMoves()
        {
            switch (Next)
            {
                case MoveType.BurnStrike:
                    yield return AttackMove(GetMove(0), Gun1, Damage1, Count1, false, false, true);
                    yield return AddCardMove(null, Library.CreateCards<Riguang>(Count1, false), AddCardZone.Discard, null, false);
                    break;
                case MoveType.Piercer:
                    yield return new SimpleEnemyMove(Intention.PositiveEffect(), BuffActions());
                    break;
                case MoveType.Skewer:
                    yield return AttackMove(GetMove(2), Gun2, Damage2, Count2, false, true, true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            yield break;
        }
        protected override void UpdateMoveCounters()
        {
            int num = CountDown - 1;
            CountDown = num;
            if (CountDown <= 0)
            {
                Next = MoveType.Skewer;
                CountDown = 3;
                return;
            }
            MoveType moveType;
            switch (Next)
            {
                case MoveType.BurnStrike:
                    moveType = MoveType.Piercer;
                    break;
                case MoveType.Piercer:
                    moveType = MoveType.BurnStrike;
                    break;
                case MoveType.Skewer:
                    moveType = EnemyMoveRng.NextInt(0, 1) == 0 ? MoveType.BurnStrike : MoveType.Piercer;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Next = moveType;
        }
        private enum MoveType
        {
            BurnStrike,
            Piercer,
            Skewer
        }
    }
    public sealed class StSSpearUnitModelDef : UnitModelTemplate
    {
        public override IdContainer GetId() => nameof(StSSpear);
        public override LocalizationOption LoadLocalization()
        {
            var locFiles = new LocalizationFiles(embeddedSource);
            locFiles.AddLocaleFile(Locale.En, "UnitModelsEn.yaml");
            return locFiles;
        }

        public override ModelOption LoadModelOptions()
        {
            return new ModelOption(ResourceLoader.LoadSpriteAsync("StSSpear.png", directorySource, ppu: 125));
        }
        public override UniTask<Sprite> LoadSpellSprite() => ResourceLoader.LoadSpriteAsync("StSSpear.png", directorySource, ppu: 125);
        public override UnitModelConfig MakeConfig()
        {
            var unitModelConfig = new UnitModelConfig(
                Name: "StSSpear",
                Type: 0,
                EffectName: null,
                Offset: new Vector2(0.0f, 0.0f),
                Flip: false,
                Dielevel: 2,
                Box: new Vector2(0.8f, 1.8f),
                Shield: 1.2f,
                Block: 1.3f,
                Hp: new Vector2(0.0f, -1.3f),
                HpLength: 640,
                Info: new Vector2(0.0f, 1.2f),
                Select: new Vector2(1.6f, 2.0f),
                ShootStartTime: new float[] { 0.1f },
                ShootPoint: new Vector2[] { new Vector2(0.6f, 0.3f) },
                ShooterPoint: new Vector2[] { new Vector2(0.6f, 0.3f) },
                Hit: new Vector2(0.3f, 0.3f),
                HitRep: 0.1f,
                GuardRep: 0.1f,
                Chat: new Vector2(0.4f, 0.8f),
                ChatPortraitXY: new Vector2(-0.8f, -0.58f),
                ChatPortraitWH: new Vector2(0.6f, 0.5f),
                HasSpellPortrait: true,
                SpellPosition: new Vector2(400.0f, 0.0f),
                SpellScale: 0.9f,
                SpellColor: new Color32[] { new Color32(25, 25, 25, 255) }



            );
            return unitModelConfig;
        }
    }
}