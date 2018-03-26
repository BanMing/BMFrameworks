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
        //------------------------为例子导出--------------------------------
        //_GT(typeof(TestEventListener)),
        //_GT(typeof(TestProtol)),
        //_GT(typeof(TestAccount)),
        //_GT(typeof(Dictionary<int, TestAccount>)).SetLibName("AccountMap"),
        //_GT(typeof(KeyValuePair<int, TestAccount>)),    
        //_GT(typeof(TestExport)),
        //_GT(typeof(TestExport.Space)),
        //-------------------------------------------------------------------        
        
        _GT(typeof(LuaDynamicImportTool)),

        _GT(typeof(LogTool)),

        // _GT(typeof(GlobalManager)),

        _GT(typeof(ResInfo)),
        _GT(typeof(VersionInfo2)),
        _GT(typeof(VersionManager2)),

        _GT(typeof(UIWindows)),
        _GT(typeof(UIManager)),
        _GT(typeof(UIAtlasTool)),
        // _GT(typeof(UIMsgBox)),
        // _GT(typeof(UIFloatingMsgBox)),
        // _GT(typeof(UIWindowFirstLoading)),
        // _GT(typeof(UIWindowPing)),
        // _GT(typeof(UIWindowUpdate)),
        // _GT(typeof(LoadingBackground)),

        // _GT(typeof(VoiceChatManager)),

        //_GT(typeof(TCP_Info)),
        //_GT(typeof(TCP_Command)),
        // _GT(typeof(TCP_Head)),
        //_GT(typeof(TCP_Buffer)),
        // _GT(typeof(TCPClientWorker)),
        // _GT(typeof(NetManager)),
        // _GT(typeof(NetworkHelper)),

        _GT(typeof(ResourcesManager)),
        // _GT(typeof(TimeManager)),
        // _GT(typeof(GameManager)),

        _GT(typeof(EventServer)),
        _GT(typeof(ThreadCommunicationEventServer)),
        _GT(typeof(EventDispatcher)),

        _GT(typeof(BufferReader)),
        _GT(typeof(BufferWriter)),
        _GT(typeof(MemoryInputStream)),
        _GT(typeof(MemoryOutputStream)),
        _GT(typeof(MemoryOutputStreamSingleton)),

        _GT(typeof(GameData)),
        _GT(typeof(CSVParser)),
        _GT(typeof(ServerURLManager)),
        _GT(typeof(LoginServerConfig)),
        _GT(typeof(SystemConfig)),
        _GT(typeof(LocalData)),
		_GT(typeof(LocalDataV)),

        _GT(typeof(HTTPTool)),
        _GT(typeof(IPTool)),
        _GT(typeof(ScriptThread)),

        _GT(typeof(MyUnityTool)),
        _GT(typeof(MyFileUtil)),
        _GT(typeof(ZIPTool)),

        _GT(typeof(MD5Tool)),
        _GT(typeof(ReflectionTool)),
        // _GT(typeof(EmojiTextManager)),

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
        // _GT(typeof(RectTransform)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions46)),
        _GT(typeof(Light)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        _GT(typeof(Material)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        _GT(typeof(Rigidbody)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        _GT(typeof(Camera)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        _GT(typeof(AudioSource)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        //_GT(typeof(LineRenderer)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        //_GT(typeof(TrailRenderer)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),    
#else
                                         
        _GT(typeof(Component)),
        _GT(typeof(Transform)),
        _GT(typeof(Material)),
        _GT(typeof(Light)),
        _GT(typeof(Rigidbody)),
        _GT(typeof(Camera)),
        _GT(typeof(AudioSource)),
        //_GT(typeof(LineRenderer))
        //_GT(typeof(TrailRenderer))
        _GT(typeof(RectTransform)),
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
		_GT(typeof(WWWForm)),
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
        _GT(typeof(MeshFilter)),
        _GT(typeof(Mesh)),

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
        _GT(typeof(Toggle.ToggleEvent)),
        // _GT(typeof(TabGroup)),
        _GT(typeof(ItemList)),
        _GT(typeof(Slider)),
        _GT(typeof(Slider.SliderEvent)),
        _GT(typeof(Image)),
        _GT(typeof(RawImage)),
        _GT(typeof(Text)),
		_GT(typeof(TextAnchor)),
        _GT(typeof(Button)),
        _GT(typeof(Button.ButtonClickedEvent)),
		_GT(typeof(Scrollbar)),
		_GT(typeof(Scrollbar.ScrollEvent)),
        _GT(typeof(Dropdown)),
        _GT(typeof(Dropdown.DropdownEvent)),
        _GT(typeof(Canvas)),
        _GT(typeof(RenderMode)),
		_GT(typeof(ScrollRect)),
		_GT(typeof(InputField)),
		_GT(typeof(InputField.OnChangeEvent)),
		_GT(typeof(InputField.SubmitEvent)),
        _GT(typeof(GridLayoutGroup)),
        _GT(typeof(HorizontalLayoutGroup)),
		_GT(typeof(LayoutElement)),
        _GT(typeof(Util)),
		_GT(typeof(XMLUtils)),
        _GT(typeof(UIEventListener)),
        _GT(typeof(UIClickListener)),
        _GT(typeof(UIUtil)),
        _GT(typeof(UIAnimation)),
        _GT(typeof(NetworkReachability)),
        // _GT(typeof(MyShareSDK)),
        _GT(typeof(cn.sharesdk.unity3d.PlatformType)),
        _GT(typeof(cn.sharesdk.unity3d.ShareContent)),
        _GT(typeof(cn.sharesdk.unity3d.ResponseState)),
        _GT(typeof(cn.sharesdk.unity3d.ContentType)),
        _GT(typeof(System.Collections.Hashtable)),
        _GT(typeof( MiniJSON)),
        _GT(typeof(System.Net.IPAddress)),
        _GT(typeof(LightColorController)),
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
        _GT(typeof(BatteryLevel)),
        _GT(typeof(MonoBehaviourDriver)),
        _GT(typeof(PointerEventData)),
        _GT(typeof(System.DateTime)),
        _GT(typeof(System.DayOfWeek)),
        _GT(typeof(System.TimeSpan)),
        _GT(typeof(System.Convert)),
        _GT(typeof(UnityEngine.Events.UnityEvent)),
        _GT(typeof(System.Collections.ArrayList)),
        _GT(typeof(Location)),
        // _GT(typeof(System.Text.StringBuilder)),
        _GT(typeof(System.Xml.Linq.XElement)),
        _GT(typeof(CanvasRenderer)),
        // _GT(typeof(UnityEngine.Handheld)),

        // _GT(typeof(Mogo.RPC.Pluto)),
        // _GT(typeof(LanguageConfig)),
        // _GT(typeof(LuaManager)),

        // _GT(typeof(IAPManager)),
		// _GT(typeof(UnityEngine.Purchasing.IAPStoreManager)),
        // _GT(typeof(UnityEngine.Purchasing.IAPConfigurationHelper)),
        // _GT(typeof(UnityEngine.Purchasing.Product)),
		// _GT(typeof(UnityEngine.Purchasing.ProductType)),
        // _GT(typeof(UnityEngine.Purchasing.PurchaseFailureReason)),

		// _GT(typeof(PhotoCall)),
        _GT(typeof(WeChatPay)),
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
