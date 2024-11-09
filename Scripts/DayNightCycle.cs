using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public static DayNightCycle Instance { get; private set; }

    [Range(0.0f, 1.0f)]
    public float time; //하루의 시간
    public float fullDayLength; //전체적인 하루 길이
    public float startTime = 0.4f;
    private float timeRate;
    public Vector3 noon; //정오(vector 90 0 0)

    [Header("Sun")]
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntensity;

    [Header("Moon")]
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;

    [Header("Other Lighting")]
    public AnimationCurve lightingIntensityMultiplier; //실제 환경의 빛
    public AnimationCurve reflectionIntensityMultiplier; //실제 오브젝트에 반사되는 정도
    private void Awake()
    {
        // Singleton 패턴 설정
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
        time = startTime; //대략 9시정도..
    }

    private void Update()
    {
        time = (time + timeRate * Time.deltaTime) % 1.0f;

        UpdateLighting(sun, sunColor, sunIntensity);
        UpdateLighting(moon, moonColor, moonIntensity);

        //전체적인 환경의 조명 세기
        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);
        //오브젝트의 반사정도
        RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time);

    }

    void UpdateLighting(Light lightSource, Gradient colorGradiant, AnimationCurve intensityCurve)
    {
        float intensity = intensityCurve.Evaluate(time);

        //낮일때는 0.25를 뺴줘 90도로 맞춰줌 , 밤일때는 0.75를 빼줌
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