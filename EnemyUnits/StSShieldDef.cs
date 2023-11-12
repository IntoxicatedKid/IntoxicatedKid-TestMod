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
using LBoL.EntityLib.StatusEffects.Basic;
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
    public sealed class StSShieldDef : EnemyUnitTemplate
    {
        public override IdContainer GetId() => nameof(StSShield);
        public override LocalizationOption LoadLocalization()
        {
            var locFiles = new LocalizationFiles(embeddedSource);
            locFiles.AddLocaleFile(Locale.En, "Resources.EnemyUnitsEn.yaml");
            return locFiles;
        }
        public override EnemyUnitConfig MakeConfig()
        {
            var enemyUnitConfig = new EnemyUnitConfig(
                Id: "StSShield",
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
                MaxHp: 120,
                Damage1: 8,
                Damage2: 22,
                Damage3: null,
                Damage4: null,
                Power: 1,
                Defend: 24,
                Count1: null,
                Count2: null,
                MaxHpHard: 130,
                Damage1Hard: 10,
                Damage2Hard: 26,
                Damage3Hard: null,
                Damage4Hard: null,
                PowerHard: 1,
                DefendHard: 27,
                Count1Hard: null,
                Count2Hard: null,
                MaxHpLunatic: 140,
                Damage1Lunatic: 12,
                Damage2Lunatic: 30,
                Damage3Lunatic: null,
                Damage4Lunatic: null,
                PowerLunatic: 1,
                DefendLunatic: 30,
                Count1Lunatic: null,
                Count2Lunatic: null,
                PowerLoot: new MinMax(25, 25),
                BluePointLoot: new MinMax(25, 25),
                Gun1: new string[] { "GuihuoB" },
                Gun2: new string[] { "埴轮造形B" },
                Gun3: new string[] { "森罗" },
                Gun4: new string[] { "森罗" }
            );
            return enemyUnitConfig;
        }
    }
    [EntityLogic(typeof(StSShieldDef))]
    public sealed class StSShield : EnemyUnit
    {
        [UsedImplicitly]
        public override string Name
        {
            get
            {
                return "Spire Shield";
            }
        }
        private MoveType Next { get; set; }
        protected override void OnEnterBattle(BattleController battle)
        {
            CountDown = 2;
            Next = EnemyMoveRng.NextInt(0, 1) == 0 ? MoveType.Bash : MoveType.Fortify;
            ReactBattleEvent(Battle.BattleStarted, new System.Func<GameEventArgs, IEnumerable<BattleAction>>(OnBattleStarted));
        }
        private IEnumerable<BattleAction> OnBattleStarted(GameEventArgs arg)
        {
            if (!Battle.Player.HasStatusEffect<StSSurroundedSeDef.StSSurroundedSe>())
            {
                yield return new ApplyStatusEffectAction<StSSurroundedSeDef.StSSurroundedSe>(Battle.Player, null, null, null, null, 0f, true);
            }
            yield return new ApplyStatusEffectAction<StSBackAttackSeDef.StSBackAttackSe>(this, null, null, null, null, 0f, true);
            yield break;
        }
        public override void OnSpawn(EnemyUnit spawner)
        {
            React(new ApplyStatusEffectAction<StSBackAttackSeDef.StSBackAttackSe>(this, null, null, null, null, 0f, true));
        }
        private IEnumerable<BattleAction> DefendActions()
        {
            yield return new EnemyMoveAction(this, GetMove(1), true);
            //yield return new CastBlockShieldAction(this, Defend, 0, BlockShieldType.Normal, false);
            foreach (EnemyUnit enemyUnit in AllAliveEnemies)
            {
                yield return new ApplyStatusEffectAction<NextTurnGainBlock>(enemyUnit, new int?(Defend), null, null, null, 0.2f, true);
            }
            yield break;
        }
        protected override IEnumerable<IEnemyMove> GetTurnMoves()
        {
            switch (Next)
            {
                case MoveType.Bash:
                    yield return AttackMove(GetMove(0), Gun1, Damage1, 1, false, false, false);
                    yield return NegativeMove(null, typeof(FirepowerNegative), new int?(Power), null, true, false, null);
                    break;
                case MoveType.Fortify:
                    yield return new SimpleEnemyMove(Intention.Defend(), DefendActions());
                    break;
                case MoveType.Smash:
                    yield return AttackMove(GetMove(2), Gun2, Damage2, 1, false, false, false);
                    yield return DefendMove(this, null, Defend >= 30 ? Defend * 2 : Defend, 0, 0, false, null);
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
                Next = MoveType.Smash;
                CountDown = 3;
                return;
            }
            MoveType moveType;
            switch (Next)
            {
                case MoveType.Bash:
                    moveType = MoveType.Fortify;
                    break;
                case MoveType.Fortify:
                    moveType = MoveType.Bash;
                    break;
                case MoveType.Smash:
                    moveType = EnemyMoveRng.NextInt(0, 1) == 0 ? MoveType.Bash : MoveType.Fortify;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Next = moveType;
        }
        private enum MoveType
        {
            Bash,
            Fortify,
            Smash
        }
    }
    public sealed class StSShieldUnitModelDef : UnitModelTemplate
    {
        public override IdContainer GetId() => nameof(StSShield);
        public override LocalizationOption LoadLocalization()
        {
            var locFiles = new LocalizationFiles(embeddedSource);
            locFiles.AddLocaleFile(Locale.En, "UnitModelsEn.yaml");
            return locFiles;
        }

        public override ModelOption LoadModelOptions()
        {
            return new ModelOption(ResourceLoader.LoadSpriteAsync("StSShield.png", directorySource, ppu: 125));
        }
        public override UniTask<Sprite> LoadSpellSprite() => ResourceLoader.LoadSpriteAsync("StSShield.png", directorySource, ppu: 125);
        public override UnitModelConfig MakeConfig()
        {
            var unitModelConfig = new UnitModelConfig(
                Name: "StSShield",
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
    public sealed class StSSurroundedSeDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(StSSurroundedSe);
        }

        public override LocalizationOption LoadLocalization()
        {
            var locFiles = new LocalizationFiles(embeddedSource);
            locFiles.AddLocaleFile(Locale.En, "Resources.StatusEffectsEn.yaml");
            return locFiles;
        }

        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("Resources.StSSurroundedSe.png", embeddedSource);
        }
        public override StatusEffectConfig MakeConfig()
        {
            var statusEffectConfig = new StatusEffectConfig(
                Id: "",
                Order: 10,
                Type: StatusEffectType.Special,
                IsVerbose: false,
                IsStackable: false,
                StackActionTriggerLevel: null,
                HasLevel: false,
                LevelStackType: StackType.Add,
                HasDuration: false,
                DurationStackType: StackType.Add,
                DurationDecreaseTiming: DurationDecreaseTiming.Custom,
                HasCount: false,
                CountStackType: StackType.Keep,
                LimitStackType: StackType.Keep,
                ShowPlusByLimit: false,
                Keywords: Keyword.None,
                RelativeEffects: new List<string>() { },
                VFX: "Default",
                VFXloop: "Default",
                SFX: "Default"
            );
            return statusEffectConfig;
        }


        [EntityLogic(typeof(StSSurroundedSeDef))]
        public sealed class StSSurroundedSe : StatusEffect
        {
            public static bool Flipped = false;
            protected override void OnAdded(Unit unit)
            {
                ReactOwnerEvent(Battle.CardUsing, new EventSequencedReactor<CardUsingEventArgs>(OnCardUsing));
                ReactOwnerEvent(Battle.UsUsing, new EventSequencedReactor<UsUsingEventArgs>(OnUsUsing));
            }
            private IEnumerable<BattleAction> OnCardUsing(CardUsingEventArgs args)
            {
                if (args.Selector.Type == TargetType.SingleEnemy && args.Selector.SelectedEnemy.HasStatusEffect<StSBackAttackSeDef.StSBackAttackSe>())
                {
                    GameMaster.Instance.StartCoroutine(FlipPlayer());
                    yield return new RemoveStatusEffectAction(args.Selector.SelectedEnemy.GetStatusEffect<StSBackAttackSeDef.StSBackAttackSe>(), false);
                    foreach (EnemyUnit enemyUnit in Battle.AllAliveEnemies.Where((EnemyUnit enemyUnit) => enemyUnit != args.Selector.SelectedEnemy))
                    {
                        yield return new ApplyStatusEffectAction(typeof(StSBackAttackSeDef.StSBackAttackSe), enemyUnit, null, null, null, null, 0.2f, true);
                    }
                }
                yield break;
            }
            private IEnumerable<BattleAction> OnUsUsing(UsUsingEventArgs args)
            {
                if (args.Selector.Type == TargetType.SingleEnemy && args.Selector.SelectedEnemy.HasStatusEffect<StSBackAttackSeDef.StSBackAttackSe>())
                {
                    GameMaster.Instance.StartCoroutine(FlipPlayer());
                    yield return new RemoveStatusEffectAction(args.Selector.SelectedEnemy.GetStatusEffect<StSBackAttackSeDef.StSBackAttackSe>(), false);
                    foreach (EnemyUnit enemyUnit in Battle.AllAliveEnemies.Where((EnemyUnit enemyUnit) => enemyUnit != args.Selector.SelectedEnemy))
                    {
                        yield return new ApplyStatusEffectAction(typeof(StSBackAttackSeDef.StSBackAttackSe), enemyUnit, null, null, null, null, 0.2f, true);
                    }
                }
                yield break;
            }
            private IEnumerator FlipPlayer()
            {
                if (Flipped == false)
                {
                    var t = GameDirector.Player.transform;
                    t.localScale = t.localScale.FlipY();
                    t.Rotate(new Vector3(0, 0, 180));
                    GameDirector.Player._flip = !GameDirector.Player._flip;
                    /*Unit player = Battle.Player;
                    UnitView playerView = GameDirector.GetUnit(player);
                    yield return DOTween.Sequence().Append(playerView.transform.DOScaleX(-1f, 0.2f))
                    .SetEase(Ease.InSine)
                    .SetUpdate(true)
                    .SetAutoKill(true)
                    .WaitForCompletion();*/
                    Flipped = true;
                }
                else
                {
                    var t = GameDirector.Player.transform;
                    t.localScale = t.localScale.FlipY();
                    t.Rotate(new Vector3(0, 0, -180));
                    GameDirector.Player._flip = GameDirector.Player._flip;
                    /*Unit player = Battle.Player;
                    UnitView playerView = GameDirector.GetUnit(player);
                    yield return DOTween.Sequence().Append(playerView.transform.DOScaleX(1f, 0.2f))
                    .SetEase(Ease.InSine)
                    .SetUpdate(true)
                    .SetAutoKill(true)
                    .WaitForCompletion();*/
                    Flipped = false;
                }
                yield break;
            }
        }
    }
    public sealed class StSBackAttackSeDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(StSBackAttackSe);
        }

        public override LocalizationOption LoadLocalization()
        {
            var locFiles = new LocalizationFiles(embeddedSource);
            locFiles.AddLocaleFile(Locale.En, "Resources.StatusEffectsEn.yaml");
            return locFiles;
        }

        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("Resources.StSBackAttackSe.png", embeddedSource);
        }
        public override StatusEffectConfig MakeConfig()
        {
            var statusEffectConfig = new StatusEffectConfig(
                Id: "",
                Order: 10,
                Type: StatusEffectType.Special,
                IsVerbose: false,
                IsStackable: true,
                StackActionTriggerLevel: null,
                HasLevel: false,
                LevelStackType: StackType.Add,
                HasDuration: false,
                DurationStackType: StackType.Add,
                DurationDecreaseTiming: DurationDecreaseTiming.Custom,
                HasCount: false,
                CountStackType: StackType.Keep,
                LimitStackType: StackType.Keep,
                ShowPlusByLimit: false,
                Keywords: Keyword.None,
                RelativeEffects: new List<string>() { },
                VFX: "Default",
                VFXloop: "Default",
                SFX: "Default"
            );
            return statusEffectConfig;
        }


        [EntityLogic(typeof(StSBackAttackSeDef))]
        public sealed class StSBackAttackSe : StatusEffect
        {
            [UsedImplicitly]
            public int Value
            {
                get
                {
                    return 50;
                }
            }
            protected override void OnAdded(Unit unit)
            {
                HandleOwnerEvent(unit.DamageDealing, new GameEventHandler<DamageDealingEventArgs>(OnDamageDealing));
            }
            private void OnDamageDealing(DamageDealingEventArgs args)
            {
                DamageInfo damageInfo = args.DamageInfo;
                if (damageInfo.DamageType == DamageType.Attack)
                {
                    damageInfo.Damage = damageInfo.Amount * (1f + Value / 100f);
                    args.DamageInfo = damageInfo;
                    args.AddModifier(this);
                }
            }
        }
    }
}