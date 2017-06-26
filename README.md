# FreePoolManager
Generic Dictionary Pool

Cons.
Dictionary(Faster than what i used >w<)
Objects are removed from pool when it used.(More Faster search even if many objects are in pool)
Clean up pool automatically with hiearchy.

Usage
-------------------

1. Inherit FreePoolableBehavior.
2. Create pool with FreePoolManager.CreatePool
3. Reuse or create objects with FreePoolManager.GetReuseableObject
4. Return object to pool with (Subclass object).ReturnToPool

