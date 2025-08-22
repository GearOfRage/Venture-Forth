using VContainer;
using VContainer.Unity;
using UnityEngine;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] GameLogic gameLogic;
    [SerializeField] TilesGeneration tilesGeneration;
    [SerializeField] StatsProjection statsProjection;

    protected override void Configure(IContainerBuilder builder)
    {
        // Model - some data
        builder.Register<TilesField>(Lifetime.Singleton);
        builder.Register<GameStats>(Lifetime.Singleton);

        // View - some element that are visible on UI
        builder.RegisterComponent(gameLogic);
        builder.RegisterComponent(tilesGeneration);
        builder.RegisterComponent(statsProjection);

        // Controller - some behavior / logic / algorithm / etc
        builder.Register<DamageService>(Lifetime.Singleton);
    }
}
