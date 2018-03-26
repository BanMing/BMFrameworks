/******************************************************************
** 文件名:	
** 版  权:	(C)  
** 创建人:  Liange
** 日  期:	2015.4.28
** 描  述: 	
            UI面板基类，用来触发lua事件
            
            UI事件注册：1.方案一,一个脚本将控件和一个特殊的名字(ID)绑定在一起， lua里面也通过名字(ID)和事件绑定在一起，这样在C#层触发消息后，也通过该名字进行映射到lua层进行处理
                        2.方案二，通过路径找到控件，然后进行事件绑定
 
**************************** 修改记录 ******************************
** 修改人: 
** 日  期: 
** 描  述: 
*******************************************************************/
using UnityEngine;
using UnityEngine.UI;
using  UnityEngine.Events; 
using System.Collections;
using System.Collections.Generic;

public class UIWindows : MonoBehaviour 
{
    public string m_WindowName = null;

    [System.Serializable]
    public class UIWidgetNameAndPathUnit 
    {
        public string name;
        public Transform tran;
    }

    public List<UIWidgetNameAndPathUnit> m_ListWidgetUnit = new List<UIWidgetNameAndPathUnit>();   //此窗口中含有的与lua进行交互的节点

	void Start () 
    {
        
	}
	
	void OnDestroy()
    {

    }

    void OnEnable()
    {

    }

    //是否已经触发ShowEvent
    [HideInInspector]
    public bool IsTriggerShowEvent = false;

    public void TriggerWindowShowEvent()
    {
        if (IsTriggerShowEvent)
        {
            string str = string.Format("UIWindows.TriggerWindowShowEvent:GameObject {0} m_WindowName {1} 多次触发", gameObject.name, m_WindowName);
            Debug.LogError(str);
            return;
        }

        if (string.IsNullOrEmpty(m_WindowName) == false)
        {
            if (gameObject.name.Contains(m_WindowName) == false)
            {
                string str = string.Format("UIWindows.TriggerWindowShowEvent:！！！警告！！！，注意UI Window名称是否匹配，GameObject Name {0}, Window Name {1}", gameObject.name, m_WindowName);
                Debug.LogWarning(str);
            }

            UIManager.Instance.TriggerWindowShowEvent(m_WindowName, this);
            IsTriggerShowEvent = true;
        }
        else
        {
            string str = string.Format("UIWindows.TriggerWindowShowEvent:GameObject {0} m_WindowName is null", gameObject.name);
            Debug.LogWarning(str);
            IsTriggerShowEvent = false;
        }
    }

    void OnDisable()
    {
        IsTriggerShowEvent = false;

        if (string.IsNullOrEmpty(m_WindowName) == false)
        {
            UIManager.Instance.TriggerWindowHideEvent(m_WindowName, this);
        }
        else
        {
            string str = string.Format("UIWindows.OnDisable:GameObject {0} m_WindowName is null", gameObject.name);
            Debug.LogError(str);
        }
    }

    //-----------------------------------------------------------------------------//

    //注册按钮响应事件
    //path, @开头则到mListWidgetUnit中进行查找;非@开头则是 待注册事件的控件相对于当前窗口的路径
    //eventType,事件类别 "OnClick"
    //eventCallBack, 事件回调

    public void RegisterEventListener(string path, string eventType, LuaInterface.LuaFunction eventCallBack, LuaInterface.LuaTable self)
    {
        Transform tran = GetTransform(path);
        if (tran == null)
        {
            return;
        }

        RegisterEventListener(tran, eventType, eventCallBack, self);
    }
    public void RegisterEventListener(string path, string eventType, LuaInterface.LuaFunction eventCallBack)
    {
        Transform tran = GetTransform(path);
        if (tran == null)
        {
            return;
        }

        RegisterEventListener(tran, eventType, eventCallBack);
    }

