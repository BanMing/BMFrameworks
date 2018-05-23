#define USING_DOTWEENING
using UnityEngine;
using System;
using System.Collections.Generic;
using LuaInterface;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using BindType = ToLuaMenu.BindType;
using System.Reflection;

public static class CustomSettings
{
    public static string saveDir = Application.dataPath + "/Scripts/Generate/";    
    public static string toluaBaseType = Application.dataPath + "/ToLua/BaseType/";    

    //导出时强制做为静态类的类型(注意customTypeList 还要添加这个类型才能导出)
    //unity 有些类作为sealed class, 其实完全等价于静态类
    public static List<Type> staticClassTypes = new List<Type>
    {        
        typeof(UnityEngine.Application),
        typeof(UnityEngine.Time),
        typeof(UnityEngine.Screen),
        typeof(UnityEngine.SleepTimeout),
        typeof(UnityEngine.Input),
        typeof(UnityEngine.Resources),
        typeof(UnityEngine.Physics),
        typeof(UnityEngine.RenderSettings),
        typeof(UnityEngine.QualitySettings),
        typeof(UnityEngine.GL),
    };

    //附加导出委托类型(在导出委托时, customTypeList 中牵扯的委托类型都会导出， 无需写在这里)
    public static DelegateType[] customDelegateList = 
    {        
        _DT(typeof(Action)),                
        _DT(typeof(UnityEngine.Events.UnityAction)),
        _DT(typeof(System.Predicate<int>)),
        _DT(typeof(System.Action<int>)),
        _DT(typeof(System.Comparison<int>)),
        _DT(typeof(DG.Tweening.TweenCallback)),
    };

