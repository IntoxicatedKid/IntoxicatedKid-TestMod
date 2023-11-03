using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LBoL.Base;
using LBoL.ConfigData;
using UnityEngine;

namespace test.AllyUnits
{
    // Token: 0x02000008 RID: 8
    public sealed class AllyGroupConfig
    {
        // Token: 0x060000CF RID: 207 RVA: 0x0000461C File Offset: 0x0000281C
        private AllyGroupConfig(string Id, string Name, string FormationName, IReadOnlyList<string> Enemies, EnemyType EnemyType, float DebutTime, bool RollBossExhibit, Vector2 PlayerRoot, string PreBattleDialogName, string PostBattleDialogName)
        {
            this.Id = Id;
            this.Name = Name;
            this.FormationName = FormationName;
            this.Enemies = Enemies;
            this.EnemyType = EnemyType;
            this.DebutTime = DebutTime;
            this.RollBossExhibit = RollBossExhibit;
            this.PlayerRoot = PlayerRoot;
            this.PreBattleDialogName = PreBattleDialogName;
            this.PostBattleDialogName = PostBattleDialogName;
        }

        // Token: 0x17000048 RID: 72
        // (get) Token: 0x060000D0 RID: 208 RVA: 0x0000467C File Offset: 0x0000287C
        // (set) Token: 0x060000D1 RID: 209 RVA: 0x00004684 File Offset: 0x00002884
        public string Id { get; private set; }

        // Token: 0x17000049 RID: 73
        // (get) Token: 0x060000D2 RID: 210 RVA: 0x0000468D File Offset: 0x0000288D
        // (set) Token: 0x060000D3 RID: 211 RVA: 0x00004695 File Offset: 0x00002895
        public string Name { get; private set; }

        // Token: 0x1700004A RID: 74
        // (get) Token: 0x060000D4 RID: 212 RVA: 0x0000469E File Offset: 0x0000289E
        // (set) Token: 0x060000D5 RID: 213 RVA: 0x000046A6 File Offset: 0x000028A6
        public string FormationName { get; private set; }

        // Token: 0x1700004B RID: 75
        // (get) Token: 0x060000D6 RID: 214 RVA: 0x000046AF File Offset: 0x000028AF
        // (set) Token: 0x060000D7 RID: 215 RVA: 0x000046B7 File Offset: 0x000028B7
        public IReadOnlyList<string> Enemies { get; private set; }

        // Token: 0x1700004C RID: 76
        // (get) Token: 0x060000D8 RID: 216 RVA: 0x000046C0 File Offset: 0x000028C0
        // (set) Token: 0x060000D9 RID: 217 RVA: 0x000046C8 File Offset: 0x000028C8
        public EnemyType EnemyType { get; private set; }

        // Token: 0x1700004D RID: 77
        // (get) Token: 0x060000DA RID: 218 RVA: 0x000046D1 File Offset: 0x000028D1
        // (set) Token: 0x060000DB RID: 219 RVA: 0x000046D9 File Offset: 0x000028D9
        public float DebutTime { get; private set; }

        // Token: 0x1700004E RID: 78
        // (get) Token: 0x060000DC RID: 220 RVA: 0x000046E2 File Offset: 0x000028E2
        // (set) Token: 0x060000DD RID: 221 RVA: 0x000046EA File Offset: 0x000028EA
        public bool RollBossExhibit { get; private set; }

        // Token: 0x1700004F RID: 79
        // (get) Token: 0x060000DE RID: 222 RVA: 0x000046F3 File Offset: 0x000028F3
        // (set) Token: 0x060000DF RID: 223 RVA: 0x000046FB File Offset: 0x000028FB
        public Vector2 PlayerRoot { get; private set; }

        // Token: 0x17000050 RID: 80
        // (get) Token: 0x060000E0 RID: 224 RVA: 0x00004704 File Offset: 0x00002904
        // (set) Token: 0x060000E1 RID: 225 RVA: 0x0000470C File Offset: 0x0000290C
        public string PreBattleDialogName { get; private set; }

        // Token: 0x17000051 RID: 81
        // (get) Token: 0x060000E2 RID: 226 RVA: 0x00004715 File Offset: 0x00002915
        // (set) Token: 0x060000E3 RID: 227 RVA: 0x0000471D File Offset: 0x0000291D
        public string PostBattleDialogName { get; private set; }

