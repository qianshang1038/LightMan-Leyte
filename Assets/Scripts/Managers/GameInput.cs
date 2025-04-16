using System;
using UnityEngine;

public class GameInput : Singleton<GameInput>
{

    // 这个部分用于事件响应，例如交互暂停等使用事件解耦
    public event EventHandler OnInteractAction;
    public event EventHandler OnPauseAction;

    // 引用 InputSystem -- PlayerInputActions
    private PlayerInputActions playerInputActions;
    private bool CanInput=true;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        // 绑定输入事件
        playerInputActions.Player.Pause.performed += Pause_performed;
        playerInputActions.Player.Interact.performed += Interact_performed;
    }

    private void OnDestroy()
    {
        // 移除事件绑定，防止内存泄露
        if (playerInputActions != null)
        {
            playerInputActions.Player.Pause.performed -= Pause_performed;
            playerInputActions.Dispose();
        }
    }

    // 获取移动输入向量，已经在InputAction中进行了归一化
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

    // 进行交互（如果有需求的话）
    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    // 按下按键触发暂停
    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

}
