using System;
using UnityEngine;

public class CountdownTimer
{
    // ����ʱ����ʱ��
    private float duration;
    // ʣ��ʱ��
    private float timeRemaining;
    // ����״̬
    public bool isRunning; 

    // ��ʱ��ʼʱ/ֹͣʱ�򴥷�
    public event Action OnTimerStart;
    public event Action OnTimerStop;

    // ���ԣ���ǰ��ʱ״̬�ͼ�ʱ���Ľ���
    public bool IsRunning => isRunning;
    public float Progress => 1 - (timeRemaining / duration);

    /// <summary>
    /// ��ʱ�����캯�������뵹��ʱ��ʱ�䣬�Ϳ��Զ��ļ�ʱ���Ŀ�ʼ�ͽ����¼�
    /// </summary>
    /// <param name="duration"></param>
    public CountdownTimer(float duration)
    {
        this.duration = duration;
        timeRemaining = duration;
        isRunning = false;
    }

    // ������ʱ��
    public void Start()
    {
        if (!isRunning)
        {
            isRunning = true;
            timeRemaining = duration;
            OnTimerStart?.Invoke(); // ������ʼ�¼�
        }
    }

    // ֹͣ��ʱ��
    public void Stop()
    {
        if (isRunning)
        {
            isRunning = false;
            OnTimerStop?.Invoke(); // ����ֹͣ�¼�
        }
    }

    // ÿ��ʱ������趼����һ�μ�ʱ��
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