    public void RegisterEventListener(Transform tran, string eventType, LuaInterface.LuaFunction eventCallBack)
    {
        RegisterEventListener(tran, eventType, eventCallBack, null);
    }

    public void RegisterEventListener(Transform tran, string eventType, LuaInterface.LuaFunction eventCallBack, LuaInterface.LuaTable self)
    {
        switch (eventType)
        {
            case "OnPointerEnter":
                UIEventListener.Get(tran.gameObject).mOnPointerEnter = delegate (GameObject go, UnityEngine.EventSystems.PointerEventData eventData)
                {
                    if (self != null)
                    {
                        eventCallBack.Call(self, go, eventData);
                    }
                    else
                    {
                        eventCallBack.Call(go, eventData);
                    }
                };
                return;
            case "OnPointerExit":
                UIEventListener.Get(tran.gameObject).mOnPointerExit = delegate (GameObject go, UnityEngine.EventSystems.PointerEventData eventData)
                {
                    if (self != null)
                    {
                        eventCallBack.Call(self, go, eventData);
                    }
                    else
                    {
                        eventCallBack.Call(go, eventData);
                    }
                };
                return;
            case "OnPointerDown":
                UIEventListener.Get(tran.gameObject).mOnPointerDown = delegate (GameObject go, UnityEngine.EventSystems.PointerEventData eventData)
                {
                    if (self != null)
                    {
                        eventCallBack.Call(self, go, eventData);
                    }
                    else
                    {
                        eventCallBack.Call(go, eventData);
                    }
                };
                return;
            case "OnPointerUp":
                UIEventListener.Get(tran.gameObject).mOnPointerUp = delegate (GameObject go, UnityEngine.EventSystems.PointerEventData eventData)
                {
                    if (self != null)
                    {
                        eventCallBack.Call(self, go, eventData);
                    }
                    else
                    {
                        eventCallBack.Call(go, eventData);
                    }
                };
                return;
            case "OnClick":
                UIEventListener.Get(tran.gameObject).mOnPointerClick = delegate(GameObject go, UnityEngine.EventSystems.PointerEventData eventData)
                {
                    if (self != null)
                    {
                        eventCallBack.Call(self, go, eventData);
                    }
                    else
                    {
                        eventCallBack.Call(go, eventData);
                    }
                };
                return;
            case "OnDoubleClick":
                UIEventListener.Get(tran.gameObject).mOnDoublePointerClick = delegate(GameObject go, UnityEngine.EventSystems.PointerEventData eventData)
                {
                    if (self != null)
                    {
                        eventCallBack.Call(self, go, eventData);
                    }
                    else
                    {
                        eventCallBack.Call(go, eventData);
                    }
                };
                return;
            case "OnDrag":
                UIEventListener.Get(tran.gameObject).mOnDrag = delegate (GameObject go, UnityEngine.EventSystems.PointerEventData eventData)
                {
                    if (self != null)
                    {
                        eventCallBack.Call(self, go, eventData);
                    }
                    else
                    {
                        eventCallBack.Call(go, eventData);
                    }
                };
                return;
            case "OnBeginDrag":
                UIEventListener.Get(tran.gameObject).mOnBeginDrag = delegate (GameObject go, UnityEngine.EventSystems.PointerEventData eventData)
                {
                    if (self != null)
                    {
                        eventCallBack.Call(self, go, eventData);
                    }
                    else
                    {
                        eventCallBack.Call(go, eventData);
                    }
                };
                return;
            case "OnEndDrag":
                UIEventListener.Get(tran.gameObject).mOnEndDrag = delegate (GameObject go, UnityEngine.EventSystems.PointerEventData eventData)
                {
                    if (self != null)
                    {
                        eventCallBack.Call(self, go, eventData);
                    }
                    else
                    {
                        eventCallBack.Call(go, eventData);
                    }
                };
                return;
            case "OnDrop":
                UIEventListener.Get(tran.gameObject).mOnDrop = delegate (GameObject go, UnityEngine.EventSystems.PointerEventData eventData)
                {
                    if (self != null)
                    {
                        eventCallBack.Call(self, go, eventData);
                    }
                    else
                    {
                        eventCallBack.Call(go, eventData);
                    }
                };
                return;
            case "OnScroll":
                UIEventListener.Get(tran.gameObject).mOnScroll = delegate (GameObject go, UnityEngine.EventSystems.PointerEventData eventData)
                {
                    if (self != null)
                    {
                        eventCallBack.Call(self, go, eventData);
                    }
                    else
                    {
                        eventCallBack.Call(go, eventData);
                    }
                };
                return;
            case "OnSelect":
                UIEventListener.Get(tran.gameObject).mOnSelect = delegate (GameObject go, UnityEngine.EventSystems.BaseEventData eventData)
                {
                    if (self != null)
                    {
                        eventCallBack.Call(self, go, eventData);
                    }
                    else
                    {
                        eventCallBack.Call(go, eventData);
                    }
                };
                return;
            case "OnDeselect":
                UIEventListener.Get(tran.gameObject).mOnDeselect = delegate (GameObject go, UnityEngine.EventSystems.BaseEventData eventData)
                {
                    if (self != null)
                    {
                        eventCallBack.Call(self, go, eventData);
                    }
                    else
                    {
                        eventCallBack.Call(go, eventData);
                    }
                };
                return;
            case "OnUpdateSelected":
                UIEventListener.Get(tran.gameObject).mOnUpdateSelected = delegate (GameObject go, UnityEngine.EventSystems.BaseEventData eventData)
                {
                    if (self != null)
                    {
                        eventCallBack.Call(self, go, eventData);
                    }
                    else
                    {
                        eventCallBack.Call(go, eventData);
                    }
                };
                return;
            case "OnMove":
                UIEventListener.Get(tran.gameObject).mOnMove = delegate (GameObject go, UnityEngine.EventSystems.AxisEventData eventData)
                {
                    if (self != null)
                    {
                        eventCallBack.Call(self, go, eventData);
                    }
                    else
                    {
                        eventCallBack.Call(go, eventData);
                    }
                };
                return;
            case "OnSubmit":
                UIEventListener.Get(tran.gameObject).mOnSubmit = delegate (GameObject go, UnityEngine.EventSystems.BaseEventData eventData)
                {
                    if (self != null)
                    {
                        eventCallBack.Call(self, go, eventData);
                    }
                    else
                    {
                        eventCallBack.Call(go, eventData);
                    }
                };
                return;
            case "OnCancel":
                UIEventListener.Get(tran.gameObject).mOnCancel = delegate (GameObject go, UnityEngine.EventSystems.BaseEventData eventData)
                {
                    if (self != null)
                    {
                        eventCallBack.Call(self, go, eventData);
                    }
                    else
                    {
                        eventCallBack.Call(go, eventData);
                    }
                };
                return;
            case "OnInitializePotentialDrag":
                UIEventListener.Get(tran.gameObject).mOnInitializePotentialDrag = delegate (GameObject go, UnityEngine.EventSystems.PointerEventData eventData)
                {
                    if (self != null)
                    {
                        eventCallBack.Call(self, go, eventData);
                    }
                    else
                    {
                        eventCallBack.Call(go, eventData);
                    }
                };
                return;

            case "ToggleValueChanged":
                {
                    RegisterToggleValueChanged(tran, eventCallBack);
                }
                return;

            //
            case "OnSliderValueChanged":
                {
                    RegisterSliderValueChanged(tran, eventCallBack);
                }
                return;
        }

        string eventTypeError = string.Format("UIWindows.RegisterEventListener:{0:s}节点，事件类型{1:s}错误,注册事件失败", tran.name, eventType);
        Debug.LogError(eventTypeError);
    }

