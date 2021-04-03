using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractDebugger : MonoBehaviour {

    /// <summary>
    /// 尝试启用。如果在设置里禁用了此项，调用此方法将无效。
    /// </summary>
    public virtual void TryEnable()
    {
        Enable();
    }

    /// <summary>
    /// 禁用
    /// </summary>
    public abstract void Disable();
    /// <summary>
    /// 启用
    /// </summary>
    public abstract void Enable();

    /// <summary>
    /// 根据设置尝试更新状态（启用或禁用）
    /// </summary>
    public virtual void TryUpdate()
    {
        
    }
}
