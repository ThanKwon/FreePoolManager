# FreePoolManager
Generic Dictionary Pool

cons.
Objects are removed from pool when use.(Faster search even if many objects are in pool)


Usage
-------------------

1. Inherit FreePoolableBehavior.
2. Create pool with FreePoolManager.CreatePool
3. Pool object with FreePoolManager.GetReuseableObject
4. Return object to pool with (Subclass object).ReturnToPool

