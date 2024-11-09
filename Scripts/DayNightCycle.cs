using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public static DayNightCycle Instance { get; private set; }

    [Range(0.0f, 1.0f)]
    public float time; //�Ϸ��� �ð�
    public float fullDayLength; //��ü���� �Ϸ� ����
    public float startTime = 0.4f;
    private float timeRate;
    public Vector3 noon; //����(vector 90 0 0)

    [Header("Sun")]
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntensity;

    [Header("Moon")]
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;

    [Header("Other Lighting")]
    public AnimationCurve lightingIntensityMultiplier; //���� ȯ���� ��
    public AnimationCurve reflectionIntensityMultiplier; //���� ������Ʈ�� �ݻ�Ǵ� ����
    private void Awake()
    {
        // Singleton ���� ����
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        timeRate = 1.0f / fullDayLength;
        time = startTime; //�뷫 9������..
    }

    private void Update()
    {
        time = (time + timeRate * Time.deltaTime) % 1.0f;

        UpdateLighting(sun, sunColor, sunIntensity);
        UpdateLighting(moon, moonColor, moonIntensity);

        //��ü���� ȯ���� ���� ����
        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);
        //������Ʈ�� �ݻ�����
        RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time);

    }

    void UpdateLighting(Light lightSource, Gradient colorGradiant, AnimationCurve intensityCurve)
    {
        float intensity = intensityCurve.Evaluate(time);

        //���϶��� 0.25�� ���� 90���� ������ , ���϶��� 0.75�� ����
        lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f)) * noon * 4.0f;
        lightSource.color = colorGradiant.Evaluate(time);
        lightSource.intensity = intensity;

        GameObject go = lightSource.gameObject;
        if (lightSource.intensity == 0 && go.activeInHierarchy)
            go.SetActive(false);
        else if (lightSource.intensity > 0 && !go.activeInHierarchy)
            go.SetActive(true);
    }
    public float GetCurrentHour()
    {
        return time * 24f;
    }
    public void SetCurrentTime(float hour)
    {
        time = hour / 24f;
    }

}