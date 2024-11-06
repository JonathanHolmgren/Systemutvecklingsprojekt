namespace PresentationLayer.Services;

public static class Mediator
{
    private static readonly Dictionary<string, Action<object>> Actions = new();

    public static void Register(string token, Action<object> callback)
    {
        if (!Actions.ContainsKey(token))
        {
            Actions[token] = callback;
        }
    }

    public static void Notify(string token, object args = null)
    {
        if (Actions.ContainsKey(token))
        {
            Actions[token].Invoke(args);
        }
    }
}
