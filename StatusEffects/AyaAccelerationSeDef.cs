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
using LBoL.EntityLib.StatusEffects.Enemy;

namespace test.StatusEffects
{
    public sealed class AyaAccelerationSeDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaAccelerationSe);
        }

        public override LocalizationOption LoadLocalization()
        {
            var locFiles = new LocalizationFiles(embeddedSource);
            locFiles.AddLocaleFile(Locale.En, "Resources.StatusEffectsEn.yaml");
            return locFiles;
        }

        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("Resources.AyaAccelerationSe.png", embeddedSource);
        }
        public override StatusEffectConfig MakeConfig()
        {
            var statusEffectConfig = new StatusEffectConfig(
                Id: "",
                Order: 1,
                Type: StatusEffectType.Positive,
                IsVerbose: false,
                IsStackable: true,
                StackActionTriggerLevel: null,
                HasLevel: true,
                LevelStackType: StackType.Add,
                HasDuration: false,
                DurationStackType: StackType.Add,
                DurationDecreaseTiming: DurationDecreaseTiming.Custom,
                HasCount: false,
                CountStackType: StackType.Keep,
                LimitStackType: StackType.Keep,
                ShowPlusByLimit: false,
                Keywords: Keyword.None,
                RelativeEffects: new List<string>() { "AyaEvasionSe" },
                VFX: "Default",
                VFXloop: "Default",
                SFX: "Default"
            );
            return statusEffectConfig;
        }


        [EntityLogic(typeof(AyaAccelerationSeDef))]
        public sealed class AyaAccelerationSe : StatusEffect
        {
            int TurnAcceleration = 0;
            protected override void OnAdded(Unit unit)
            {
                HandleOwnerEvent(Owner.BlockShieldGaining, delegate (BlockShieldEventArgs args)
                {
                    if (args.Type == BlockShieldType.Direct)
                    {
                        return;
                    }
                    ActionCause cause = args.Cause;
                    if (cause == ActionCause.Card || cause == ActionCause.OnlyCalculate || cause == ActionCause.Us)
                    {
                        if (args.Shield != 0f)
                        {
                            args.Shield = Math.Max(args.Shield - Level, 0f);
                        }
                        args.AddModifier(this);
                    }
                });
                HandleOwnerEvent(Owner.TurnStarting, new GameEventHandler<UnitEventArgs>(OnOwnerTurnStarting));
                HandleOwnerEvent(Owner.StatusEffectAdded, new GameEventHandler<StatusEffectApplyEventArgs>(OnOwnerStatusEffectAdded));
            }
            private void OnOwnerTurnStarting(UnitEventArgs args)
            {
                TurnAcceleration = 0;
                int num = (int)Math.Truncate((float)Level / 4);
                if (Owner.HasStatusEffect<AyaEvasionSeDef.AyaEvasionSe>())
                {
                    if (num > 0)
                    {
                        Owner.GetStatusEffect<AyaEvasionSeDef.AyaEvasionSe>().Level = num;
                    }
                    else
                    {
                        React(new RemoveStatusEffectAction(Owner.GetStatusEffect<AyaEvasionSeDef.AyaEvasionSe>(), true));
                    }
                }
                else if (num > 0)
                {
                    React(new ApplyStatusEffectAction<AyaEvasionSeDef.AyaEvasionSe>(Owner, num, null, null, null, 0f, true));
                }
            }
            private void OnOwnerStatusEffectAdded(StatusEffectApplyEventArgs args)
            {
                if (args.Effect is AyaAccelerationSe)
                {
                    TurnAcceleration += args.Effect.Level;
                    if (TurnAcceleration >= 4)
                    {
                        int num = (int)Math.Truncate((float)TurnAcceleration / 4);
                        React(new ApplyStatusEffectAction<AyaEvasionSeDef.AyaEvasionSe>(Owner, num, null, null, null, 0f, true));
                        TurnAcceleration -= num * 4;
                    }
                }
            }
        }
    }
}