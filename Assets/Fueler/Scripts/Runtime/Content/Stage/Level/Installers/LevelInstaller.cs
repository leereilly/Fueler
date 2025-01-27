﻿using Fueler.Content.Services.Configuration;
using Fueler.Content.Shared.Levels.Configuration;
using Fueler.Content.Shared.Levels.UseCases.LoadNextLevel;
using Fueler.Content.Shared.Levels.UseCases.LoadPreviousLevel;
using Fueler.Content.Shared.Levels.UseCases.ReloadLevel;
using Fueler.Content.Shared.Levels.UseCases.TryGetLevelByIndex;
using Fueler.Content.Shared.Levels.UseCases.TryGetLevelIndexByLevelId;
using Fueler.Content.Stage.General.Data;
using Fueler.Content.Stage.General.Entities;
using Fueler.Content.Stage.General.Factories;
using Fueler.Content.Stage.General.UseCases.LoadLevel;
using Fueler.Content.Stage.General.UseCases.ShipCollidedWithEnd;
using Fueler.Content.Stage.General.UseCases.TryEndStage;
using Fueler.Contexts.Shared.UseCases.UnloadAndLoadStage;
using Fueler.Contexts.Stage;
using Juce.Core.DI.Builder;
using Juce.Core.Disposables;
using Juce.Core.Factories;
using Juce.Core.Repositories;

namespace Fueler.Content.Stage.General.Installers
{
    public static class LevelInstaller
    {
        public static void InstallLevel(this IDIContainerBuilder container)
        {
            container.Bind<IFactory<LevelEntityFactoryDefinition, IDisposable<LevelEntity>>>().FromFunction(c => new LevelEntityFactory(
                c.Resolve<StageContextInstance>().LevelEntityParent
                ));

            container.Bind<ISingleRepository<IDisposable<LevelEntity>>>().FromInstance(
                new SimpleSingleRepository<IDisposable<LevelEntity>>()
                );

            container.Bind<ITryGetLevelIndexByLevelIdUseCase>()
                .FromFunction(c => new TryGetLevelIndexByLevelIdUseCase(
                    c.Resolve<IConfigurationService>().LevelsConfiguration
                    ));

            container.Bind<ITryGetLevelByIndexUseCase>()
                .FromFunction(c => new TryGetLevelByIndexUseCase(
                    c.Resolve<IConfigurationService>().LevelsConfiguration
                    ));

            container.Bind<ILoadNextLevelUseCase>()
                .FromFunction(c => new LoadNextLevelUseCase(
                    c.Resolve<ILevelConfiguration>(),
                    c.Resolve<ITryGetLevelIndexByLevelIdUseCase>(),
                    c.Resolve<ITryGetLevelByIndexUseCase>(),
                    c.Resolve<IUnloadAndLoadStageUseCase>()
                    ));

            container.Bind<ILoadPreviousLevelUseCase>()
                .FromFunction(c => new LoadPreviousLevelUseCase(
                    c.Resolve<ILevelConfiguration>(),
                    c.Resolve<ITryGetLevelIndexByLevelIdUseCase>(),
                    c.Resolve<ITryGetLevelByIndexUseCase>(),
                    c.Resolve<IUnloadAndLoadStageUseCase>()
                    ));

            container.Bind<IReloadLevelUseCase>()
                .FromFunction(c => new ReloadLevelUseCase(
                    c.Resolve<ILevelConfiguration>(),
                    c.Resolve<IUnloadAndLoadStageUseCase>()
                    ));

            container.Bind<ILoadLevelUseCase>().FromFunction(c => new LoadLevelUseCase(
                c.Resolve<IFactory<LevelEntityFactoryDefinition, IDisposable<LevelEntity>>>(),
                c.Resolve<ISingleRepository<IDisposable<LevelEntity>>>()
                ));

            container.Bind<IShipCollidedWithEndUseCase>()
                .FromFunction(c => new ShipCollidedWithEndUseCase(
                    c.Resolve<ITryEndStageUseCase>()
                    ));
        }
    }
}