    public void UnRegisterEventListener(string path, string eventType)
    {
        Transform tran = GetTransform(path);
        if (tran == null)
        {
            return;
        }

        switch (eventType)
        {
            case "OnPointerEnter": UIEventListener.Get(tran.gameObject).mOnPointerEnter = null; return;
            case "OnPointerExit": UIEventListener.Get(tran.gameObject).mOnPointerExit = null; return;
            case "OnPointerDown": UIEventListener.Get(tran.gameObject).mOnPointerDown = null; return;
            case "OnPointerUp": UIEventListener.Get(tran.gameObject).mOnPointerUp = null; return;
            case "OnClick": UIEventListener.Get(tran.gameObject).mOnClick = null; return;
            case "OnDoubleClick": UIEventListener.Get(tran.gameObject).mOnDoublePointerClick = null; return;
            case "OnDrag": UIEventListener.Get(tran.gameObject).mOnDrag = null; return;
            case "OnBeginDrag": UIEventListener.Get(tran.gameObject).mOnBeginDrag = null; return;
            case "OnEndDrag": UIEventListener.Get(tran.gameObject).mOnEndDrag = null; return;
            case "OnDrop": UIEventListener.Get(tran.gameObject).mOnDrop = null; return;
            case "OnScroll": UIEventListener.Get(tran.gameObject).mOnScroll = null; return;
            case "OnSelect": UIEventListener.Get(tran.gameObject).mOnSelect = null; return;
            case "OnDeselect": UIEventListener.Get(tran.gameObject).mOnDeselect = null; return;
            case "OnUpdateSelected": UIEventListener.Get(tran.gameObject).mOnUpdateSelected = null; return;
            case "OnMove": UIEventListener.Get(tran.gameObject).mOnMove = null; return;
            case "OnSubmit": UIEventListener.Get(tran.gameObject).mOnSubmit = null; return;
            case "OnCancel": UIEventListener.Get(tran.gameObject).mOnCancel = null; return;
            case "OnInitializePotentialDrag": UIEventListener.Get(tran.gameObject).mOnInitializePotentialDrag = null; return;
            case "ToggleValueChanged": UnRegisterToggleValueChanged(tran); return;
            case "OnSliderValueChanged": UnRegisterSliderValueChanged(tran); return;

        }
    }

