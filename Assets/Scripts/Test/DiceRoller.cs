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
    [Tooltip("����Roll����")]
    // ��ʼ��
    [SerializeField] float rollForce = 50f;
    // ��ת�ٶ�
    [SerializeField] float torqueAmount = 5f;
    // ��Ͷ��ʱ��
    [SerializeField] float maxRollTime = 3f;
    // ��С���ٶ�
    [SerializeField] float minAngularVelocity = 0.1f;
    // �����ӹ�λ--��ʱ
    [SerializeField] float smoothTime = 0.1f;
    [SerializeField] float maxSpeed = 15f;

    [Tooltip("Ͷ�����")]
    [SerializeField] TMPro.TextMeshProUGUI resultText;

    [Tooltip("Ͷ��Ч������")]
    // ��Ч���ã���������ײ������������ʾ����������Ч
    // [SerializeField] AudioClip shakeClip;
    [SerializeField] AudioClip rollClip;
    [SerializeField] AudioClip impactClip;
    [SerializeField] AudioClip finalResultClip;
    [SerializeField] GameObject impactEffect;
    [SerializeField] GameObject finalResultEffect;

    [Tooltip("�����������")]
    [SerializeField] float particleSize = 1.0f;
    [SerializeField] float particleDestroySpeed = 1.0f;

    Dice diceSides;
    AudioSource audioSource;
    Rigidbody rb;

    CountdownTimer rollTimer;

    // ���ӵ�ԭʼλ�� �Լ���ǰ�ٶ�
    Vector3 originPosition;
    Vector3 currentVelocity;
    bool finalize;

    public event Action DiceFinishCalled;

    private void Awake()
    {
        // ��ȡ�������������Ϣ���ӿɽű�������DiceSO������������Ϣ
        diceSides = GetComponent<Dice>();

        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();

        // resultText.text = "Click to roll";
        originPosition = transform.position;

        // ����ʱ��һ�����ʱ��
        rollTimer = new CountdownTimer(maxRollTime);

        // ���ü�ʱ��������Ͷ��ʱ�䣬������������ص�����
        rollTimer.OnTimerStart += StartTheRoll;
        rollTimer.OnTimerStop += () => finalize = true;
    }

    // ��������ӿ�ʼ��ת
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

    // ÿ֡���µ���ʱ
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

        // ��ײ�¼�����
        if (impactClip)
        {
            audioSource.PlayOneShot(impactClip);
        }

        // ����������Ч������еĻ�!��
        if (impactEffect)
        {
            var particles = InstantiateFX(impactEffect, col.contacts[0].point, particleSize);
            Destroy(particles, particleDestroySpeed);
        }
    }

    private void StartTheRoll()
    {
        rb.isKinematic = false; // �ָ�����Ч��
        ResetDiceState();
        // resultText.text = "";

        // �����һ������ʩ��������ת
        Vector3 targetPosition = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, UnityEngine.Random.Range(-1f, 1f));
        rb.AddForce(targetPosition * rollForce, ForceMode.Impulse);
        rb.AddTorque(UnityEngine.Random.insideUnitSphere * torqueAmount, ForceMode.Impulse);

        // audioSource.clip = shakeClip;
        // audioSource.loop = true;
        // audioSource.Play();
    }

    // �����ǰRoller��λ��
    private void MoveDiceToCenter()
    {
        rb.isKinematic = true; // ��������Ч��
        transform.position = Vector3.SmoothDamp(transform.position, originPosition, ref currentVelocity, smoothTime, maxSpeed);

        // ������λ�ú�ԭ��ӽ�ʱ��Ϳ�����������״̬��
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

        // ʹ��DiceSides��ȡ���ϵ���ֵ
        int result = diceSides.GetTopFaceValue();

        Logger.Log($"Dice landed on {result}");
        // resultText.text = result.ToString();
         TurnController.Instance.DiceNum=result;
        DiceFinishCalled?.Invoke();

    }

    // ����״̬׼����һ��Ͷ�������ж��������λ
    private void ResetDiceState()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = originPosition;
    }

    /// <summary>
    /// ����������Ч
    /// </summary>
    /// <param name="effects"></param>
    /// <param name="position"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    GameObject InstantiateFX(GameObject effects, Vector3 position, float size)
    {
        // ��������ת��������Ч
        var particles = Instantiate(effects, position, Quaternion.identity);
        particles.transform.localScale = Vector3.one * size;
        return particles;
    }
}
