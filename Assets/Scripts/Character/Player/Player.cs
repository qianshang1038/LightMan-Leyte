using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(GameInput))]
public class Player : Singleton<Player>
{
    private CharacterController _characterController;
    public float moveSpeed = 6.0f;
    public float luminance = 100.0f;

    public float luminanceThreshold = 60.0f;
    public float sanValue = 100.0f;

    public bool isInDarkness = false;
    public bool isNervousBreakdown = false;

    public Transform InteractPos;
    //public Transform emotionPos;
    private GameObject buttonEmote;
    private bool isGenerateMark;
    private bool interactActionInput=false;

    // 移动方向
    private Vector3 finalMoveDir;

    // x轴水平移动距离
    public float moveRayDistance = 1.5f;

    // z轴移动的空气墙距离避免穿帮
    public float airWallDistance = 3.2f;

    private void Awake()
    {
        //GameInput.Instance.OnInteractAction +=InteractActionInput;
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleMovement();
        CheckIsInDarkness();

        InputToInteract();
        if(!isGenerateMark)
        {
            Destroy(buttonEmote);
            buttonEmote = null;
        }
    }

    private void LateUpdate()
    {
        isGenerateMark = false;
    }


    private void HandleMovement()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

        // 直接返回，避免不必要的计算
        if (inputVector == Vector2.zero) return;

        // 将二维输入转化为三维方向
        Vector3 inputDir = new Vector3(inputVector.x, 0f, inputVector.y);
        float moveDistance = moveSpeed * Time.deltaTime;

        //GetComponent<Rigidbody>().MovePosition(transform.position + inputDir * moveDistance);
        _characterController.Move(inputDir * moveDistance);
    }


    private bool CheckIsInDarkness()
    {
/*      if ()
        {
            isInDarkness = true;
            return true;
        }
        else
        {
            isInDarkness = false;   
            return false;
        }*/
        return false;
    }

    void InputToInteract()
    {
        Vector3 v = new Vector3(InteractPos.position.x, InteractPos.position.y, InteractPos.position.z+0.5f);
        var hit = Physics.OverlapSphere(v, 3f);
        if (hit.Length > 0)
        {
            /*isGenerateMark = true;
            if (buttonEmote == null)
            {
               buttonEmote = Instantiate(buttonMarkPrefab, emotionPos.position, Quaternion.identity, emotionPos);
            }*/
            if (hit[0].tag=="InteractObj")
            {
                
                hit[0].GetComponent<IInteract>().InteractInfo(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    hit[0].GetComponent<IInteract>().Interact();
                    interactActionInput = false;
                }
            }
        }
    }

    /*private void InteractActionInput(object sender, EventArgs e)
    {
        interactActionInput = true;
    }*/

    public void LoseSanValue()
    {

    }


    private void OnDestroy()
    {
        
    }

    protected override bool ShouldDestroyOnLoad() => true;
}
