using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.EventSystems;
using TMPro;

public abstract class UI_Base : MonoBehaviourPunCallbacks
{
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();
    public abstract void Init();
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objects[i] = Util.FindChild(gameObject, names[i], true);
            else
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);
            
            if (objects[i] == null)
            {
                Debug.Log($"Failed to bind({names[i]})");
            }
        }
    }

    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if(_objects.TryGetValue(typeof(T), out objects) == false)
            return null;
        return objects[idx] as T;
    }

    protected Text GetText(int idx) { return Get<Text>(idx); }
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }
    protected RawImage GetRawImage(int idx) { return Get<RawImage>(idx); }
    protected GameObject GetGameObject(int idx) { return Get<GameObject>(idx); }
    protected ScrollRect GetScrollRect(int idx) { return Get<ScrollRect>(idx); }
    protected InputField GetInputField(int idx) { return Get<InputField>(idx); }
    //protected ScrollView GetScrollView(int idx) { return Get<ScrollView>(idx); }
    protected TMP_InputField GetTMP_InputField(int idx) { return Get<TMP_InputField>(idx); }

    public static void BindEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);

        switch(type)
        {
            case Define.UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            // case Define.UIEvent.Drag:
            //     evt.OnDragHandler -= action;
            //     evt.OnDragHandler += action;
            //     break;
        }

        // evt.OnDragHandler += ((PointerEventData data) => {evt.gameObject.transform.position = data.position; });
    }
}