    //-----------------------------------------------------------------------------//

    //注册按钮响应事件

    public void RegisterClickListener(string path, LuaInterface.LuaFunction eventCallBack)
    {
        Transform tran = GetTransform(path);
        if (tran == null)
        {
            return;
        }

        RegisterClickListener(tran, eventCallBack);
    }

    public void RegisterClickListener(Transform tran, LuaInterface.LuaFunction eventCallBack)
    {
        UIClickListener.Get(tran.gameObject).mOnPointerClick = delegate(GameObject go, UnityEngine.EventSystems.PointerEventData eventData)
        {
            eventCallBack.Call(go);
        };
    }

    public void UnRegisterClickListener(string path)
    {
        Transform tran = GetTransform(path);
        if (tran == null)
        {
            return;
        }

        UIClickListener.Get(tran.gameObject).mOnPointerClick = null;
	}

	public void UnRegisterClickListener(Transform tran)
	{
		if (tran == null)
		{
			return;
		}

		UIClickListener.Get(tran.gameObject).mOnPointerClick = null;
	}

    //-----------------------------------------------------------------------------//

    public void RegisterDoubleClickListener(string path, LuaInterface.LuaFunction eventCallBack)
    {
        Transform tran = GetTransform(path);
        if (tran == null)
        {
            return;
        }

        RegisterDoubleClickListener(tran, eventCallBack);
    }

    public void RegisterDoubleClickListener(Transform tran, LuaInterface.LuaFunction eventCallBack)
    {
        UIClickListener.Get(tran.gameObject).mOnDoublePointerClick = delegate(GameObject go, UnityEngine.EventSystems.PointerEventData eventData)
        {
            eventCallBack.Call(go);
        };
    }

