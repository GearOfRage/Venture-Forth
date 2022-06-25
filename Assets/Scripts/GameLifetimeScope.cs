using VContainer;
using VContainer.Unity;
using UnityEngine;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] GameLogic gameLogic;
    [SerializeField] TilesGeneration tilesGeneration;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<TilesField>(Lifetime.Singleton);
        builder.Register<DamageService>(Lifetime.Singleton);

        builder.RegisterComponent(gameLogic);
        builder.RegisterComponent(tilesGeneration);
    }
}