    //在这里添加你要导出注册到lua的类型列表
    public static BindType[] customTypeList =
    {                

        _GT(typeof(GlobalManager)),

        _GT(typeof(ResInfo)),
        _GT(typeof(VersionInfo2)),
        _GT(typeof(VersionManager2)),

        _GT(typeof(UIWindows)),
        _GT(typeof(UIManager)),
        _GT(typeof(UIAtlasTool)),
        _GT(typeof(UIMsgBox)),
        _GT(typeof(UIFloatingMsgBox)),
        _GT(typeof(UIWindowFirstLoading)),
        _GT(typeof(UIWindowPing)),
        _GT(typeof(UIWindowUpdate)),



        _GT(typeof(ResourcesManager)),

        _GT(typeof(BufferReader)),
        _GT(typeof(BufferWriter)),
        _GT(typeof(MemoryInputStream)),
        _GT(typeof(MemoryOutputStream)),
        _GT(typeof(MemoryOutputStreamSingleton)),

        _GT(typeof(GameData)),
        _GT(typeof(ServerURLManager)),
        _GT(typeof(LoginServerConfig)),
        _GT(typeof(SystemConfig)),

        _GT(typeof(HTTPTool)),
        _GT(typeof(ScriptThread)),

        _GT(typeof(MyUnityTool)),
        _GT(typeof(MyFileUtil)),
        _GT(typeof(ZIPTool)),

        _GT(typeof(MD5Tool)),
        _GT(typeof(ReflectionTool)),

        _GT(typeof(Debugger)).SetNameSpace(null),

#if USING_DOTWEENING
        _GT(typeof(DG.Tweening.DOTween)),
        _GT(typeof(DG.Tweening.Tween)).SetBaseType(typeof(System.Object)).AddExtendType(typeof(DG.Tweening.TweenExtensions)),
        _GT(typeof(DG.Tweening.Sequence)).AddExtendType(typeof(DG.Tweening.TweenSettingsExtensions)),
        _GT(typeof(DG.Tweening.Tweener)).AddExtendType(typeof(DG.Tweening.TweenSettingsExtensions)),
        _GT(typeof(DG.Tweening.LoopType)),
        _GT(typeof(DG.Tweening.PathMode)),
        _GT(typeof(DG.Tweening.PathType)),
        _GT(typeof(DG.Tweening.RotateMode)),
        _GT(typeof(DG.Tweening.AxisConstraint)),
        _GT(typeof(Component)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        _GT(typeof(Transform)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        _GT(typeof(RectTransform)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions46)),
        _GT(typeof(Light)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        _GT(typeof(Material)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        _GT(typeof(Rigidbody)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        _GT(typeof(Camera)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        _GT(typeof(AudioSource)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
#else
                                         
        _GT(typeof(Component)),
        _GT(typeof(Transform)),
         _GT(typeof(RectTransform)),
        _GT(typeof(Material)),
        _GT(typeof(Light)),
        _GT(typeof(Rigidbody)),
        _GT(typeof(Camera)),
        _GT(typeof(AudioSource)),
#endif
        _GT(typeof(Rect)),
        _GT(typeof(UnityEngine.SceneManagement.SceneManager)),
        _GT(typeof(Behaviour)),
        _GT(typeof(MonoBehaviour)),
        _GT(typeof(GameObject)),
        _GT(typeof(TrackedReference)),
        _GT(typeof(Application)),
        _GT(typeof(Physics)),
        _GT(typeof(Collider)),
        _GT(typeof(Time)),
        _GT(typeof(Texture)),
        _GT(typeof(Sprite)),
        _GT(typeof(Texture2D)),
        _GT(typeof(Shader)),
        _GT(typeof(Renderer)),
        _GT(typeof(WWW)),
        _GT(typeof(Screen)),
        _GT(typeof(CameraClearFlags)),
        _GT(typeof(AudioClip)),
        _GT(typeof(AssetBundle)),
        _GT(typeof(AsyncOperation)).SetBaseType(typeof(System.Object)),
        _GT(typeof(LightType)),
        _GT(typeof(SleepTimeout)),
        _GT(typeof(Animator)),
        _GT(typeof(Input)),
        _GT(typeof(KeyCode)),
        _GT(typeof(SkinnedMeshRenderer)),
        _GT(typeof(Space)),


        _GT(typeof(MeshRenderer)),
        _GT(typeof(ParticleSystem)),

        _GT(typeof(BoxCollider)),
        _GT(typeof(MeshCollider)),
        _GT(typeof(SphereCollider)),
        _GT(typeof(CharacterController)),
        _GT(typeof(CapsuleCollider)),

        _GT(typeof(Animation)),
        _GT(typeof(AnimationClip)).SetBaseType(typeof(UnityEngine.Object)),
        _GT(typeof(AnimationState)),
        _GT(typeof(AnimationBlendMode)),
        _GT(typeof(QueueMode)),
        _GT(typeof(PlayMode)),
        _GT(typeof(WrapMode)),

        _GT(typeof(QualitySettings)),
        _GT(typeof(RenderSettings)),
        _GT(typeof(BlendWeights)),
        _GT(typeof(RenderTexture)),

        _GT(typeof(RuntimePlatform)),
        _GT(typeof(SystemInfo)),
        _GT(typeof(Toggle)),
        _GT(typeof(Slider)),
        _GT(typeof(Image)),
        _GT(typeof(RawImage)),
        _GT(typeof(Text)),
        _GT(typeof(Button)),
		_GT(typeof(Scrollbar)),
		_GT(typeof(Dropdown)),
		_GT(typeof(Canvas)),
		_GT(typeof(ScrollRect)),
		_GT(typeof(InputField)),
        _GT(typeof(GridLayoutGroup)),
        _GT(typeof(Util)),
        _GT(typeof(NetworkReachability)),
        _GT(typeof(cn.sharesdk.unity3d.PlatformType)),
        _GT(typeof(cn.sharesdk.unity3d.ShareContent)),
        _GT(typeof(cn.sharesdk.unity3d.ResponseState)),
        _GT(typeof(cn.sharesdk.unity3d.ContentType)),
        _GT(typeof(System.Collections.Hashtable)),
        _GT(typeof( MiniJSON)),
        _GT(typeof(System.Net.IPAddress)),
        _GT(typeof(TweenAlpha)),
        _GT(typeof(TweenColor)),
        _GT(typeof(TweenPosition)),
        _GT(typeof(TweenFOV)),
        _GT(typeof(TweenRotation)),
        _GT(typeof(TweenScale)),
        _GT(typeof(TweenVolume)),
        _GT(typeof(TweenOrthoSize)),
        _GT(typeof(TweenTransform)),
        _GT(typeof(NGUITools)),
        _GT(typeof(RealTime)),
        _GT(typeof(UITweener)),
        _GT(typeof(ToggleGroup)),
        _GT(typeof(PointerEventData)),
        _GT(typeof(System.DateTime)),
        _GT(typeof(System.DayOfWeek)),
        _GT(typeof(System.TimeSpan)),
        _GT(typeof(System.Convert)),
        _GT(typeof(UnityEngine.Events.UnityEvent)),
        _GT(typeof(System.Collections.ArrayList)),
        _GT(typeof(Consts)),
        _GT(typeof(System.Xml.Linq.XElement)),
        _GT(typeof(LuaDebugTool)),
        _GT(typeof(LuaValueInfo)),

   };

    public static List<Type> dynamicList = new List<Type>()
    {
        typeof(MeshRenderer),
        typeof(ParticleSystem),

        typeof(BoxCollider),
        typeof(MeshCollider),
        typeof(SphereCollider),
        typeof(CharacterController),
        typeof(CapsuleCollider),

        typeof(Animation),
        typeof(AnimationClip),
        typeof(AnimationState),

        typeof(BlendWeights),
        typeof(RenderTexture),
        typeof(Rigidbody),
    };

    //重载函数，相同参数个数，相同位置out参数匹配出问题时, 需要强制匹配解决
    //使用方法参见例子14
    public static List<Type> outList = new List<Type>()
    {
        
    };

    public static BindType _GT(Type t)
    {
        return new BindType(t);
    }

    public static DelegateType _DT(Type t)
    {
        return new DelegateType(t);
    }    
}
