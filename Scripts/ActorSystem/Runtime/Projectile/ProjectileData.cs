/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	Projectile
作    者:	HappLI
描    述:	飞行道具
*********************************************************************/

#if USE_FIXEDMATH
using ExternEngine;
#else
using FFloat = System.Single;
using FVector3 = UnityEngine.Vector3;
#endif
using System.Collections.Generic;
using UnityEngine;
using Framework.DrawProps;
using Framework.ActorSystem.Runtime;


#if USE_SERVER
using AudioClip = ExternEngine.AudioClip;
using Transform = ExternEngine.Transform;
#endif

#if UNITY_EDITOR
using System.Linq;
#endif

namespace Framework.ActorSystem.Runtime
{
    public enum EProjectileParabolicType
    {
        [Display("起点终点曲线")] StartEnd = 0,
        [Display("路径点曲线")] TrackPath = 1,
        [Display("路径点曲线连线终点")] TrackPathLinkEnd = 2,
    }
    public struct ProjectileKeyframe
    {
        public FFloat time;
        public FFloat speed;
        public FVector3 point;
        public FVector3 inTan;
        public FVector3 outTan;
        public ProjectileKeyframe(FFloat time, FFloat fSpeed, FVector3 point, FVector3 inTan, FVector3 outTan)
        {
            this.speed = fSpeed;
            this.time = time;
            this.point = point;
            this.inTan = inTan;
            this.outTan = outTan;
        }
    }
    public enum ELaunchFlag : uint
    {
        [Display("使用X方向")] DirX = 1 << 0,
        [Display("使用Y方向")] DirY = 1 << 1,
        [Display("使用Z方向")] DirZ = 1 << 2,
        [Display("朝向发射")] DirectionLuanch = 1 << 3,
        [Display("死亡保持")] DieKeep = 1 << 4,
        [Display("根据目标更新曲线终点")] RefreshEndPoint = 1 << 5,
        [Display("只打目标")] TrackIngoreOtherCollision = 1 << 6,
        [Disable] [Display("使用方向")] AllDir = DirX | DirY | DirZ,
    }

    public enum EBoundFlag : byte
    {
        [Display("弹射目标排除已弹射")] BoundDiscardBounded = 1 << 0,
        [Display("弹射反转")] BoundInversion = 1 << 1,
        [Display("弹射伤害+已弹次数")] BoundDamageAdd = 1 << 2,
        [Display("物理反弹")] PhysicReflectBound = 1 << 3,
    }

    public enum EProjectileType : byte
    {
        [Display("飞行器")] Projectile = 0,
        [Display("跟踪飞行器")] Track = 1,
        [Display("陷阱")] Trap = 2,
        [Display("追踪点位-xy")] TrackPoint = 3,
        [Display("飞行轨迹")] TrackPath = 4,
        [Display("弹跳弹")] Bounce = 5,
    }

    public enum EProjecitleBornType : byte
    {
        [Display("无")]
        None = 0,
        [Display("跟随发射者")]
        FollowTrigger,
        [Display("跟随目标")]
        FollowTarget,
        [Display("初始跟随目标")]
        StartTargetPos,
        [Display("初始跟随触发者")]
        StartTriggerPos,
        [Display("跟随发射时目标位置")]
        FollowThenTargetPos,
    }

    public enum EProjectileCollisionType : byte
    {
        BOX = 0,
        CAPSULE,
        NONE
    };

    [System.Serializable]
    public class ProjectileData : AttackFrameParameter
    {
        public uint id;
        [Display("类型")]
        public EProjectileType type = EProjectileType.Projectile;
        public EProjecitleBornType bornType = EProjecitleBornType.None;
        public float effectSpeed = -1;
        public string effect;
        //      public string effect_trail_wide;
        [Display("发射音效")]
        public string sound_launch;
        public Vector3[] speedLows;
        public Vector3[] speedUppers;
        public Vector3[] speedMaxs;
        public Vector3[] accelerations;
        public Vector2 speedLerp;
        public Vector3 minRotate;
        public Vector3 maxRotate;
        public EProjectileCollisionType collisionType;
        public Vector3 aabb_min;
        public Vector3 aabb_max;
        //   public float hit_rate_base;
        public float life_time = 5;
        public byte max_oneframe_hit = 1;
        public byte hit_count = 1;
        public float hit_step;
        public bool penetrable = false;
        public bool counteract;

        [Display("更新标志")]
        [DisplayEnumBit(typeof(ELaunchFlag))]
        public uint launch_flag = (int)ELaunchFlag.AllDir;

        public float explode_range;
     //   [Data.FormViewBinder("Data.CsvData_SkillDamage", "groupID")]
     //   public uint explode_damage_id;

        public float launch_delay = 0;

