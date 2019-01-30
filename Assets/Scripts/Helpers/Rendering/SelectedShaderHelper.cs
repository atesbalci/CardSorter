using UnityEngine;

namespace Helpers.Rendering
{
    public class SelectedShaderHelper : MonoBehaviour
    {
        private void OnEnable()
        {
            Shader.SetGlobalFloat("_LastSelectTime", Time.time);
        }
    }
}
