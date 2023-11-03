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

namespace test.StatusEffects
{
    public sealed class AyaPerfectEvasionSeDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaPerfectEvasionSe);
        }

        public override LocalizationOption LoadLocalization()
        {
            var locFiles = new LocalizationFiles(embeddedSource);
            locFiles.AddLocaleFile(Locale.En, "Resources.StatusEffectsEn.yaml");
            return locFiles;
        }

        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("Resources.AyaPerfectEvasionSe.png", embeddedSource);
        }
        public override StatusEffectConfig MakeConfig()
        {
            var statusEffectConfig = new StatusEffectConfig(
                Id: "",
                Order: 1,
                Type: StatusEffectType.Positive,
                IsVerbose: true,
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
                RelativeEffects: new List<string>() { "AyaEvasionSe", "Graze" },
                VFX: "Graze",
                VFXloop: "Default",
                SFX: "Graze"
            );
            return statusEffectConfig;
        }


        [EntityLogic(typeof(AyaPerfectEvasionSeDef))]
        public sealed class AyaPerfectEvasionSe : StatusEffect
        {
            [HarmonyPatch(typeof(Unit), nameof(Unit.MeasureDamage))]
            class Unit_MeasureDamage_Patch
            {

                static bool Prefix(Unit __instance, ref DamageInfo info, ref DamageInfo __result)
                {
                    if (__instance.HasStatusEffect<AyaPerfectEvasionSe>() && info.DamageType == DamageType.Attack && info.Damage.Round(MidpointRounding.AwayFromZero) > 0f && info.IsAccuracy)
                    {
                        __result = new DamageInfo(0f, info.DamageType, true, info.IsAccuracy).BlockBy(__instance.Block).ShieldBy(__instance.Shield);
                        return false;
                    }
                    return true;
                }



            }

            [HarmonyPatch(typeof(Unit), nameof(Unit.TakeDamage))]
            class Unit_TakeDamage_Patch
            {

                static int CheckGraze(Unit unit)
                {
                    // cheeky way to check specifically for this situation
                    return unit.HasStatusEffect<Graze>() ? (int)DamageType.Attack : -1;
                }

                static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
                {
                    instructions.Where(i => i.opcode == OpCodes.Ldarga_S).Do(i => log.LogDebug(i.operand.GetType()));

                    return new CodeMatcher(instructions, generator)
                        // remove error message
                        .MatchForward(true, new CodeMatch[] { new CodeMatch(OpCodes.Ldstr, "Taking grazed accurary DamageInfo {0}."), OpCodes.Ldarg_1, OpCodes.Box, OpCodes.Call, OpCodes.Call })
                        .Set(OpCodes.Pop, null)
                        // check if unit has Graze
                        .MatchForward(true, new CodeMatch[] { OpCodes.Ldarga_S, new CodeMatch(OpCodes.Call, AccessTools.PropertyGetter(typeof(DamageInfo), nameof(DamageInfo.DamageType))), OpCodes.Ldc_I4_2 })
                        .Set(OpCodes.Call, AccessTools.Method(typeof(Unit_TakeDamage_Patch), nameof(CheckGraze)))
                        .Insert(new CodeInstruction(OpCodes.Ldarg_0))

                        .InstructionEnumeration();
                }



                static void Postfix(Unit __instance, ref DamageInfo info)
                {
                    if (info.DamageType == DamageType.Attack && __instance.HasStatusEffect<AyaPerfectEvasionSe>())
                    {
                        if (info.IsGrazed)
                        {
                            __instance.GetStatusEffect<AyaPerfectEvasionSe>().Activate();
                        }
                    }
                }
            }
            [HarmonyPatch(typeof(AyaEvasionSe), nameof(AyaEvasionSe.Activate))]
            class AyaEvasionSe_Activate_Patch
            {
                static bool Prefix(StatusEffect __instance)
                {
                    if (__instance.Owner.HasStatusEffect<AyaPerfectEvasionSe>())
                    {
                        return false;
                    }
                    return true;
                }
            }
            [HarmonyPatch(typeof(AyaEvasionSe), nameof(AyaEvasionSe.BeenAccurate))]
            class AyaEvasionSe_BeenAccurate_Patch
            {
                static bool Prefix(StatusEffect __instance)
                {
                    if (__instance.Owner.HasStatusEffect<AyaPerfectEvasionSe>())
                    {
                        return false;
                    }
                    return true;
                }
            }
            [HarmonyPatch(typeof(Graze), nameof(Graze.Activate))]
            class Graze_Activate_Patch
            {
                static bool Prefix(StatusEffect __instance)
                {
                    if (__instance.Owner.HasStatusEffect<AyaPerfectEvasionSe>())
                    {
                        return false;
                    }
                    return true;
                }
            }
            [HarmonyPatch(typeof(Graze), nameof(Graze.BeenAccurate))]
            class Graze_BeenAccurate_Patch
            {
                static bool Prefix(StatusEffect __instance)
                {
                    if (__instance.Owner.HasStatusEffect<AyaPerfectEvasionSe>())
                    {
                        return false;
                    }
                    return true;
                }
            }
            protected override void OnAdded(Unit unit)
            {
                HandleOwnerEvent(Owner.TurnStarted, new GameEventHandler<UnitEventArgs>(OnOwnerTurnStarted));
            }
            private void OnOwnerTurnStarted(UnitEventArgs args)
            {
                if (IsAutoDecreasing)
                {
                    int num = Level - 1;
                    Level = num;
                    if (Level == 0)
                    {
                        React(new RemoveStatusEffectAction(this, true));
                        return;
                    }
                }
                else
                {
                    IsAutoDecreasing = true;
                }
            }
            public void Activate()
            {
                int num = Level - 1;
                Level = num;
                if (Level > 0)
                {
                    NotifyActivating();
                    return;
                }
                React(new RemoveStatusEffectAction(this, true));
            }
            public override string UnitEffectName
            {
                get
                {
                    return "GrazeLoop";
                }
            }
        }
    }
}