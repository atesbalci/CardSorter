using UnityEngine;

namespace Helpers.Rendering
{
    /// <summary>
    /// A simple MonoBehaviour which just passes on the time this object is enabled to a uniform shader variable.
    /// </summary>
    public class SelectedShaderHelper : MonoBehaviour
    {
        private void OnEnable()
        {
            Shader.SetGlobalFloat("_LastSelectTime", Time.time);
        }
    }
}
