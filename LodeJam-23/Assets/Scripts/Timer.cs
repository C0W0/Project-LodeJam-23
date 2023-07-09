using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public delegate void TimerFinishCallback();
public struct TimerParam
{
	public DateTime startTime;
	public int duration;
	public TimerFinishCallback callback;
}

public class Timer : MonoBehaviour
{
	public static Timer Instance;
	private Dictionary<int, TimerParam> _processes;

	private void Awake()
	{
		Instance = this;
		_processes = new Dictionary<int, TimerParam>();
	}

	public void AddProcess(int duration, TimerFinishCallback callback)
	{
		int processId = Random.Range(1, 10000);
		while (_processes.ContainsKey(processId))
		{
			processId = Random.Range(1, 10000);
		}
		
		_processes.Add(processId, new TimerParam
		{
			callback = callback,
			duration = duration,
			startTime = DateTime.Now
		});
	}

    // Update is called once per frame
    void Update()
    {
	    List<int> processesToRemove = new List<int>();
	    DateTime now = DateTime.Now;
	    foreach (var (pid, param) in _processes)
	    {
		    if ((now - param.startTime).Seconds > param.duration)
		    {
			    param.callback.Invoke();
			    processesToRemove.Add(pid);
		    }
	    }

	    foreach (int pid in processesToRemove)
	    {
		    _processes.Remove(pid);
	    }
    }
}
