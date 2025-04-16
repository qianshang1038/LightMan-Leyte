using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace scene
{
    public class Door : MonoBehaviour, IInteract
    {
        public GameObject InteractObject;
        public Transform InteractInfoPos;
        private GameObject _interactObject;
        private bool isGenerateEmote = false;

        [Header("门的基本信息")]
        // 每扇门的唯一标识符会在DoorManager中分配
        public int doorID = 0;

        // 默认没有解锁
        bool isUnlocked = false;

        [field: Range(0, 50f)]
        public float nearDistanceJudgement = 10.0f;
        
        // 用于控制是否监听其他房间事件（后续可拓展加入item等等如法炮制）
        public bool shouldListenToRoom = false;

        #region 自身主要方法

        // 外部尝试开门的方法接口，每个种类门对应覆写
        protected virtual bool TryUnlock()
        {
            Logger.Log("尝试开门");
            return true;
        }

        // 绑定房间事件的函数，目前支持是绑定一个房间，如果后续要拓展可以进行修改（加入roomList）
        public void BindRoom(Room room)
        {
            if (shouldListenToRoom)
            {
                // 只有当 shouldListenToRoom 为 true 时才监听事件
                // room.OnRoomSolved += OnRoomSolved_Handler;
            }
        }

        // 处理房间谜题解决事件的回调
        private void OnRoomSolved_Handler(object sender, EventArgs e)
        {
            Unlock();
            Logger.Log("Door " + doorID + " unlocked due to room solved.");
        }

        void Update()
        {
            // 只有当 isGenerateEmote 为 false 且 _interactObject 存在时，才销毁对象
            if (!isGenerateEmote && _interactObject != null)
            {
                Destroy(_interactObject);
                _interactObject = null; // 确保 _interactObject 设置为 null，避免重复销毁
            }
        }

        void LateUpdate()
        {
            isGenerateEmote = false;
        }

        public void Interact()
        {
            // 交互则尝试解锁，解锁判断方法在Unlock()
            Unlock();

            if (isUnlocked)
            {
                OpenDoor();
            }
            else
            {
                Debug.Log("门未解锁，无法打开");
            }
        }

        public virtual void InteractInfo(bool b)
        {
            isGenerateEmote = true;
            // 只有在 _interactObject 不存在时才生成新对象
            if (_interactObject == null)
            {
                _interactObject = Instantiate(InteractObject, InteractInfoPos.position, InteractInfoPos.rotation);
                _interactObject.transform.parent = InteractInfoPos.transform;
            }
        }

        // 具体打开自身之后执行的方法
        private void OpenDoor()
        {
            Logger.Log("打开门");
        }

        private void CloseDoor()
        {
            Logger.Log("关上门");
        }

        #endregion

        #region 外部可调用的方法

        // 给自己上锁
        public void Lock()
        {
            if (isUnlocked)
            {
                Logger.Log("Door Unlocked");
                isUnlocked = false;
                CloseDoor();
            }
            else
            {
                Logger.Log("Door has Locked, no need to lock again.");
            }
        }

        // 解锁自身的方法
        public void Unlock()
        {
             // 尝试开门，TryUnlock内部覆写可开门的条件
            if (!TryUnlock())
            {
                Logger.Log("条件不满足无法开门");
                return;
            }
            if (!isUnlocked) //通过了开门，则解锁
            {
                Logger.Log("Door Unlocked");
                isUnlocked = true;
                OpenDoor();
            }
            else
            {
                Logger.Log("Door has unlocked, no need to unlock again.");
            }
        }

        public bool IsUnlocked()
        {
            return isUnlocked;
        }

        public bool IsPlayerNearTheDoor()
        {
            float distance = Vector3.Distance(Player.Instance.transform.position, gameObject.transform.position);
            return distance < nearDistanceJudgement;
        }
        #endregion
    }
}
