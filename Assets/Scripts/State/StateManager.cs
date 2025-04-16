using System;

public class StateManager
{
    private State _currentSate;
    public event Action<State> StateChanged;

    public void SwitchStateTo(State state)
    {
        if (_currentSate != null)
        {
            _currentSate.Exit();
        }
        _currentSate = state;
        _currentSate.Enter();
        StateChanged?.Invoke(_currentSate);
    }
    public void Destroy()
    {
        _currentSate.Exit();
    }
}
