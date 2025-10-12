namespace EightQueensRobot.IKSolver;

public class Firefly<TData, TOutput>(TData data)
    where TData : class
{
    public TData Data { get; set; } = data;
    public float? Fitness { get; set; } = null;
    public TOutput? Output { get; set; } = default;
    public float Brightness => Fitness is not null ? 1 / Fitness.Value : 0;

    public Firefly<TData, TOutput> Clone()
    {
        Firefly<TData, TOutput> newFirefly = new Firefly<TData, TOutput>(Data);
        newFirefly.Fitness = Fitness;
        newFirefly.Output = Output;
        return newFirefly;
    }
}