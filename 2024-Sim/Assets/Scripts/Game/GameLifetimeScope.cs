using Player;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private PlayerMovementModel playerMovementModel;
    
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<PlayerMover>();
        
        /* MonoBehaviour Object */
        builder.RegisterComponent(playerMovementModel);
    }
}