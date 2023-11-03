using Cysharp.Threading.Tasks;
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
using LBoL.EntityLib.Exhibits.Shining;
using LBoL.EntityLib.JadeBoxes;
using LBoL.EntityLib.StatusEffects.Enemy;
using LBoL.Presentation;
using LBoL.Presentation.UI.Panels;
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
using test.PlayerUnits;
using test.Stages;
using UnityEngine;
using static test.BepinexPlugin;
using static UnityEngine.UI.GridLayoutGroup;


namespace test.EnemyUnits
{
    public sealed class AyaBossDef : EnemyUnitTemplate
    {
        public override IdContainer GetId() => nameof(AyaBoss);
        public override LocalizationOption LoadLocalization()
        {
            var locFiles = new LocalizationFiles(embeddedSource);
            locFiles.AddLocaleFile(Locale.En, "Resources.EnemyUnitsEn.yaml");
            return locFiles;
        }
        public override EnemyUnitConfig MakeConfig()
        {
            var enemyUnitConfig = new EnemyUnitConfig(
                Id: "AyaBoss",
                RealName: true,
                OnlyLore: false,
                BaseManaColor: new ManaColor[] { ManaColor.Red, ManaColor.Green },
                Order: 10,
                ModleName: "Aya",
                NarrativeColor: "#000000",
                Type: EnemyType.Boss,
                IsPreludeOpponent: true,
                HpLength: null,
                MaxHpAdd: null,
                MaxHp: 170,
                Damage1: 9,
                Damage2: 12,
                Damage3: 15,
                Damage4: 30,
                Power: 1,
                Defend: 15,
                Count1: 8,
                Count2: 8,
                MaxHpHard: 180,
                Damage1Hard: 10,
                Damage2Hard: 14,
                Damage3Hard: 18,
                Damage4Hard: 35,
                PowerHard: 2,
                DefendHard: 17,
                Count1Hard: 8,
                Count2Hard: 8,
                MaxHpLunatic: 190,
                Damage1Lunatic: 11,
                Damage2Lunatic: 16,
                Damage3Lunatic: 21,
                Damage4Lunatic: 40,
                PowerLunatic: 3,
                DefendLunatic: 19,
                Count1Lunatic: 8,
                Count2Lunatic: 8,
                PowerLoot: new MinMax(100, 100),
                BluePointLoot: new MinMax(100, 100),
                Gun1: new string[] { "EAyaShoot1" },
                Gun2: new string[] { "森罗" },
                Gun3: new string[] { "森罗" },
                Gun4: new string[] { "森罗" }
            );
            return enemyUnitConfig;
        }
    }
    [EntityLogic(typeof(AyaBossDef))]
    public sealed class AyaBoss : EnemyUnit
    {
        public override UnitName GetName()
        {
            return UnitNameTable.GetName("Aya", EnemyUnitConfig.FromId("Aya").NarrativeColor);
        }
        private MoveType Next { get; set; }
        private string SpellGameOver
        {
            get
            {
                return "Game Over";
            }
        }
        protected override void OnEnterBattle(BattleController battle)
        {
            var background = StageTemplate.TryGetEnvObject(NewBackgrounds.HexagonBackground);
            background.SetActive(true);
            Next = MoveType.GameOver;
            ReactBattleEvent(Battle.BattleStarted, new Func<GameEventArgs, IEnumerable<BattleAction>>(OnBattleStarted));
        }
        private IEnumerable<BattleAction> OnBattleStarted(GameEventArgs arg)
        {
            //GameMaster.Instance.StartCoroutine(HexagonBackgroundOff(background));
            //yield return new CastBlockShieldAction(this, Defend, Defend, BlockShieldType.Normal, false);
            yield return new ApplyStatusEffectAction<LBoL.Core.StatusEffects.ExtraTurn>(Battle.Player, Count1, null, null, null, 0f, true);
            yield return new ApplyStatusEffectAction<HexagonSeDef.HexagonSe>(this, 0, null, 10, null, 0f, true);
            GameMaster.Instance.StartCoroutine(Announce());
            yield break;
        }
        /*IEnumerator HexagonBackgroundOff(GameObject gameObject)
        {
            yield return new WaitForSeconds(5f);
            gameObject.SetActive(false);
        }*/
        private IEnumerator Announce()
        {
            yield return new WaitForSecondsRealtime(30f);
            yield return PerformAction.Chat(this, "Point", 2f, 0f, 0f, true);
            yield return new WaitForSecondsRealtime(30f);
            yield return PerformAction.Chat(this, "Line", 2f, 0f, 0f, true);
            yield return new WaitForSecondsRealtime(30f);
            yield return PerformAction.Chat(this, "Triangle", 2f, 0f, 0f, true);
            yield return new WaitForSecondsRealtime(30f);
            yield return PerformAction.Chat(this, "Square", 2f, 0f, 0f, true);
            yield return new WaitForSecondsRealtime(30f);
            yield return PerformAction.Chat(this, "Pentagon", 2f, 0f, 0f, true);
            yield return new WaitForSecondsRealtime(30f);
            yield return PerformAction.Chat(this, "Hexagon", 2f, 0f, 0f, true);
        }
        private IEnumerable<BattleAction> GameOver()
        {
            Battle.Player.ClearStatusEffects();
            foreach (BattleAction battleAction in AttackActions(null, Gun1, Damage1, 1, true, false))
            {
                yield return battleAction;
            }
            yield break;
        }
        protected override IEnumerable<IEnemyMove> GetTurnMoves()
        {
            switch (Next)
            {
                case MoveType.GameOver:
                    yield return new SimpleEnemyMove(Intention.SpellCard(SpellGameOver, Damage1, true), GameOver());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            yield break;
        }
        protected override void UpdateMoveCounters()
        {
            Next = MoveType.GameOver;
        }
        private enum MoveType
        {
            GameOver
        }
    }
    public sealed class AyaBossSeDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaBossSe);
        }

        public override LocalizationOption LoadLocalization()
        {
            var locFiles = new LocalizationFiles(embeddedSource);
            locFiles.AddLocaleFile(Locale.En, "Resources.StatusEffectsEn.yaml");
            return locFiles;
        }

        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("Resources.HexagonSe.png", embeddedSource);
        }
        public override StatusEffectConfig MakeConfig()
        {
            var statusEffectConfig = new StatusEffectConfig(
                Id: "",
                Order: 0,
                Type: StatusEffectType.Special,
                IsVerbose: false,
                IsStackable: false,
                StackActionTriggerLevel: null,
                HasLevel: true,
                LevelStackType: StackType.Add,
                HasDuration: false,
                DurationStackType: StackType.Add,
                DurationDecreaseTiming: DurationDecreaseTiming.Custom,
                HasCount: true,
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


        [EntityLogic(typeof(AyaBossSeDef))]
        public sealed class AyaBossSe : StatusEffect
        {
            static bool Activate = false;
            int DifficultyDamage = 0;
            protected override void OnAdded(Unit unit)
            {
                HandleOwnerEvent(unit.DamageTaking, new GameEventHandler<DamageEventArgs>(OnDamageTaking));
                HandleOwnerEvent(unit.StatusEffectRemoving, new GameEventHandler<StatusEffectEventArgs>(OnStatusEffectRemoving));
                if (!Activate)
                {
                    Activate = true;
                    GameMaster.Instance.StartCoroutine(Difficulty());
                    GameMaster.Instance.StartCoroutine(IncomingWall());
                    GameMaster.Instance.StartCoroutine(LastWord());
                    GameMaster.Instance.StartCoroutine(DeathSentence());
                }
                ReactOwnerEvent(Battle.CardUsed, new EventSequencedReactor<CardUsingEventArgs>(OnCardUsed));
            }
            private void OnDamageTaking(DamageEventArgs args)
            {
                int num = args.DamageInfo.Damage.RoundToInt();
                if (num > 0)
                {
                    NotifyActivating();
                    args.DamageInfo = args.DamageInfo.ReduceActualDamageBy(num);
                    args.AddModifier(this);
                }
            }
            private void OnStatusEffectRemoving(StatusEffectEventArgs args)
            {
                if (args.Effect is AyaBossSe)
                {
                    args.ForceCancelBecause(CancelCause.Reaction);
                    React(new DamageAction(Owner, Battle.Player, DamageInfo.Reaction(Level), "扩散结界", GunType.Single));
                    args.Effect.Count = 10;
                }
            }
            private IEnumerator IncomingWall()
            {
                while (Owner.Hp > 0)
                {
                    if (Count > 0)
                    {
                        Count--;
                    }
                    if (Count <= 0)
                    {
                        EnemyUnit enemyUnit = Owner as EnemyUnit;
                        Level += enemyUnit.Count2 + DifficultyDamage;
                        Count = 10;
                    }
                    yield return new WaitForSecondsRealtime(1f);
                }
            }
            private IEnumerator DeathSentence()
            {
                while (Owner.Hp > 0)
                {
                    if (Level >= 100)
                    {
                        if (Battle.Player.Hp < 1)
                        {
                            break;
                        }
                        else if (Battle.Player.Hp == 1)
                        {
                            GameMaster.Instance.CurrentGameRun.Battle.Player.ClearStatusEffects();
                            GameMaster.Instance.CurrentGameRun.Battle.RequestEndPlayerTurn();
                            break;
                        }
                        else if (Battle.Player.Hp > 1)
                        {
                            GameMaster.Instance.CurrentGameRun.SetHpAndMaxHp(Battle.Player.Hp - 1, Battle.Player.MaxHp, false);
                        }
                    }
                    yield return new WaitForSecondsRealtime(Math.Max(1 - Level * 0.001f, 0.1f));
                }
            }
            private IEnumerator LastWord()
            {
                while (Owner.Hp > 0)
                {
                    GameMaster.Instance.CurrentGameRun.SetEnemyHpAndMaxHp(Math.Max(Owner.Hp - 3, 0), Owner.MaxHp, Owner as EnemyUnit, false);
                    yield return new WaitForSecondsRealtime(0.084f);
                }
                GameMaster.Instance.CurrentGameRun.Battle.RequestEndPlayerTurn();
                GameMaster.Instance.CurrentGameRun.Battle.ForceKill(Owner, Owner);
                /*foreach (BattleAction battleAction in Defeat())
                {
                    yield return battleAction;
                }*/
            }
            private IEnumerable<BattleAction> Defeat()
            {
                yield return new ForceKillAction(Owner, Owner);
            }
            private IEnumerator Difficulty()
            {
                yield return new WaitForSecondsRealtime(30f);
                DifficultyDamage += 5;
                yield return new WaitForSecondsRealtime(30f);
                DifficultyDamage += 5;
                yield return new WaitForSecondsRealtime(30f);
                DifficultyDamage += 5;
                yield return new WaitForSecondsRealtime(30f);
                DifficultyDamage += 5;
                yield return new WaitForSecondsRealtime(30f);
                DifficultyDamage += 5;
                yield return new WaitForSecondsRealtime(30f);
                DifficultyDamage += 5;
            }
            private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
            {
                if (Battle.Player.IsInTurn && Level > 0)
                {
                    NotifyActivating();
                    yield return new DamageAction(Owner, Battle.Player, DamageInfo.Reaction(Level), "扩散结界", GunType.Single);
                    Level = 0;
                }
                yield break;
            }
        }
    }
    public sealed class AyaBossBgm : BgmTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaBoss);
        }

        public override UniTask<AudioClip> LoadAudioClipAsync()
        {
            return ResourceLoader.LoadAudioClip("Courtesy (Remastered) - Super Hexagon.ogg", AudioType.OGGVORBIS, directorySource);

        }

        public override BgmConfig MakeConfig()
        {
            var config = new BgmConfig(
                    ID: "",
                    No: sequenceTable.Next(typeof(BgmConfig)),
                    Show: true,
                    Name: "",
                    Folder: "",
                    Path: "",
                    LoopStart: (float?)118.25,
                    LoopEnd: (float?)236.4,
                    TrackName: "Courtesy",
                    Artist: "SiIvaGunner",
                    Original: "Courtesy",
                    Comment: ""
            );

            return config;
        }
        [HarmonyPatch(typeof(BattleController), nameof(BattleController.StartBattle))]
        class BattleController_StartBattle_Patch
        {
            //public static bool Changed;
            static void Postfix(BattleController __instance)
            {
                if (GameMaster.Instance.CurrentGameRun.CurrentStation.Type != StationType.Boss)
                {
                    if (GameMaster.Instance.CurrentGameRun.Battle.EnemyGroup.Id == "Hexagon")
                    {
                        BgmConfig.FromID(new HexagonBgm().UniqueId).LoopStart = 0f;
                        BgmConfig.FromID(new HexagonBgm().UniqueId).LoopEnd = 336.004f;
                        AudioManager.PlayInLayer1("Hexagon");

                    }
                }
            }
        }
    }
}