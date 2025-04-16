using System;
using UnityEngine;

public class CountdownTimer
{
    // 倒计时的总时间
    private float duration;
    // 剩余时间
    private float timeRemaining;
    // 运行状态
    public bool isRunning; 

    // 计时开始时/停止时候触发
    public event Action OnTimerStart;
    public event Action OnTimerStop;

    // 属性：当前计时状态和计时器的进度
    public bool IsRunning => isRunning;
    public float Progress => 1 - (timeRemaining / duration);

    /// <summary>
    /// 计时器构造函数，传入倒计时总时间，就可以订阅计时器的开始和结束事件
    /// </summary>
    /// <param name="duration"></param>
    public CountdownTimer(float duration)
    {
        this.duration = duration;
        timeRemaining = duration;
        isRunning = false;
    }

    // 启动计时器
    public void Start()
    {
        if (!isRunning)
        {
            isRunning = true;
            timeRemaining = duration;
            OnTimerStart?.Invoke(); // 触发开始事件
        }
    }

    // 停止计时器
    public void Stop()
    {
        if (isRunning)
        {
            isRunning = false;
            OnTimerStop?.Invoke(); // 触发停止事件
        }
    }

    // 每个时间戳步骤都更新一次计时器
    public void Tick(float deltaTime)
    {
        if (isRunning)
        {
            timeRemaining -= deltaTime;

            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                Stop();
            }
        }
    }
}
