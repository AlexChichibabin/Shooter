using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    private static GameObject TimerCollector;

    public UnityAction OnTimeRunOut;
    public UnityAction OnTick;

    private float maxTime;
    private float currentTime;
    private bool isPaused;

    public bool IsLoop;

    public float MaxTime => maxTime;
    public float CurrentTime => currentTime;
    public bool IsPaused => isPaused;
    public bool IsCompleted => currentTime <= 0;

    private void Update()
    {
        if(isPaused) return;

        currentTime -= Time.deltaTime;

        OnTick?.Invoke();

        if (currentTime <= 0)
        {
            currentTime = 0;
            OnTimeRunOut?.Invoke();

            if (IsLoop == true)
            {
                currentTime = maxTime;
            }
        }
    }
    public static Timer CreateTimer(float time, bool isLoop)
    {
        if (TimerCollector == null)
        {
            TimerCollector = new GameObject("Timers");
        }

        Timer timer = TimerCollector.AddComponent<Timer>();
        timer.maxTime = time;
        timer.IsLoop = isLoop;

        return timer;
    }
    public static Timer CreateTimer(float time)
    {
        if (TimerCollector == null)
        {
            TimerCollector = new GameObject("Timers");
        }

        Timer timer = TimerCollector.AddComponent<Timer>();
        timer.maxTime = time;
        timer.currentTime = timer.maxTime;
        timer.IsLoop = false;

        return timer;
    }
    public void Play() => isPaused = false;
    public void Pause() => isPaused = true;

    public void Complete()
    {
        isPaused = false;
        currentTime = 0;
    }
    public void CompleteWithoutEvent() => Destroy(this);

    public void Restart(float time)
    {
        maxTime = time;
        currentTime = maxTime;
    }
    public void Restart() => currentTime = maxTime;
}
