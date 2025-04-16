using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Dice))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public class DiceRoller : MonoBehaviour
{
    [Tooltip("骰子Roll设置")]
    // 初始力
    [SerializeField] float rollForce = 50f;
    // 旋转速度
    [SerializeField] float torqueAmount = 5f;
    // 总投掷时间
    [SerializeField] float maxRollTime = 3f;
    // 最小角速度
    [SerializeField] float minAngularVelocity = 0.1f;
    // 将骰子归位--用时
    [SerializeField] float smoothTime = 0.1f;
    [SerializeField] float maxSpeed = 15f;

    [Tooltip("投掷结果")]
    [SerializeField] TMPro.TextMeshProUGUI resultText;

    [Tooltip("投掷效果设置")]
    // 音效设置：翻滚声，撞击声，结束提示音；两个特效
    // [SerializeField] AudioClip shakeClip;
    [SerializeField] AudioClip rollClip;
    [SerializeField] AudioClip impactClip;
    [SerializeField] AudioClip finalResultClip;
    [SerializeField] GameObject impactEffect;
    [SerializeField] GameObject finalResultEffect;

    [Tooltip("粒子相关设置")]
    [SerializeField] float particleSize = 1.0f;
    [SerializeField] float particleDestroySpeed = 1.0f;

    Dice diceSides;
    AudioSource audioSource;
    Rigidbody rb;

    CountdownTimer rollTimer;

    // 骰子的原始位置 以及当前速度
    Vector3 originPosition;
    Vector3 currentVelocity;
    bool finalize;

    public event Action DiceFinishCalled;

    private void Awake()
    {
        // 获取所有骰子面的信息，从可脚本化物体DiceSO上面读入相关信息
        diceSides = GetComponent<Dice>();

        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();

        // resultText.text = "Click to roll";
        originPosition = transform.position;

        // 给计时器一个最大时间
        rollTimer = new CountdownTimer(maxRollTime);

        // 启用计时器，代表投掷时间，控制了骰子相关的事情
        rollTimer.OnTimerStart += StartTheRoll;
        rollTimer.OnTimerStop += () => finalize = true;
    }

    // 鼠标点击骰子开始运转
    private void OnMouseUp()
    {
        if (rollTimer.IsRunning) return;
        rollTimer.Start();
        Logger.Log("Start!");
    }

/*    public void OnPointerClick(PointerEventData eventData)
    {
        if (rollTimer.IsRunning) return;
        rollTimer.Start();
        Logger.Log("Start!");
    }*/

    // 每帧更新倒计时
    public void AutoRoll()
    {
        rollTimer.Start();
    }
    private void Update()
    {
        if (rollTimer.IsRunning)
        {
            rollTimer.Tick(Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        if (finalize)
        {
            MoveDiceToCenter();
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (rollTimer.IsRunning && rollTimer.Progress < 0.5f && rb.angularVelocity.magnitude < minAngularVelocity)
        {
            finalize = true;
        }

        // 碰撞事件声音
        if (impactClip)
        {
            audioSource.PlayOneShot(impactClip);
        }

        // 生成粒子特效（如果有的话!）
        if (impactEffect)
        {
            var particles = InstantiateFX(impactEffect, col.contacts[0].point, particleSize);
            Destroy(particles, particleDestroySpeed);
        }
    }

    private void StartTheRoll()
    {
        rb.isKinematic = false; // 恢复物理效果
        ResetDiceState();
        // resultText.text = "";

        // 随机向一个方向施加力和旋转
        Vector3 targetPosition = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, UnityEngine.Random.Range(-1f, 1f));
        rb.AddForce(targetPosition * rollForce, ForceMode.Impulse);
        rb.AddTorque(UnityEngine.Random.insideUnitSphere * torqueAmount, ForceMode.Impulse);

        // audioSource.clip = shakeClip;
        // audioSource.loop = true;
        // audioSource.Play();
    }

    // 如果当前Roller的位置
    private void MoveDiceToCenter()
    {
        rb.isKinematic = true; // 禁用物理效果
        transform.position = Vector3.SmoothDamp(transform.position, originPosition, ref currentVelocity, smoothTime, maxSpeed);

        // 当骰子位置和原点接近时候就可以重置骰子状态了
        if (Vector3.Distance(originPosition, transform.position) < 0.1f)
        {
            FinalizeRoll();
        }
    }

    private void FinalizeRoll()
    {
        rollTimer.Stop();
        finalize = false;

        audioSource.loop = false;
        audioSource.Stop();
        if (finalResultClip)
        {
            audioSource.PlayOneShot(finalResultClip);
        }
        if (finalResultEffect)
        {
            var particles = InstantiateFX(finalResultEffect, transform.position, particleSize);
            Destroy(particles, 3f);
        }

        // 使用DiceSides获取朝上的面值
        int result = diceSides.GetTopFaceValue();

        Logger.Log($"Dice landed on {result}");
        // resultText.text = result.ToString();
         TurnController.Instance.DiceNum=result;
        DiceFinishCalled?.Invoke();

    }

    // 重置状态准备下一次投掷，所有东西清零归位
    private void ResetDiceState()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = originPosition;
    }

    /// <summary>
    /// 生成粒子特效
    /// </summary>
    /// <param name="effects"></param>
    /// <param name="position"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    GameObject InstantiateFX(GameObject effects, Vector3 position, float size)
    {
        // 生成无旋转的粒子特效
        var particles = Instantiate(effects, position, Quaternion.identity);
        particles.transform.localScale = Vector3.one * size;
        return particles;
    }
}
