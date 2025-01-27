﻿using Fueler.Content.Services.Configuration;
using Fueler.Content.Shared.Levels.Configuration;
using Fueler.Content.Stage.Accessibility.UseCases.IsTimeInfinite;
using Fueler.Content.Stage.General.Data;
using Fueler.Content.Stage.General.UseCases.TryEndStage;
using Fueler.Content.Stage.Time.Data;
using Fueler.Content.Stage.Time.Tickables.UpdateTime;
using Fueler.Content.Stage.Time.UseCases.InitTime;
using Fueler.Content.Stage.Time.UseCases.StopTime;
using Fueler.Content.Stage.Time.UseCases.TryEndStageIfTimeRunOut;
using Fueler.Content.Stage.Time.UseCases.TryShowLowTimeWarning;
using Fueler.Content.Stage.Time.UseCases.TryStartTime;
using Fueler.Content.Stage.Time.UseCases.UpdateTime;
using Fueler.Content.StageUi.Ui.Level;
using Juce.Core.DI.Builder;
using Juce.CoreUnity.Tickables;

namespace Fueler.Content.Stage.Time.Installers
{
    public static class TimeInstaller
    {
        public static void InstallTime(this IDIContainerBuilder container)
        {
            container.Bind<TimeData>().FromNew();

            container.Bind<IInitTimeUseCase>()
                .FromFunction(c => new InitTimeUseCase(
                    c.Resolve<TimeData>(),
                    c.Resolve<ILevelUiInteractor>(),
                    c.Resolve<ILevelConfiguration>(),
                    c.Resolve<IIsTimeInfiniteUseCase>()
                    ));

            container.Bind<ITryStartTimeUseCase>()
                .FromFunction(c => new TryStartTimeUseCase(
                    c.Resolve<TimeData>()
                    ));

            container.Bind<ITryEndStageIfTimeRunOutUseCase>()
                .FromFunction(c => new TryEndStageIfTimeRunOutUseCase(
                    c.Resolve<StageStateData>(),
                    c.Resolve<TimeData>(),
                    c.Resolve<ITryEndStageUseCase>()
                    ));

            container.Bind<ITryShowLowTimeWarningUseCase>()
                .FromFunction(c => new TryShowLowTimeWarningUseCase(
                    c.Resolve<TimeData>(),
                    c.Resolve<ILevelUiInteractor>(),
                    c.Resolve<IConfigurationService>().TimeConfiguration
                    ));

            container.Bind<IUpdateTimeUseCase>()
                .FromFunction(c => new UpdateTimeUseCase(
                    c.Resolve<TimeData>(),
                    c.Resolve<ILevelUiInteractor>(),
                    c.Resolve<ITryShowLowTimeWarningUseCase>()
                    ));

            container.Bind<TimeTickables>()
                .FromFunction(c => new TimeTickables(
                    c.Resolve<IUpdateTimeUseCase>(),
                    c.Resolve<ITryEndStageIfTimeRunOutUseCase>()
                    ))
                .WhenInit((c, o) => c.Resolve<ITickablesService>().Add(o))
                .WhenDispose((c, o) => c.Resolve<ITickablesService>().Remove(o))
                .NonLazy();

            container.Bind<IStopTimeUseCase>()
                .FromFunction(c => new StopTimeUseCase(
                    c.Resolve<TimeData>()
                    ));
        }
    }
}