    public void UnRegisterDoubleClickListener(string path)
    {
        Transform tran = GetTransform(path);
        if (tran == null)
        {
            return;
        }

        UIClickListener.Get(tran.gameObject).mOnDoublePointerClick = null;
    }

    //-----------------------------------------------------------------------------//

    public void SetLabelText(string path, string text)
    {
        Transform tran = GetTransform(path);
        if (tran == null)
        {
            return;
        }
        
        tran.GetComponent<Text>().text = text;
    }

    public void SetLabelText(Transform tran, string text)
    {
        tran.GetComponent<Text>().text = text;
    }

    public string GetLabelText(string path)
    {
        Transform tran = GetTransform(path);
        if (tran == null)
        {
            return null;
        }

        Text text = tran.GetComponent<Text>();
        return text.text;
    }

    //-----------------------------------------------------------------------------//

    public string GetInputField(string path)
    {
        Transform tran = GetTransform(path);
        if (tran == null)
        {
            return null;
        }

        InputField input = tran.GetComponent<InputField>();
        return input.text;
    }

    //-----------------------------------------------------------------------------//

    public void SetImage(string path, string atlasPath, string imageName)
    {
        Transform tran = GetTransform(path);
        if (tran == null)
        {
            return;
        }

        SetImage(tran, atlasPath, imageName);
    }

    public void SetImage(Transform tran, string atlasPath, string imageName)
    {
        System.Action<Sprite> onLoad = delegate (Sprite sprite)
        {
            tran.GetComponent<Image>().sprite = sprite;
        };

        UIAtlasTool.Instance.GetSprite(atlasPath, imageName, onLoad);
    }

    public void SetImage(Transform parent, string path, string atlasPath, string imageName)
    {
        Transform tran = MyUnityTool.FindChild(parent, path);
        SetImage(tran, atlasPath, imageName);
    }

    public void SetImage(string path, Sprite sprite)
    {
        Transform tran = GetTransform(path);
        if (tran == null)
        {
            return;
        }

        SetImage(tran, sprite);
    }

    public void SetImage(Transform tran, Sprite sprite)
    {
        var image = tran.GetComponent<Image>();
        image.sprite = sprite;
    }

    //-----------------------------------------------------------------------------//

    public void SetImageColor(string path, float r, float g, float b)
    {
        Transform tran = GetTransform(path);
        if (tran == null)
        {
            return;
        }

        Image iamge = tran.GetComponent<Image>();
        Color col = new Color(r, g, b);
        iamge.color = col;
    }

    public void SetImageColor(Transform tran, float r, float g, float b)
    {
        Image image = transform.GetComponent<Image>();
        Color col = new Color(r, g, b);
        image.color = col;
    }

    public void SetImageColor(string path, float r, float g, float b, float a)
    {
        Transform tran = GetTransform(path);
        if (tran == null)
        {
            return;
        }

        Image iamge = tran.GetComponent<Image>();
        Color col = new Color(r, g, b, a);
        iamge.color = col;
    }

    public void SetImageColor(Transform tran, float r, float g, float b, float a)
    {
        Image image = transform.GetComponent<Image>();
        Color col = new Color(r, g, b, a);
        image.color = col;
    }

    //-----------------------------------------------------------------------------//

    public void SetRawImage(string path, Texture tex)
    {
        Transform tran = GetTransform(path);
        if (tran == null)
        {
            return;
        }

        SetRawImage(tran, tex);
    }

    public void SetRawImage(Transform tran, Texture tex)
    {
        RawImage rawImage = tran.GetComponent<RawImage>();
        rawImage.texture = tex;
    }

    //-----------------------------------------------------------------------------//

    public void RegisterToggleValueChanged(string path, LuaInterface.LuaFunction eventCallBack)
    {
        Transform tran = GetTransform(path);
        RegisterToggleValueChanged(tran, eventCallBack);

        
    }

