using UnityEngine;
using UnityEngine.Perception.Randomization.Randomizers;
using UnityEngine.Perception.Randomization.Parameters;

[AddRandomizerMenu("Perception/Light Rotation Randomizer")]
public class LightRotationRandomizer : Randomizer
{
    public FloatParameter rotationX;
    public FloatParameter rotationY;
    private Light directionalLight;

    protected override void OnAwake()
    {
        // Find the Directional Light in the scene
        directionalLight = GameObject.FindObjectOfType<Light>();
        if (directionalLight == null || directionalLight.type != LightType.Directional)
        {
            Debug.LogError("No Directional Light found in the scene.");
        }
    }

    protected override void OnIterationStart()
    {
        if (directionalLight != null)
        {
            // Randomize the X and Y rotation
            directionalLight.transform.rotation = Quaternion.Euler(rotationX.Sample(), rotationY.Sample(), 0);
            Debug.Log($"Directional Light rotated to {directionalLight.transform.rotation.eulerAngles}");
        }
    }
}
