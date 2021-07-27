using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class Pool
{
    public GameObject Prefab
    {
        get
        {
            return prefab;
        }
    }

    [SerializeField] GameObject prefab;

    [SerializeField] int size = 1;

    Queue<GameObject> queue;

    Transform parent;

    #region 生成备用对象
    // 初始化队列，复制体入队列
    public void Initialize(Transform parent)
    {
        queue = new Queue<GameObject>();
        this.parent = parent;

        for (var i = 0; i < size; i++)
        {
            queue.Enqueue(Copy());
        }
    }

    // 生成复制体
    GameObject Copy()
    {
        var copy = GameObject.Instantiate(prefab, parent);

        copy.SetActive(false);

        return copy;
    }
    #endregion

    #region 从池中取出一个可用对象
    // 从池中取出可用对象，出队列并获取出队列的元素
    GameObject AvailableObject()
    {
        GameObject availableObject = null;

        if(queue.Count > 0 && !queue.Peek().activeSelf)
        {
            availableObject = queue.Dequeue();
        }
        else
        {
            availableObject = Copy();
        }

        queue.Enqueue(availableObject);

        return availableObject;
    }
    #endregion

    #region 启用可用对象
    public GameObject preparedObject()
    {
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);

        return preparedObject;
    }

    public GameObject preparedObject(Vector3 position)
    {
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);
        preparedObject.transform.position = position;

        return preparedObject;
    }

    public GameObject preparedObject(Vector3 position, GameObject parent)
    {
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);
        preparedObject.transform.position = position;
        preparedObject.transform.SetParent(parent.transform);

        return preparedObject;
    }

    public GameObject preparedObject(Vector3 position, Quaternion rotation)
    {
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);
        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;

        return preparedObject;
    }

    public GameObject preparedObject(Vector3 position, Quaternion rotation, Vector3 localScale)
    {
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);
        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;
        preparedObject.transform.localScale = localScale;

        return preparedObject;
    }
    #endregion

    #region 让完成任务的对象返回对象池
    public void Return(GameObject gameObject)
    {
        gameObject.SetActive(false);

        queue.Enqueue(gameObject);
    }
    #endregion
}