        [Display("弹射标志")]
        [DisplayEnumBit(typeof(EBoundFlag))]
        public ushort bound_flag = 0;
        public int bound_count = 0;
        public float bound_range = 0;
        public Vector3 bound_speed = Vector3.one;
        [DisplayName("弹射伤害")]
        //[Data.FormViewBinder("Data.CsvData_SkillDamage", "groupID")]
        //public int bound_damage_id = 0;
        //[Data.FormViewBinder("Data.CsvData_Buff", "groupID")]
        //[Display("弹射Buff")]
        //public int[] bound_buffs = null;
        [Display("弹射锁定个数")]
        public byte bound_lock_num = 0;
        [Display("弹射锁定参数1")]
        [NoListHeader]
        public int[] bound_lock_param1 = null;
        [Display("弹射锁定参数2")]
        [NoListHeader]
        public int[] bound_lock_param2 = null;
        [Display("弹射锁定参数3")]
        [NoListHeader]
        public int[] bound_lock_param3 = null;
        [Display("弹射锁定赛道")]
        [NoListHeader]
        public short[] bound_lock_rode = null;
        [Display("弹射锁定高度")]
        public bool bound_lockHeight = false;
        [DisplayName("弹射锁定最小高度")]
        public float bound_minLockHeight = 0;
        [Display("弹射锁定最大高度")]
        public float bound_maxLockHeight = 0;

        public string bound_effect = null;
        public float bound_effectSpeed = -1;
    //    [StringView("FMODUnity.EventReference", -1)]
        public string bound_sound_launch = null;
        public string bound_hit_effect = null;
    //    [StringViewGUI("FMODUnity.EventReference", -1))]
        public string bound_hit_sound = null;

        public string waring_effect = "";
        public float waring_duration = 0;

        [Display("追踪目标绑点")]
        public string[] track_target_slot;
        public Vector3 track_target_offset;

        public string explode_effect;
        public Vector3 explode_effect_offset;

        [Display("缩放")]
        public float scale = 1;
        public string desc;

        [Display("忽略场景地表检测")]
        public bool unSceneTest = false;

        [Display("分类ID")]
        public byte classify = 0;
        public static bool IsTrack(EProjectileType type)
        {
            return type == EProjectileType.Track || type == EProjectileType.TrackPoint || type == EProjectileType.TrackPath;
        }
        //------------------------------------------------------
        public bool IsValidTrackPath()
        {
            if (speedLows == null || speedMaxs == null || speedUppers == null || accelerations == null) return false;
            if (speedLows.Length != speedUppers.Length || speedUppers.Length != speedMaxs.Length || speedMaxs.Length != accelerations.Length) return false;
            if (speedLows.Length <= 0) return false;
            return true;
        }
        //------------------------------------------------------
        public void AddTrackPoint(FVector3 point, FVector3 inTan, FVector3 outTan)
        {
            List<Vector3> points = speedMaxs != null ? new List<Vector3>(speedMaxs) : new List<Vector3>();
            List<Vector3> inTans = speedLows != null ? new List<Vector3>(speedLows) : new List<Vector3>();
            List<Vector3> outTans = speedUppers != null ? new List<Vector3>(speedUppers) : new List<Vector3>();
            List<Vector3> accSpeeds = accelerations != null ? new List<Vector3>(accelerations) : new List<Vector3>();
            points.Add(point);
            inTans.Add(inTan);
            outTans.Add(outTan);
            accSpeeds.Add(Vector3.right);
            speedMaxs = points.ToArray();
            speedUppers = outTans.ToArray();
            speedLows = inTans.ToArray();
            accelerations = accSpeeds.ToArray();
        }
        //------------------------------------------------------
        public void InsertTrackPoint(int index, FVector3 point, FVector3 inTan, FVector3 outTan)
        {
            List<Vector3> points = speedMaxs != null ? new List<Vector3>(speedMaxs) : new List<Vector3>();
            List<Vector3> inTans = speedLows != null ? new List<Vector3>(speedLows) : new List<Vector3>();
            List<Vector3> outTans = speedUppers != null ? new List<Vector3>(speedUppers) : new List<Vector3>();
            List<Vector3> accSpeeds = accelerations != null ? new List<Vector3>(accelerations) : new List<Vector3>();
            if (index < 0) index = 0;
            if (index >= points.Count) index = points.Count;
            points.Insert(index, point);
            inTans.Insert(index, inTan);
            outTans.Insert(index, outTan);
            accSpeeds.Insert(index, Vector3.right);
            speedMaxs = points.ToArray();
            speedUppers = outTans.ToArray();
            speedLows = inTans.ToArray();
            accelerations = accSpeeds.ToArray();
        }
        //------------------------------------------------------
        public void RemoveTrackPoint(int index)
        {
            List<Vector3> points = speedMaxs != null ? new List<Vector3>(speedMaxs) : new List<Vector3>();
            List<Vector3> inTans = speedLows != null ? new List<Vector3>(speedLows) : new List<Vector3>();
            List<Vector3> outTans = speedUppers != null ? new List<Vector3>(speedUppers) : new List<Vector3>();
            List<Vector3> accSpeeds = accelerations != null ? new List<Vector3>(accelerations) : new List<Vector3>();
            points.RemoveAt(index);
            inTans.RemoveAt(index);
            outTans.RemoveAt(index);
            accSpeeds.RemoveAt(index);
            speedMaxs = points.ToArray();
            speedUppers = outTans.ToArray();
            speedLows = inTans.ToArray();
            accelerations = accSpeeds.ToArray();
        }
        //------------------------------------------------------
        public bool IsLaunchFlaged(ELaunchFlag flag)
        {
            return (launch_flag & (int)flag) != 0;
        }
    }
}

