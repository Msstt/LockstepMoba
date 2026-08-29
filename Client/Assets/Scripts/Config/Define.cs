// TODO 预加载

using Combat.Actor;
using Combat.Area;
using Combat.Buff;
using Combat.Equipment;
using Combat.Skill;
using Framework;
using UnityEngine;

public static partial class Config {
    public static readonly ConfigCache<ChampionConfig> Champion = new(
        (id) => Resources.Load<ChampionConfig>("Config/Actor/Champion/" + id));
    public static readonly ConfigCache<MinionConfig> Minion = new(
        (id) => Resources.Load<MinionConfig>("Config/Actor/Minion/" + id));
    public static readonly ConfigCache<TurretConfig> Turret = new(
        (id) => Resources.Load<TurretConfig>("Config/Actor/Turret/" + id));
    
    public static readonly ConfigCache<EquipmentConfig> Equipment = new(
        (id) => Resources.Load<EquipmentConfig>("Config/Equipment/" + id));
    
    public static readonly ConfigCache<SkillConfig> Skill = new(
        (id) => JsonHelper.LoadFromRes("Config/Skill/Json/" + id, out SkillConfig config) ? config : null);
    
    public static readonly ConfigCache<BuffConfig> Buff = new(
        (id) => JsonHelper.LoadFromRes("Config/Buff/Json/" + id, out BuffConfig config) ? config : null);
    
    public static readonly ConfigCache<AreaConfig> Area = new(
        (id) => JsonHelper.LoadFromRes("Config/Area/Json/" + id, out AreaConfig config) ? config : null);
    
    public static readonly OtherConfig.Time Time = Resources.Load<OtherConfig.Time>("Config/Other/Time");
    public static OtherConfig.Map Map = Resources.Load<OtherConfig.Map>("Config/Other/Map");
    public static readonly OtherConfig.Exp Exp = Resources.Load<OtherConfig.Exp>("Config/Other/Exp");
    public static readonly OtherConfig.Vision Vision = Resources.Load<OtherConfig.Vision>("Config/Other/Vision");
    public static readonly OtherConfig.MinionWave MinionWave = Resources.Load<OtherConfig.MinionWave>("Config/Other/MinionWave");
}