﻿using Fueler.Content.Services.Configuration;
using Fueler.Content.Shared.Levels.Configuration;
using Fueler.Content.Stage.Accessibility.UseCases.IsFuelInfinite;
using Fueler.Content.Stage.Fuel.Data;
using Fueler.Content.Stage.Fuel.UseCases.CheckShipMovementIfNoFuel;
using Fueler.Content.Stage.Fuel.UseCases.InitFuel;
using Fueler.Content.Stage.Fuel.UseCases.ShipFuelUsed;
using Fueler.Content.Stage.Ship.Entities;
using Fueler.Content.StageUi.Ui.Level;
using Juce.Core.DI.Builder;
using Juce.Core.Disposables;
using Juce.Core.Repositories;

namespace Fueler.Content.Stage.Fuel.Installers
{
    public static class FuelInstaller
    {
        public static void InstallFuel(this IDIContainerBuilder container)
        {
            container.Bind<FuelData>().FromNew();

            container.Bind<IInitFuelUseCase>()
                .FromFunction(c => new InitFuelUseCase(
                    c.Resolve<FuelData>(),
                    c.Resolve<ILevelUiInteractor>(),
                    c.Resolve<ILevelConfiguration>(),
                    c.Resolve<IIsFuelInfiniteUseCase>()
                    ));

            container.Bind<ICheckShipMovementIfNoFuelUseCase>()
                .FromFunction(c => new CheckShipMovementIfNoFuelUseCase(
                    c.Resolve<FuelData>(),
                    c.Resolve<ISingleRepository<IDisposable<ShipEntity>>>()
                    ));

            container.Bind<IShipFuelUsedUseCase>()
                .FromFunction(c => new ShipFuelUsedUseCase(
                    c.Resolve<FuelData>(),
                    c.Resolve<ILevelUiInteractor>(),
                    c.Resolve<IConfigurationService>().FuelConfiguration,
                    c.Resolve<ICheckShipMovementIfNoFuelUseCase>()
                    ));
        }
    }
}
