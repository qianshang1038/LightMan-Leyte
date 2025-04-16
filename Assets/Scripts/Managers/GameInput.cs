using System;
using UnityEngine;

public class GameInput : Singleton<GameInput>
{

    // ������������¼���Ӧ�����罻����ͣ��ʹ���¼�����
    public event EventHandler OnInteractAction;
    public event EventHandler OnPauseAction;

    // ���� InputSystem -- PlayerInputActions
    private PlayerInputActions playerInputActions;
    private bool CanInput=true;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        // �������¼�
        playerInputActions.Player.Pause.performed += Pause_performed;
        playerInputActions.Player.Interact.performed += Interact_performed;
    }

    private void OnDestroy()
    {
        // �Ƴ��¼��󶨣���ֹ�ڴ�й¶
        if (playerInputActions != null)
        {
            playerInputActions.Player.Pause.performed -= Pause_performed;
            playerInputActions.Dispose();
        }
    }

    // ��ȡ�ƶ������������Ѿ���InputAction�н����˹�һ��
    public Vector2 GetMovementVectorNormalized()
    {
        if(CanInput)
        {
            Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
            return inputVector;
        }
        return new Vector2(0, 0);
    }

    public void SetInput(bool b)
    {
        CanInput = b;
    }

    // ���н��������������Ļ���
    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    // ���°���������ͣ
    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

}
