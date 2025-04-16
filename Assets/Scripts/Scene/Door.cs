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

        [Header("�ŵĻ�����Ϣ")]
        // ÿ���ŵ�Ψһ��ʶ������DoorManager�з���
        public int doorID = 0;

        // Ĭ��û�н���
        bool isUnlocked = false;

        [field: Range(0, 50f)]
        public float nearDistanceJudgement = 10.0f;
        
        // ���ڿ����Ƿ�������������¼�����������չ����item�ȵ��編���ƣ�
        public bool shouldListenToRoom = false;

        #region ������Ҫ����

        // �ⲿ���Կ��ŵķ����ӿڣ�ÿ�������Ŷ�Ӧ��д
        protected virtual bool TryUnlock()
        {
            Logger.Log("���Կ���");
            return true;
        }

        // �󶨷����¼��ĺ�����Ŀǰ֧���ǰ�һ�����䣬�������Ҫ��չ���Խ����޸ģ�����roomList��
        public void BindRoom(Room room)
        {
            if (shouldListenToRoom)
            {
                // ֻ�е� shouldListenToRoom Ϊ true ʱ�ż����¼�
                // room.OnRoomSolved += OnRoomSolved_Handler;
            }
        }

        // �������������¼��Ļص�
        private void OnRoomSolved_Handler(object sender, EventArgs e)
        {
            Unlock();
            Logger.Log("Door " + doorID + " unlocked due to room solved.");
        }

        void Update()
        {
            // ֻ�е� isGenerateEmote Ϊ false �� _interactObject ����ʱ�������ٶ���
            if (!isGenerateEmote && _interactObject != null)
            {
                Destroy(_interactObject);
                _interactObject = null; // ȷ�� _interactObject ����Ϊ null�������ظ�����
            }
        }

        void LateUpdate()
        {
            isGenerateEmote = false;
        }

        public void Interact()
        {
            // �������Խ����������жϷ�����Unlock()
            Unlock();

            if (isUnlocked)
            {
                OpenDoor();
            }
            else
            {
                Debug.Log("��δ�������޷���");
            }
        }

        public virtual void InteractInfo(bool b)
        {
            isGenerateEmote = true;
            // ֻ���� _interactObject ������ʱ�������¶���
            if (_interactObject == null)
            {
                _interactObject = Instantiate(InteractObject, InteractInfoPos.position, InteractInfoPos.rotation);
                _interactObject.transform.parent = InteractInfoPos.transform;
            }
        }

        // ���������֮��ִ�еķ���
        private void OpenDoor()
        {
            Logger.Log("����");
        }

        private void CloseDoor()
        {
            Logger.Log("������");
        }

        #endregion

        #region �ⲿ�ɵ��õķ���

        // ���Լ�����
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

        // ��������ķ���
        public void Unlock()
        {
             // ���Կ��ţ�TryUnlock�ڲ���д�ɿ��ŵ�����
            if (!TryUnlock())
            {
                Logger.Log("�����������޷�����");
                return;
            }
            if (!isUnlocked) //ͨ���˿��ţ������
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