    public void RegisterToggleValueChanged(Transform tran, LuaInterface.LuaFunction eventCallBack)
    {
        if (tran == null)
        {
            return;
        }

        UnityAction<bool> callback = delegate (bool result)
        {
            eventCallBack.Call(result);
        };

        Toggle toggle = tran.GetComponent<Toggle>();
        toggle.onValueChanged.RemoveAllListeners();
        toggle.onValueChanged.AddListener(callback);
    }

    public void UnRegisterToggleValueChanged(string path)
    {
        Transform tran = GetTransform(path);
        UnRegisterToggleValueChanged(tran);
    }

    public void UnRegisterToggleValueChanged(Transform tran)
    {
        if (tran == null)
        {
            return;
        }

        Toggle toggle = tran.GetComponent<Toggle>();
        toggle.onValueChanged.RemoveAllListeners();
    }

    //-----------------------------------------------------------------------------//SliderValueChanged

    public void RegisterSliderValueChanged(string path,LuaInterface.LuaFunction eventCallBack)
    {
        Transform tran = GetTransform(path);
        RegisterSliderValueChanged(tran, eventCallBack);
    }

    public void RegisterSliderValueChanged(Transform tran, LuaInterface.LuaFunction eventCallBack)
    {
        if(tran == null)
        {
            return;
        }
        UnityAction<float> callback = delegate (float result)
       {
           eventCallBack.Call(result);
       };
        Slider slider = tran.GetComponent<Slider>();
        slider.onValueChanged.RemoveAllListeners();
        slider.onValueChanged.AddListener(callback);
    }

    public void UnRegisterSliderValueChanged(string path)
    {
        Transform tran = GetTransform(path);
        UnRegisterSliderValueChanged(tran);
    }

    public void UnRegisterSliderValueChanged(Transform tran)
    {
        if(tran == null)
        {
            return;
        }
        Slider slider = tran.GetComponent<Slider>();
        slider.onValueChanged.RemoveAllListeners();
    }

    //-----------------------------------------------------------------------------//

    public void PlayTweenAnimation(string path, bool isForward)
    {
        PlayTweenAnimation(path, isForward, false);
    }

    public void PlayTweenAnimation(string path, bool isForward, bool isResetBeforeStartPlay)
    {
        Transform tran = GetTransform(path);
        if (tran == null)
        {
            return;
        }

        UITweener tween = tran.GetComponent<UITweener>();
        if (isResetBeforeStartPlay)
        {
            tween.ResetToBeginning();
        }

        if (isForward)
        {
            tween.PlayForward();
        }
        else
        {
            tween.PlayReverse();
        }
    }

    //-----------------------------------------------------------------------------//

    public void ShowChildNode(string name)
    {
        Transform tran = GetTransform(name);
        if (tran != null)
        {
            tran.gameObject.SetActive(true);
        }
    }

    public void HideChildNode(string name)
    {
        Transform tran = GetTransform(name);
        if (tran != null)
        {
            tran.gameObject.SetActive(false);
        }
    }

    public void ShowNode(string name)
    {
        GameObject go = MyUnityTool.Find(name);
        if (go != null)
        {
            go.SetActive(true);
        }
    }

    public void HideNode(string name)
    {
        GameObject go = MyUnityTool.Find(name);
        if (go != null)
        {
            go.SetActive(false);
        }
    }

    public void SetActive(GameObject go, bool active)
    {
        go.SetActive(active);
    }

    //-----------------------------------------------------------------------------//

    public void OpenWindow(string windowName)
    {
        UIManager.Instance.OpenWindow(windowName);
    }

    public void OpenWindowAndCloseSelf(string windowName)
    {
        UIManager.Instance.OpenWindow(windowName);
        UIManager.Instance.CloseWindow(gameObject);
    }

    public void CloseWindow(string windowName)
    {
        UIManager.Instance.CloseWindow(windowName);
    }

    public void CloseWindow(GameObject go)
    {
        UIManager.Instance.CloseWindow(go);
    }

