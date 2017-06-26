using UnityEngine;

public class FreePoolableBehavior : MonoBehaviour
{
    protected PoolObjectInstance _poolItem;

    public virtual void ReuseObject(PoolObjectInstance poolItem)
    {
        _poolItem = poolItem;
    }

    protected virtual string GetDisabledObjectHolderName()
    {
        return "ObjectPoolGrave";
    }
    
    protected void ReturnToPool()
    {
        if (_poolItem == null)
        {
            Destroy(this);
            return;
        }

        FreePoolManager.instance.ReturnToPool(_poolItem, GetDisabledObjectHolderName());
        return;
    }
}