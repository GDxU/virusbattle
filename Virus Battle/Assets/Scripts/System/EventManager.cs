using System;
using System.Collections.Generic;

/// <summary>
/// 事件管理器
/// </summary>
public static class EventManager
{
    /// <summary>
    /// 事件队列
    /// </summary>
	private static Dictionary<Enum, List<EventCallback>> _eventQueue;

    /// <summary>
    /// 构造函数
    /// </summary>
    static EventManager()
    {
        _eventQueue = new Dictionary<Enum, List<EventCallback>>();
    }

    /// <summary>
    /// 添加监听器
    /// </summary>
    /// <param name="type">事件类型</param>
    /// <param name="callback">回调函数</param>
    public static void AddListener(Enum type, EventCallback callback)
	{
		if (!_eventQueue.ContainsKey(type))
		{
			_eventQueue.Add(type, new List<EventCallback>());
		}
		if (!_eventQueue[type].Contains(callback))
		{
			_eventQueue[type].Add(callback);
		}
	}

    /// <summary>
    /// 移除监听器
    /// </summary>
    /// <param name="type">事件类型</param>
    /// <param name="callback">回调函数</param>
    public static void RemoveListener(Enum type, EventCallback callback)
	{
		if (_eventQueue.ContainsKey(type))
		{
			_eventQueue[type].Remove(callback);
		}
	}

    /// <summary>
    /// 发送事件
    /// </summary>
    /// <param name="type">事件类型</param>
    /// <param name="parameter">事件参数</param>
    public static void PostEvent(Enum type, params object[] parameters)
	{
		if (_eventQueue != null && _eventQueue.ContainsKey(type))
		{
            #region 执行所有监听器回调函数
            List<EventCallback> callbacks = _eventQueue[type];
			for (int i = 0; i < callbacks.Count; i++)
			{
				callbacks[i](parameters);
			}
            #endregion
        }
    }

    /// <summary>
    /// 事件回调
    /// </summary>
    /// <param name="parameter">事件参数</param>
    public delegate void EventCallback(params object[] parameters);
}
