﻿using System;

[Serializable]
public class Timer {
    public float Total;
    public float Current => _current;

    [ReadOnly] public float _current;

    public void Tick() {
        _current -= 1;
    }

    public void Reset() {
        _current = Total;
    }
}