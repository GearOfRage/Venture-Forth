using VContainer;
using VContainer.Unity;
using UnityEngine;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] GameLogic gl;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<TilesField>(Lifetime.Singleton);

        builder.Register<DamageService>(Lifetime.Singleton);
        builder.RegisterComponent(gl);
    }
}
