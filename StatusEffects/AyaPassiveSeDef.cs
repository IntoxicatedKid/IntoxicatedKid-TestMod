using LBoL.ConfigData;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System;
using System.Collections.Generic;
using System.Text;
using static test.BepinexPlugin;
using UnityEngine;
using LBoL.Core;
using LBoL.Base;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Base.Extensions;
using System.Collections;
using LBoL.Presentation;
using LBoL.EntityLib.Cards.Neutral.Blue;
using HarmonyLib;
using LBoL.Core.StatusEffects;
using UnityEngine.Rendering;
using LBoL.Core.Units;
using LBoL.EntityLib.Exhibits.Shining;
using Mono.Cecil;
using JetBrains.Annotations;
using System.Linq;
using LBoL.EntityLib.StatusEffects.Neutral.Black;
using LBoL.EntityLib.PlayerUnits;
using LBoL.EntityLib.Cards.Character.Sakuya;
using LBoL.EntityLib.Cards.Other.Enemy;
using LBoL.EntityLib.StatusEffects.Cirno;
using System.Reflection;
using System.Reflection.Emit;
using static test.StatusEffects.AyaEvasionSeDef;
using test.PlayerUnits;
using System.Runtime.CompilerServices;
using LBoL.EntityLib.StatusEffects.Enemy;

namespace test.StatusEffects
{
    public sealed class AyaPassiveSeDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaPassiveSe);
        }

        public override LocalizationOption LoadLocalization()
        {
            var locFiles = new LocalizationFiles(embeddedSource);
            locFiles.AddLocaleFile(Locale.En, "Resources.StatusEffectsEn.yaml");
            return locFiles;
        }

        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("Resources.AyaPassiveSe.png", embeddedSource);
        }
        public override StatusEffectConfig MakeConfig()
        {
            var statusEffectConfig = new StatusEffectConfig(
                Id: "",
                Order: -1,
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
                Keywords: Keyword.Exile | Keyword.Ethereal | Keyword.Replenish,
                RelativeEffects: new List<string>() { },
                VFX: "Default",
                VFXloop: "Default",
                SFX: "Default"
            );
            return statusEffectConfig;
        }


        [EntityLogic(typeof(AyaPassiveSeDef))]
        public sealed class AyaPassiveSe : StatusEffect
        {
            protected override void OnAdded(Unit unit)
            {
                foreach (Card card in Battle.EnumerateAllCards())
                {
                    if (card.CardType == CardType.Status && card.DebugName.Contains("News"))
                    {
                        card.DeltaDamage += 10;
                        card.IsEthereal = true;
                        card.IsReplenish = true;
                    }
                }
                HandleOwnerEvent(Battle.CardsAddedToDiscard, new GameEventHandler<CardsEventArgs>(OnAddCard));
                HandleOwnerEvent(Battle.CardsAddedToHand, new GameEventHandler<CardsEventArgs>(OnAddCard));
                HandleOwnerEvent(Battle.CardsAddedToExile, new GameEventHandler<CardsEventArgs>(OnAddCard));
                HandleOwnerEvent(Battle.CardsAddedToDrawZone, new GameEventHandler<CardsAddingToDrawZoneEventArgs>(OnAddCardToDraw));
            }
            private void OnAddCard(CardsEventArgs args)
            {
                foreach (Card card in args.Cards)
                {
                    if (card.CardType == CardType.Status && card.DebugName.Contains("News"))
                    {
                        card.DeltaDamage += 10;
                        card.IsEthereal = true;
                        card.IsReplenish = true;
                    }
                }
            }
            private void OnAddCardToDraw(CardsAddingToDrawZoneEventArgs args)
            {
                foreach (Card card in args.Cards)
                {
                    if (card.CardType == CardType.Status && card.DebugName.Contains("News"))
                    {
                        card.DeltaDamage += 10;
                        card.IsEthereal = true;
                        card.IsReplenish = true;
                    }
                }
            }
        }
    }
}