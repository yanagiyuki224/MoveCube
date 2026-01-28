using UnityEngine;
namespace DialogSystem
{
    public class DialogOption
    {
        public System.Action OnClose;
    }

    public abstract class DialogBase<T> : MonoBehaviour where T : DialogOption
    {
        protected T option;

        public static string prefabName;
        protected static GameObject prefab;

        public static U Show<U>(T optionData) where U : DialogBase<T>
        {
            if (prefab == null)
                prefab = Resources.Load(prefabName) as GameObject;

            GameObject obj = Instantiate(prefab);
            U dialog = obj.GetComponent<U>();
            dialog.UpdateContent(optionData);
            return dialog;
        }

        public virtual void UpdateContent(T opt)
        {
            option = opt;
        }

        public virtual void Close()
        {
            option?.OnClose?.Invoke();
            Destroy(gameObject);
        }
    }
}
