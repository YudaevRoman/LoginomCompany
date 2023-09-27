namespace Source;

public interface ISource
{
    bool CheckEnd { get; }
    void Move();
    void Restart();
}