    public void CloseWindow()
    {
        UIManager.Instance.CloseWindow(gameObject);
    }

    public void HideWindow(string windowName)
    {
        UIManager.Instance.HideWindow(windowName);
    }

    public void HideWindow(GameObject go)
    {
        UIManager.Instance.HideWindow(go);
    }

    public void HideWindow()
    {
        UIManager.Instance.HideWindow(gameObject);
    }

    //-----------------------------------------------------------------------------//

    public Transform CloneNode(string name)
    {
        Transform tran = GetTransform(name);
        if (tran == null)
        {
            return null;
        }

        Transform newTran = Instantiate(tran);
        newTran.parent = tran.parent;
        newTran.localRotation = Quaternion.identity;
        newTran.localPosition = Vector3.zero;
        newTran.localScale = Vector3.one;
        
        newTran.gameObject.SetActive(true);

        return newTran;
    }

    public RectTransform CloneUINode(string name, string parentName)
    {
        RectTransform tran = GetRectTransform(name);
        RectTransform parentTran = GetRectTransform(parentName);

        RectTransform newTran = Instantiate(tran);
        newTran.SetParent(parentTran);
        newTran.localRotation = Quaternion.identity;
        newTran.localPosition = Vector3.zero;
        newTran.localScale = Vector3.one;

        newTran.gameObject.SetActive(true);

        return newTran;
    }

    //-----------------------------------------------------------------------------//

    public void DestroyChildrenNode(string name)
    {
        Transform tran = GetTransform(name);
        while(tran.childCount > 0)
        {
            Transform child = tran.GetChild(0);
            child.SetParent(null);
            Destroy(child.gameObject);
        }
    }

    //-----------------------------------------------------------------------------//

    public Transform GetTransform(string name)
    {
        Transform tran = null;
        if (name.StartsWith("@"))
        {
            for (int i = 0; i < m_ListWidgetUnit.Count; ++i)
            {
                if (name == m_ListWidgetUnit[i].name)
                {
                    tran = m_ListWidgetUnit[i].tran;
                    break;
                }
            }
        }

        if (tran == null)
        {
            tran = MyUnityTool.FindChild(transform, name);

            if (tran == null)
            {
                string str = string.Format("UIWindows.GetNode:面板{0:s}中待绑定的节点{1:s}名称或路径错误", m_WindowName, name);
                Debug.LogError(str);
            }
        }

        return tran;
    }

    public RectTransform GetRectTransform(string name)
    {
        Transform tran = GetTransform(name);
        if (tran == null)
        {
            return null;
        }
        return tran.GetComponent<RectTransform>();
    }

    //-----------------------------------------------------------------------------//

    public UnityEngine.UI.Button GetUIButton(string childName)
    {
        return GetUIButton(transform, childName);
    }

    public UnityEngine.UI.Button GetUIButton(Transform parent, string childName)
    {
        return GetComponent<UnityEngine.UI.Button>(parent, childName);
    }

    public UnityEngine.UI.Text GetUIText(string childName)
    {
        return GetUIText(transform, childName);
    }

    public UnityEngine.UI.Text GetUIText(Transform parent, string childName)
    {
        return GetComponent<UnityEngine.UI.Text>(parent, childName);
    }

    public UnityEngine.UI.Image GetUIImage(string childName)
    {
        return GetUIImage(transform, childName);
    }

    public UnityEngine.UI.Image GetUIImage(Transform parent, string childName)
    {
        return GetComponent<UnityEngine.UI.Image>(parent, childName);
    }

    public UnityEngine.UI.RawImage GetUIRawImage(string childName)
    {
        return GetUIRawImage(transform, childName);
    }

    public UnityEngine.UI.RawImage GetUIRawImage(Transform parent, string childName)
    {
        return GetComponent<UnityEngine.UI.RawImage>(parent, childName);
    }

    public UnityEngine.UI.Toggle GetUIToggle(string childName)
    {
        return GetUIToggle(transform, childName);
    }

