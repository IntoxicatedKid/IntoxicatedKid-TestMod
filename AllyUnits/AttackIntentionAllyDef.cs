/*using System;
using JetBrains.Annotations;
using LBoL.Core;
using LBoL.Core.Units;

namespace test.AllyUnits
{
    // Token: 0x020000E9 RID: 233
    [UsedImplicitly]
    public sealed class AttackIntentionAllyDef : Intention
    {
        // Token: 0x170002B0 RID: 688
        // (get) Token: 0x060008D1 RID: 2257 RVA: 0x00019667 File Offset: 0x00017867
        public override IntentionType Type
        {
            get
            {
                return IntentionType.Attack;
            }
        }

        // Token: 0x170002B1 RID: 689
        // (get) Token: 0x060008D2 RID: 2258 RVA: 0x0001966A File Offset: 0x0001786A
        private string MultiDamageDescription
        {
            get
            {
                return LocalizeProperty("MultiDamageDescription", true, true);
            }
        }

        // Token: 0x170002B2 RID: 690
        // (get) Token: 0x060008D3 RID: 2259 RVA: 0x00019679 File Offset: 0x00017879
        private string AccurateDescription
        {
            get
            {
                return LocalizeProperty("AccurateDescription", true, true);
            }
        }

        // Token: 0x170002B3 RID: 691
        // (get) Token: 0x060008D4 RID: 2260 RVA: 0x00019688 File Offset: 0x00017888
        private string AccurateMultiDamageDescription
        {
            get
            {
                return LocalizeProperty("AccurateMultiDamageDescription", true, true);
            }
        }

        // Token: 0x170002B4 RID: 692
        // (get) Token: 0x060008D5 RID: 2261 RVA: 0x00019697 File Offset: 0x00017897
        // (set) Token: 0x060008D6 RID: 2262 RVA: 0x0001969F File Offset: 0x0001789F
        public DamageInfo Damage { get; internal set; }

        // Token: 0x170002B5 RID: 693
        // (get) Token: 0x060008D7 RID: 2263 RVA: 0x000196A8 File Offset: 0x000178A8
        // (set) Token: 0x060008D8 RID: 2264 RVA: 0x000196B0 File Offset: 0x000178B0
        public int? Times { get; internal set; }

        // Token: 0x170002B6 RID: 694
        // (get) Token: 0x060008D9 RID: 2265 RVA: 0x000196B9 File Offset: 0x000178B9
        // (set) Token: 0x060008DA RID: 2266 RVA: 0x000196C1 File Offset: 0x000178C1
        public bool IsAccuracy { get; internal set; }

        // Token: 0x170002B7 RID: 695
        // (get) Token: 0x060008DB RID: 2267 RVA: 0x000196CC File Offset: 0x000178CC
        public string DamageText
        {
            get
            {
                int? times = Times;
                int num = 1;
                if (!(times.GetValueOrDefault() > num & times != null))
                {
                    return CalculateDamage(Damage).ToString();
                }
                return CalculateDamage(Damage).ToString() + "x" + Times.ToString();
            }
        }

        // Token: 0x060008DC RID: 2268 RVA: 0x00019740 File Offset: 0x00017940
        protected override string GetBaseDescription()
        {
            if (!IsAccuracy)
            {
                int? num = Times;
                int num2 = 1;
                if (!(num.GetValueOrDefault() > num2 & num != null))
                {
                    return BaseDescription;
                }
                return MultiDamageDescription;
            }
            else
            {
                int? num = Times;
                int num2 = 1;
                if (!(num.GetValueOrDefault() > num2 & num != null))
                {
                    return AccurateDescription;
                }
                return AccurateMultiDamageDescription;
            }
        }

        // Token: 0x060008DD RID: 2269 RVA: 0x000197AA File Offset: 0x000179AA
        public AttackIntentionAllyDef()
        {
        }
    }
}
*/