        // Token: 0x060000E4 RID: 228 RVA: 0x00004726 File Offset: 0x00002926
        public static IReadOnlyList<AllyGroupConfig> AllConfig()
        {
            ConfigDataManager.Initialize();
            return Array.AsReadOnly(_data);
        }

        // Token: 0x060000E5 RID: 229 RVA: 0x00004738 File Offset: 0x00002938
        public static AllyGroupConfig FromId(string Id)
        {
            ConfigDataManager.Initialize();
            AllyGroupConfig AllyGroupConfig;
            return !_IdTable.TryGetValue(Id, out AllyGroupConfig) ? null : AllyGroupConfig;
        }

        // Token: 0x060000E6 RID: 230 RVA: 0x00004764 File Offset: 0x00002964
        public override string ToString()
        {
            string[] array = new string[21];
            array[0] = "{AllyGroupConfig Id=";
            array[1] = ConfigDataManager.System_String.ToString(Id);
            array[2] = ", Name=";
            array[3] = ConfigDataManager.System_String.ToString(Name);
            array[4] = ", FormationName=";
            array[5] = ConfigDataManager.System_String.ToString(FormationName);
            array[6] = ", Enemies=[";
            array[7] = string.Join(", ", Enemies.Select((v1) => ConfigDataManager.System_String.ToString(v1)));
            array[8] = "], EnemyType=";
            array[9] = EnemyType.ToString();
            array[10] = ", DebutTime=";
            array[11] = ConfigDataManager.System_Single.ToString(DebutTime);
            array[12] = ", RollBossExhibit=";
            array[13] = ConfigDataManager.System_Boolean.ToString(RollBossExhibit);
            array[14] = ", PlayerRoot=";
            array[15] = ConfigDataManager.UnityEngine_Vector2.ToString(PlayerRoot);
            array[16] = ", PreBattleDialogName=";
            array[17] = ConfigDataManager.System_String.ToString(PreBattleDialogName);
            array[18] = ", PostBattleDialogName=";
            array[19] = ConfigDataManager.System_String.ToString(PostBattleDialogName);
            array[20] = "}";
            return string.Concat(array);
        }

        // Token: 0x060000E7 RID: 231 RVA: 0x000048C8 File Offset: 0x00002AC8
        private static void Load(byte[] bytes)
        {
            using (BinaryReader binaryReader = new BinaryReader(new MemoryStream(bytes)))
            {
                AllyGroupConfig[] array = new AllyGroupConfig[binaryReader.ReadInt32()];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = new AllyGroupConfig(ConfigDataManager.System_String.ReadFrom(binaryReader), ConfigDataManager.System_String.ReadFrom(binaryReader), ConfigDataManager.System_String.ReadFrom(binaryReader), ConfigDataManager.ReadList(binaryReader, (r1) => ConfigDataManager.System_String.ReadFrom(r1)), (EnemyType)binaryReader.ReadInt32(), ConfigDataManager.System_Single.ReadFrom(binaryReader), ConfigDataManager.System_Boolean.ReadFrom(binaryReader), ConfigDataManager.UnityEngine_Vector2.ReadFrom(binaryReader), ConfigDataManager.System_String.ReadFrom(binaryReader), ConfigDataManager.System_String.ReadFrom(binaryReader));
                }
                _data = array;
                _IdTable = _data.ToDictionary((elem) => elem.Id);
            }
        }

        // Token: 0x060000E8 RID: 232 RVA: 0x000049E0 File Offset: 0x00002BE0
        internal static void Reload()
        {
            TextAsset textAsset = Resources.Load<TextAsset>("ConfigData/AllyGroupConfig");
            if (textAsset != null)
            {
                try
                {
                    Load(textAsset.bytes);
                }
                catch (Exception ex)
                {
                    Debug.LogError(string.Format("Failed to load AllyGroupConfig data: {0}, try reimport config data", ex.Message));
                }
                Resources.UnloadAsset(textAsset);
            }
            else
            {
                Debug.LogError("Cannot load config data of 'AllyGroupConfig', please reimport config data");
            }
        }

        // Token: 0x060000E9 RID: 233 RVA: 0x00004A54 File Offset: 0x00002C54
        // Note: this type is marked as 'beforefieldinit'.
        static AllyGroupConfig()
        {
        }

        // Token: 0x0400006E RID: 110
        private static AllyGroupConfig[] _data = Array.Empty<AllyGroupConfig>();

        // Token: 0x0400006F RID: 111
        private static Dictionary<string, AllyGroupConfig> _IdTable = _data.ToDictionary((elem) => elem.Id);
    }
}
