using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.ExamplePanel;
using Unity.Services.Analytics.Internal;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMovement : MonoBehaviour
{
    private bool canMove=true;
    public string MonsterName;

    [Header("Move Info")]
    public float moveSpeed;
    public float traceSpeed;
    private Vector3 patrolCenter;//巡逻中心点 
    private Transform playerTransfrom;
    private Vector3 patrolDir;

    [Header("Navi Info")]
    public float checkDistance;
    public float attackDistance;
    public float distanceThreshold;
    public float patrolRadius;
    public float stayDuration;
    public Transform[] wayPoints;
    private int index=0;
    private float stayTimer;
    private float status=0;
    private float distanceToPoint;


    #region Component
    private Rigidbody rb;
    private Animator anim;
    
    #endregion


    public void SetMoveable(bool b)
    {
        canMove = b;
    }

    //2.5d的怪物
    void Start()
    {
        rb= GetComponent<Rigidbody>();
        patrolCenter = transform.position;
      
       playerTransfrom = Player.Instance.transform;
        stayTimer = stayDuration;
    }

    
    void Update()
    {
        Vector3 currentPosXZ = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 targetPosXZ = new Vector3(Player.Instance.transform.position.x, 0, Player.Instance.transform.position.z);
        float dir= Vector3.Distance(currentPosXZ, targetPosXZ);

        if (canMove)
        {
            if (dir < checkDistance)
            {
                Trace();
                if (dir < attackDistance)
                {

                    Attack();
                }
            }
            else
            {

                OnPatrolling();
            }
        }
    }
    private void Trace()
    {
        Vector3 direction = ( playerTransfrom.position- transform.position).normalized;
        Vector3 _v = direction * traceSpeed * Time.deltaTime;
        Vector3 nextPosition = transform.position + new Vector3(_v.x, 0, _v.z);

        RaycastHit hit;
        if (Physics.Raycast(nextPosition + Vector3.up * 1f, Vector3.down, out hit, 5f))
        {
            nextPosition.y = hit.point.y;
        }
        transform.position = nextPosition;
        status += Time.deltaTime;
    }

    private void Attack()
    {
        //进入2d场景()
        GamePanel.Instance.HideMe();
        //传入monsterName和monster数量
        FightBtn.Instance.FightStart(MonsterName,3);
    }
    private void OnPatrolling()
    {

        //追击转回巡逻
        if (status > 0f)
        {
            ResetPath();
            status = 0f;

        }

        //定点巡逻idle计时
        if ( stayTimer > 0f)
        {
            stayTimer-= Time.deltaTime; 
            //idle状态的动作
            return;
        }


        //distanceToPoint= Vector3.Distance(wayPoints[index].position, transform.position);
        Vector3 currentPosXZ = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 targetPosXZ = new Vector3(wayPoints[index].position.x, 0, wayPoints[index].position.z);
        distanceToPoint = Vector3.Distance(currentPosXZ, targetPosXZ);

        if (distanceToPoint < distanceThreshold)
        { 
            index =(index+1)% wayPoints.Length;
            patrolDir = (wayPoints[index].position - transform.position).normalized;
            stayTimer = stayDuration;
            //Debug.Log("I am on point" + wayPoints[index].name);
        }
        //怪物移动
        Vector3 _v = patrolDir * moveSpeed * Time.deltaTime;
        Vector3 nextPosition = transform.position + new Vector3(_v.x, 0, _v.z);

        RaycastHit hit;
        if (Physics.Raycast(nextPosition + Vector3.up * 1f, Vector3.down, out hit, 5f))
        {
            nextPosition.y = hit.point.y;
        }
        transform.position = nextPosition;

    }
   private void ResetPath()
    {
        index = 0;
        patrolDir = (wayPoints[index].position - transform.position).normalized;
        distanceToPoint = Vector3.Distance(wayPoints[index].position, transform.position);
    }
   
}
