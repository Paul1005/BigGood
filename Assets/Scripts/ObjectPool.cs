using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public Poolable poolType;
    public uint initialCount;
    private Stack<Poolable> pool;

	// Use this for initialization
	void Awake ()
    {
        pool = new Stack<Poolable>();
        for (uint i = 0; i < initialCount; i++)
        {
            Poolable obj = Instantiate(poolType);
            obj.Pool = this;
            obj.transform.parent = transform;
            obj.gameObject.SetActive(false);
            pool.Push(obj);
        }
	}

    public Poolable Pop ()
    {
        Poolable obj;
        if (pool.Count == 0)
        {
            // if stack is empty, create a new poolable
            obj = Instantiate(poolType);
            obj.Pool = this;
            obj.transform.parent = transform;
        }
        else
        {
            // otherwise return the topmost poolable
            obj = pool.Pop();
            obj.gameObject.SetActive(true);
            obj.transform.localScale = poolType.transform.localScale;
        }

        return obj;
    }

    public void Push (Poolable obj)
    {
        obj.gameObject.SetActive(false);
        pool.Push(obj);
    }
}
