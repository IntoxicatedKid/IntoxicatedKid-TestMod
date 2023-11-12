using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Adventures;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoL.EntityLib.StatusEffects.Sakuya;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using LBoL.Core.StatusEffects;
using LBoL.Core.Randoms;
using static test.BepinexPlugin;
using LBoL.EntityLib.StatusEffects.Basic;
using LBoL.EntityLib.StatusEffects.Reimu;
using LBoL.Base.Extensions;
using System.Linq;
using UnityEngine;
using LBoL.EntityLib.StatusEffects.Enemy;
using test.StatusEffects;
using LBoL.EntityLib.Cards.Character.Sakuya;
using LBoL.EntityLib.Cards.Other.Enemy;
using LBoL.Presentation;
using System.IO;
using HarmonyLib;
using LBoL.Core.Stations;
using test.EnemyUnits;
using LBoL.EntityLib.StatusEffects.Neutral;
using LBoLEntitySideloader.ExtraFunc;
using Cysharp.Threading.Tasks;

namespace test.Cards
{
    public sealed class AyaTerukuniShiningThroughHeavenandEarthDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaTerukuniShiningThroughHeavenandEarth);
        }

        public override CardImages LoadCardImages()
        {
            var imgs = new CardImages(embeddedSource);
            imgs.AutoLoad(this, ".png", relativePath: "Resources.");
            return imgs;
        }

        public override LocalizationOption LoadLocalization()
        {
            var locFiles = new LocalizationFiles(embeddedSource);
            locFiles.AddLocaleFile(Locale.En, "Resources.CardsEn.yaml");
            return locFiles;
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = new CardConfig(
               Index: sequenceTable.Next(typeof(CardConfig)),
               Id: "",
               Order: 10,
               AutoPerform: true,
               Perform: new string[0][],
               GunName: "Simple1",
               GunNameBurst: "Simple1",
               DebugLevel: 0,
               Revealable: false,
               IsPooled: true,
               HideMesuem: false,
               IsUpgradable: true,
               Rarity: Rarity.Rare,
               Type: CardType.Ability,
               TargetType: TargetType.Self,
               Colors: new List<ManaColor>() { ManaColor.Red, ManaColor.Green },
               IsXCost: false,
               Cost: new ManaGroup() { Red = 3, Green = 3 },
               UpgradedCost: new ManaGroup() { Red = 3, Green = 3 },
               MoneyCost: null,
               Damage: null,
               UpgradedDamage: null,
               Block: null,
               UpgradedBlock: null,
               Shield: null,
               UpgradedShield: null,
               Value1: 1,
               UpgradedValue1: null,
               Value2: 1,
               UpgradedValue2: null,
               Mana: new ManaGroup() { Philosophy = 6 },
               UpgradedMana: new ManaGroup() { Philosophy = 6 },
               Scry: null,
               UpgradedScry: null,
               ToolPlayableTimes: null,
               Loyalty: null,
               UpgradedLoyalty: null,
               PassiveCost: null,
               UpgradedPassiveCost: null,
               ActiveCost: null,
               UpgradedActiveCost: null,
               UltimateCost: null,
               UpgradedUltimateCost: null,

               Keywords: Keyword.None,
               UpgradedKeywords: Keyword.Initial | Keyword.Retain | Keyword.Replenish,
               EmptyDescription: false,
               RelativeKeyword: Keyword.Tool,
               UpgradedRelativeKeyword: Keyword.Tool,

               RelativeEffects: new List<string>() { },
               UpgradedRelativeEffects: new List<string>() { },
               RelativeCards: new List<string>() { },
               UpgradedRelativeCards: new List<string>() { },
               Owner: "AyaPlayerUnit",
               Unfinished: false,
               Illustrator: "uru uzuki",
               SubIllustrator: new List<string>() { }
            );

            return cardConfig;
        }
    }
    [EntityLogic(typeof(AyaTerukuniShiningThroughHeavenandEarthDef))]
    public sealed class AyaTerukuniShiningThroughHeavenandEarth : Card
    {
        static bool PoolQue = false;
        //static bool Played = false;
        [HarmonyPatch(typeof(BattleController), nameof(BattleController.StartBattle))]
        class BattleController_StartBattle_Patch
        {
            static void Postfix(BattleController __instance)
            {
                if (CardConfig.FromId("AyaTerukuniShiningThroughHeavenandEarth").IsPooled == true)
                {
                    PoolQue = true;
                    CardConfig.FromId("AyaTerukuniShiningThroughHeavenandEarth").IsPooled = false;
                }
            }
        }
        [HarmonyPatch(typeof(BattleController), nameof(BattleController.EndBattle))]
        class BattleController_EndBattle_Patch
        {
            static void Postfix(BattleController __instance)
            {
                if (PoolQue == true)
                {
                    PoolQue = false;
                    CardConfig.FromId("AyaTerukuniShiningThroughHeavenandEarth").IsPooled = true;
                }
            }
        }
        /*[HarmonyPatch(typeof(GameRunController), nameof(GameRunController.EnterStation))]
        class GameRunController_EnterStation_Patch
        {
            static void Postfix(GameRunController __instance)
            {
                if (Played)
                {
                    CardConfig.FromId("AyaTerukuniShiningThroughHeavenandEarth").IsPooled = false;
                }
            }
        }*/
        /*public override bool Triggered
        {
            get
            {
                return CanUse;
            }
        }
        public override bool CanUse
        {
            get
            {
                return Battle != null && played == false;
            }
        }*/
        /*public override void Initialize()
        {
            base.Initialize();
            if (DeckCounter > 0)
            {
                Played = true;
            }
        }*/
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return PerformAction.Spell(Battle.Player, "AyaLastWord");
            AudioManager.PlayInLayer1("AyaSpellBgm");
            yield return PerformAction.Effect(Battle.Player, "RenzhenAura", 0f, "RenzhenAura", 0f, PerformAction.EffectBehavior.Add, 0f);
            yield return new RemoveAllNegativeStatusEffectAction(Battle.Player);
            yield return BuffAction<AyaTerukuniShiningThroughHeavenandEarthSeDef.AyaTerukuniShiningThroughHeavenandEarthSe>(0, 0, 0, 0, 0.2f);
            yield return BuffAction<ExtraTurn>(Value1, 0, 0, 0, 0.3f);
            yield return new RequestEndPlayerTurnAction();
        }
        public override IEnumerable<BattleAction> AfterUseAction()
        {
            base.AfterUseAction();
            /*DeckCounter += 1;
            Card[] cards = GameRun.BaseDeck.Where((Card card) => card is AyaTerukuniShiningThroughHeavenandEarth).ToArray();
            Battle.GameRun.RemoveDeckCards(cards, false);
            List<Card> list = Battle.EnumerateAllCards().Where((Card card) => card is AyaTerukuniShiningThroughHeavenandEarth).ToList();
            foreach (Card card in list)
            {
                yield return new RemoveCardAction(card);
            }*/
            Card deckCardByInstanceId = Battle.GameRun.GetDeckCardByInstanceId(InstanceId);
            Battle.GameRun.RemoveDeckCards(new Card[] { deckCardByInstanceId }, false);
            yield return new RemoveCardAction(this);
            yield break;
        }
    }
    /*public sealed class AyaLastWordSpellDef : SpellTemplate
    {
        public override IdContainer GetId() => new AyaLastWordDef().UniqueId;

        public override LocalizationOption LoadLocalization()
        {
            var locFiles = new LocalizationFiles(embeddedSource);
            locFiles.AddLocaleFile(Locale.En, "SpellsEn.yaml");
            return locFiles;
        }

        public override SpellConfig MakeConfig()
        {
            return DefaultConfig();
        }
    }*/
    public sealed class AyaLastWordDef : UltimateSkillTemplate
    {
        public override IdContainer GetId() => nameof(AyaLastWord);

        public override LocalizationOption LoadLocalization()
        {
            var locFiles = new LocalizationFiles(embeddedSource);
            locFiles.AddLocaleFile(Locale.En, "UltimateSkillsEn.yaml");
            return locFiles;
        }

        public override Sprite LoadSprite()
        {
            return null;
            //return ResourceLoader.LoadSprite("AyaUltG.png", embeddedSource);
        }

        public override UltimateSkillConfig MakeConfig()
        {
            var config = new UltimateSkillConfig(
                Id: "",
                Order: 10,
                PowerCost: 100,
                PowerPerLevel: 100,
                MaxPowerLevel: 2,
                RepeatableType: UsRepeatableType.OncePerTurn,
                Damage: 0,
                Value1: 0,
                Value2: 0,
                Keywords: Keyword.Accuracy,
                RelativeEffects: new List<string>() { },
                RelativeCards: new List<string>() { }
                );
            return config;
        }
    }

    [EntityLogic(typeof(AyaLastWordDef))]
    public sealed class AyaLastWord : UltimateSkill
    {
        public AyaLastWord()
        {
            TargetType = TargetType.SingleEnemy;
            GunName = "Simple1";
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector)
        {
            yield return new DamageAction(Owner, selector.GetEnemy(Battle), Damage, GunName, GunType.Single);
        }
    }
    public sealed class AyaTerukuniShiningThroughHeavenandEarthSeDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaTerukuniShiningThroughHeavenandEarthSe);
        }

        public override LocalizationOption LoadLocalization()
        {
            var locFiles = new LocalizationFiles(embeddedSource);
            locFiles.AddLocaleFile(Locale.En, "Resources.StatusEffectsEn.yaml");
            return locFiles;
        }

        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("Resources.AyaTerukuniShiningThroughHeavenandEarthSe.png", embeddedSource);
        }
        public override StatusEffectConfig MakeConfig()
        {
            var statusEffectConfig = new StatusEffectConfig(
                Id: "",
                Order: -2,
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
        [EntityLogic(typeof(AyaTerukuniShiningThroughHeavenandEarthSeDef))]
        public sealed class AyaTerukuniShiningThroughHeavenandEarthSe : StatusEffect
        {
            private bool Again = false;
            private Card card = null;
            private ManaGroup manaGroup = ManaGroup.Empty;
            private UnitSelector unitSelector = null;
            private int? activeCost = null;
            private int? upgradedActiveCost = null;
            private int? ultimateCost = null;
            private int? upgradedUltimateCost = null;
            protected override void OnAdded(Unit unit)
            {
                ReactOwnerEvent(Battle.CardUsing, new EventSequencedReactor<CardUsingEventArgs>(OnCardUsing));
                ReactOwnerEvent(Battle.CardMoving, new EventSequencedReactor<CardMovingEventArgs>(OnCardMoving));
                ReactOwnerEvent(Battle.CardExiling, new EventSequencedReactor<CardEventArgs>(OnCardExiling));
                ReactOwnerEvent(Battle.CardRemoving, new EventSequencedReactor<CardEventArgs>(OnCardRemoving));
            }
            private IEnumerable<BattleAction> OnCardUsing(CardUsingEventArgs args)
            {
                if (args.Card != card && (args.Card.CardType != CardType.Friend || (args.Card.CardType == CardType.Friend && args.Card.Summoned)))
                {
                    Again = true;
                    card = args.Card;
                    manaGroup = args.ConsumingMana;
                    unitSelector = args.Selector;
                    if (args.Card.CardType == CardType.Friend)
                    {
                        activeCost = args.Card.Config.ActiveCost;
                        upgradedActiveCost = args.Card.Config.UpgradedActiveCost;
                        ultimateCost = args.Card.Config.UltimateCost;
                        upgradedUltimateCost = args.Card.Config.UpgradedUltimateCost;
                    }
                }
                yield break;
            }
            private IEnumerable<BattleAction> OnCardMoving(CardMovingEventArgs args)
            {
                if (!Battle.BattleShouldEnd && Again && args.Card == card && !(args.SourceZone == CardZone.PlayArea && args.DestinationZone == CardZone.Hand))
                {
                    foreach (var battleAction in DoubleAction(args.Card, args))
                    {
                        yield return battleAction;
                    }
                }
                else if (Battle.BattleShouldEnd)
                {
                    yield return PerformAction.Effect(Battle.Player, "RenzhenAura", 0f, "", 0f, PerformAction.EffectBehavior.Remove, 0f);
                    Again = false;
                    card = null;
                    manaGroup = ManaGroup.Empty;
                    unitSelector = null;
                    activeCost = null;
                    upgradedActiveCost = null;
                    ultimateCost = null;
                    upgradedUltimateCost = null;
                }
            }
            private IEnumerable<BattleAction> OnCardExiling(CardEventArgs args)
            {
                if (!Battle.BattleShouldEnd && Again && args.Card == card)
                {
                    foreach (var battleAction in DoubleAction(args.Card, args))
                    {
                        yield return battleAction;
                    }
                }
                else if (Battle.BattleShouldEnd)
                {
                    yield return PerformAction.Effect(Battle.Player, "RenzhenAura", 0f, "", 0f, PerformAction.EffectBehavior.Remove, 0f);
                    Again = false;
                    card = null;
                    manaGroup = ManaGroup.Empty;
                    unitSelector = null;
                    activeCost = null;
                    upgradedActiveCost = null;
                    ultimateCost = null;
                    upgradedUltimateCost = null;
                }
            }
            private IEnumerable<BattleAction> OnCardRemoving(CardEventArgs args)
            {
                if (!Battle.BattleShouldEnd && Again && args.Card == card)
                {
                    foreach (var battleAction in DoubleAction(args.Card, args))
                    {
                        yield return battleAction;
                    }
                }
                else if (Battle.BattleShouldEnd)
                {
                    yield return PerformAction.Effect(Battle.Player, "RenzhenAura", 0f, "", 0f, PerformAction.EffectBehavior.Remove, 0f);
                    Again = false;
                    card = null;
                    manaGroup = ManaGroup.Empty;
                    unitSelector = null;
                    activeCost = null;
                    upgradedActiveCost = null;
                    ultimateCost = null;
                    upgradedUltimateCost = null;
                }
            }
            private IEnumerable<BattleAction> DoubleAction(Card card2, GameEventArgs args)
            {
                Again = false;
                Battle.MaxHand += 1;
                if (Battle.HandZone.Count >= Battle.MaxHand)
                {
                    Battle.MaxHand -= 1;
                    card = null;
                    manaGroup = ManaGroup.Empty;
                    unitSelector = null;
                    if (card2.CardType == CardType.Friend)
                    {
                        activeCost = null;
                        upgradedActiveCost = null;
                        ultimateCost = null;
                        upgradedUltimateCost = null;
                    }
                    yield break;
                }
                NotifyActivating();
                args.CancelBy(this);
                yield return new MoveCardAction(card2, CardZone.Hand);
                Battle.MaxHand -= 1;
                if (card2.Zone == CardZone.Hand)
                {
                    if (unitSelector.Type == TargetType.SingleEnemy && !unitSelector.SelectedEnemy.IsAlive)
                    {
                        unitSelector = new UnitSelector(Battle.AllAliveEnemies.Sample(GameRun.BattleRng));
                    }
                    if (card2.CardType == CardType.Friend)
                    {
                        card2.Config.ActiveCost = 0;
                        card2.Config.UpgradedActiveCost = 0;
                        if (card2.UltimateUsed || card2.FriendU.CostType == FriendCostType.Active)
                        {
                            card2.Config.UltimateCost = 0;
                            card2.Config.UpgradedUltimateCost = 0;
                        }
                        yield return CardHelper.AutoCastAction(card2, unitSelector, manaGroup);
                        card2.Config.ActiveCost = activeCost;
                        card2.Config.UpgradedActiveCost = upgradedActiveCost;
                        card2.Config.UltimateCost = ultimateCost;
                        card2.Config.UpgradedUltimateCost = upgradedUltimateCost;
                        activeCost = null;
                        upgradedActiveCost = null;
                        ultimateCost = null;
                        upgradedUltimateCost = null;
                        if (card2.Zone == CardZone.Hand && (card2.UltimateUsed || card2.Loyalty <= 0))
                        {
                            yield return new RemoveCardAction(card2);
                        }
                        else if (card2.Zone == CardZone.Hand && card2.Loyalty > 0)
                        {
                            yield return new MoveCardAction(card2, CardZone.Discard);
                        }
                    }
                    else
                    {
                        yield return CardHelper.AutoCastAction(card2, unitSelector, manaGroup);
                        if (card2.Zone == CardZone.Hand && card2.CardType == CardType.Ability)
                        {
                            yield return new RemoveCardAction(card2);
                        }
                        else if (card2.Zone == CardZone.Hand && card2.IsExile)
                        {
                            yield return new ExileCardAction(card2);
                        }
                        else if (card2.Zone == CardZone.Hand)
                        {
                            yield return new MoveCardAction(card2, CardZone.Discard);
                        }
                    }
                }
                card = null;
                manaGroup = ManaGroup.Empty;
                unitSelector = null;
                GameRun.SetHpAndMaxHp(Math.Max(GameRun.Player.Hp - 1, 1), Math.Max(GameRun.Player.MaxHp - 1, 1), true);
            }
        }
    }
    public sealed class AyaSpellBgm : BgmTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaSpellBgm);
        }

        public override UniTask<AudioClip> LoadAudioClipAsync()
        {
            return ResourceLoader.LoadAudioClip("Demetori - Tengu is Watching ~ Eye of the Needles.ogg", AudioType.OGGVORBIS, directorySource);

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
                    LoopStart: (float?)54.35,
                    LoopEnd: (float?)241,
                    TrackName: "Tengu is Watching ~ Eye of the Needles",
                    Artist: "Demetori",
                    Original: "天狗が見ている　～ Black Eyes",
                    Comment: ""
            );

            return config;
        }
    }
    /*public sealed class AyaSpellBgm : BgmTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaSpellBgm);
        }

        public override UniTask<AudioClip> LoadAudioClipAsync()
        {
            return ResourceLoader.LoadAudioClip("Demetori - The Youkai Mountain ~ Mysterious Mountain.ogg", AudioType.OGGVORBIS, directorySource);

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
                    LoopStart: (float?)0.5,
                    LoopEnd: (float?)278.1,
                    TrackName: "The Youkai Mountain ~ Mysterious Mountain",
                    Artist: "Demetori",
                    Original: "妖怪の山 ～ Mysterious Mountain",
                    Comment: ""
            );

            return config;
        }
    }*/
}
