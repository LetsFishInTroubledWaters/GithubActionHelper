using System.Reflection;

namespace GithubActionHelper.Proxy;

public class DynamicProxy<T> : DispatchProxy
{
    private static T Wrapped { get; set; }

    public static T Create(T @object)
    {
        Wrapped = @object;
        return DispatchProxy.Create<T, DynamicProxy<T>>();
    }

    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        Console.WriteLine("start invoke");
        var invoke = targetMethod?.Invoke(Wrapped, args);
        Console.WriteLine("end invoke");
        return invoke;
    }
}