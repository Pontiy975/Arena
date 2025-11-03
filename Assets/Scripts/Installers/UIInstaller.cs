using UnityEngine;
using Zenject;

public class UIInstaller : MonoInstaller
{
    [SerializeField] private CustomJoystick joystick;

    public override void InstallBindings()
    {
        Container.Bind<CustomJoystick>()
                 .FromComponentInHierarchy(joystick)
                 .AsSingle();
    }
}