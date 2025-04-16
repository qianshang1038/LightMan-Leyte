using UnityEngine;

/// <summary>
/// ����ģʽĬ���ڳ����л�ʱ�����٣����Ը�д ShouldDestroyOnLoad() ���޸�
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    [SerializeField] private bool shouldDestroyOnLoad = false; // Ĭ���ڳ����л�ʱ�򱣴�����
    private static T instance;

    public static T Instance
    {
        // �������๹�췽��
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<T>() as T;
                if (instance == null)
                {
                    //ͨ������newһ��ͬ����GameObject����������ű�
                    GameObject gameObject = new GameObject(typeof(T).Name);

                    // ������ű�����ʱ�����Awake����
                    instance = gameObject.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            // ����instance=Activator.CreateInstance<T>();
            //  ��instance=Activator.CreateInstance<T>()������MonoBehaviour��ʹ��,��ҪT���޲����Ĺ������캯��
            //  ����Activator.CreateInstance(typeof(T),true) ��ҪT���޲εĹ���/˽�еĹ��캯���������÷��俪����
            instance = (T)this;

            if (shouldDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ����һ�����Ը�д�Ĳ��֣������ڳ����л�ʱ������
    protected virtual bool ShouldDestroyOnLoad() => shouldDestroyOnLoad;

}