    public UnityEngine.UI.Toggle GetUIToggle(Transform parent, string childName)
    {
        return GetComponent<UnityEngine.UI.Toggle>(parent, childName);
    }

    public UnityEngine.UI.Slider GetUISlider(string childName)
    {
        return GetUISlider(transform, childName);
    }

    public UnityEngine.UI.Slider GetUISlider(Transform parent, string childName)
    {
        return GetComponent<UnityEngine.UI.Slider>(parent, childName);
    }

    public UnityEngine.UI.ScrollRect GetUIScrollRect(string childName)
    {
        return GetUIScrollRect(transform, childName);
    }

    public UnityEngine.UI.ScrollRect GetUIScrollRect(Transform parent, string childName)
    {
        return GetComponent<UnityEngine.UI.ScrollRect>(parent, childName);
    }

    public UnityEngine.UI.Scrollbar GetUIScrollbar(string childName)
    {
        return GetUIScrollbar(transform, childName);
    }

    public UnityEngine.UI.Scrollbar GetUIScrollbar(Transform parent, string childName)
    {
        return GetComponent<UnityEngine.UI.Scrollbar>(parent, childName);
    }

    public UnityEngine.UI.InputField GetUIInputField(string childName)
    {
        return GetUIInputField(transform, childName);
    }

    public UnityEngine.UI.InputField GetUIInputField(Transform parent, string childName)
    {
        return GetComponent<UnityEngine.UI.InputField>(parent, childName);
    }

    public UnityEngine.UI.Dropdown GetUIDropdown(string childName)
    {
        return GetUIDropdown(transform, childName);
    }

    public UnityEngine.UI.Dropdown GetUIDropdown(Transform parent, string childName)
    {
        return GetComponent<UnityEngine.UI.Dropdown>(parent, childName);
    }

    public UnityEngine.Canvas GetUICanvas(string childName)
    {
        return GetUICanvas(transform, childName);
    }

    public UnityEngine.Canvas GetUICanvas(Transform parent, string childName)
    {
        return GetComponent<UnityEngine.Canvas>(parent, childName);
    }

    public UnityEngine.UI.CanvasScaler GetUICanvasScaler(string childName)
    {
        return GetUICanvasScaler(transform, childName);
    }

    public UnityEngine.UI.CanvasScaler GetUICanvasScaler(Transform parent, string childName)
    {
        return GetComponent<UnityEngine.UI.CanvasScaler>(parent, childName);
    }

    public T GetComponent<T>(Transform parent, string childName)
    {
        Transform tran = MyUnityTool.FindChild(parent, childName);
        return tran.gameObject.GetComponent<T>();
    }

    public UnityEngine.Component GetComponentByString(string childName, string type)
    {
        Transform tran = GetTransform(childName);
        return GetComponentByString(tran, type);
    }

    public UnityEngine.Component GetComponentByString(Transform tran, string type)
    {
        return tran.gameObject.GetComponent(type);
    }

    //-----------------------------------------------------------------------------//

    [ContextMenu("Generate UI Node Bind List")]
    public void GenerateBindList()
    {

#if UNITY_EDITOR
        m_WindowName = gameObject.name;
#else
        if(string.IsNullOrEmpty(m_WindowName))
        {
            m_WindowName = gameObject.name;
        }
#endif

        m_ListWidgetUnit.Clear();
        Transform[] tranArray = transform.GetComponentsInChildren<Transform>(true);

        for (int i = 0; i < tranArray.Length; ++i)
        {
            Transform tran = tranArray[i];
            if (tran.name.StartsWith("@") == false)
            {
                continue;
            }

            UIWidgetNameAndPathUnit unit = new UIWidgetNameAndPathUnit();
            unit.name = tran.name;
            unit.tran = tran;

            m_ListWidgetUnit.Add(unit);
        }

        Debug.Log("Generate UI Node Bind List Over");
    }
}
