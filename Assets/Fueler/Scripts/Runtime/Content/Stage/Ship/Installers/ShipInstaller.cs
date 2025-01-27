﻿using Fueler.Content.Stage.Ship.UseCases.ShipCollided;
using Fueler.Content.Stage.General.Entities;
using Fueler.Content.Stage.Ship.Entities;
using Fueler.Content.Stage.Ship.Factories;
using Fueler.Content.Stage.Ship.UseCases.LoadShip;
using Fueler.Content.Stage.Ship.UseCases.SetShipInitialPosition;
using Fueler.Content.Stage.Ship.UseCases.SetupShipCamera;
using Fueler.Contexts.Stage;
using Juce.Core.DI.Builder;
using Juce.Core.Disposables;
using Juce.Core.Factories;
using Juce.Core.Repositories;
using Fueler.Content.Stage.Astrounats.UseCases.ShipCollidedWithAstronaut;
using Fueler.Content.Stage.Ship.UseCases.ShipCollidedWithShipKiller;
using Fueler.Content.Stage.General.UseCases.TryEndStage;
using Fueler.Content.Stage.General.UseCases.ShipCollidedWithEnd;
using Fueler.Content.Stage.Ship.UseCases.ShipMoves;
using Fueler.Content.Stage.Fuel.UseCases.ShipFuelUsed;
using Fueler.Content.Stage.Time.UseCases.TryStartTime;

namespace Fueler.Content.Stage.Ship.Installers
{
    public static class ShipInstaller
    {
        public static void InstallShip(this IDIContainerBuilder container)
        {
            container.Bind<IFactory<ShipEntityFactoryDefinition, IDisposable<ShipEntity>>>()
                .FromFunction(c => new ShipEntityFactory(
                    c.Resolve<StageContextInstance>().ShipEntityPrefab,
                    c.Resolve<StageContextInstance>().ShipEntityParent
                    ));

            container.Bind<ISingleRepository<IDisposable<ShipEntity>>>()
                .FromInstance(new SimpleSingleRepository<IDisposable<ShipEntity>>());

            container.Bind<ILoadShipUseCase>().FromFunction(c => new LoadShipUseCase(
                c.Resolve<IFactory<ShipEntityFactoryDefinition, IDisposable<ShipEntity>>>(),
                c.Resolve<ISingleRepository<IDisposable<ShipEntity>>>()
                ));

            container.Bind<ISetShipInitialPositionUseCase>()
                .FromFunction(c => new SetShipInitialPositionUseCase(
                    c.Resolve<ISingleRepository<IDisposable<LevelEntity>>>()
                    ));

            container.Bind<ISetupShipCameraUseCase>()
                .FromFunction(c => new SetupShipCameraUseCase(
                    c.Resolve<StageContextInstance>().ShipVirtualCamera,
                    c.Resolve<StageContextInstance>().CameraConfiner,
                    c.Resolve<ISingleRepository<IDisposable<LevelEntity>>>()
                    ));

            container.Bind<IShipMovesUseCase>()
                .FromFunction(c => new ShipMovesUseCase(
                    c.Resolve<IShipFuelUsedUseCase>(),
                    c.Resolve<ITryStartTimeUseCase>()
                    ));

            container.Bind<IShipCollidedWithShipKillerUseCase>()
                .FromFunction(c => new ShipCollidedWithShipKillerUseCase(
                    c.Resolve<ISingleRepository<IDisposable<ShipEntity>>>(),
                    c.Resolve<ITryEndStageUseCase>()
                    ));

            container.Bind<IShipCollidedUseCase>()
                .FromFunction(c => new ShipCollidedUseCase(
                    c.Resolve<IShipCollidedWithEndUseCase>(),
                    c.Resolve<IShipCollidedWithShipKillerUseCase>(),
                    c.Resolve<IShipCollidedWithAstronautUseCase>()
                    ));
        }
    }
}
