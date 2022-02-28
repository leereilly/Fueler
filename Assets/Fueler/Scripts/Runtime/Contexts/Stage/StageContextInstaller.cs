using Fueler.Content.Stage.General.Installers;
using Fueler.Content.Stage.General.UseCases.LoadStage;
using Fueler.Content.Stage.General.Installers;
using Fueler.Content.Stage.General.UseCases.EndStage;
using Fueler.Content.Stage.Ship.Installers;
using Fueler.Contexts.Stage.UseCases.End;
using Fueler.Contexts.Stage.UseCases.Load;
using Juce.Core.DI.Builder;
using Juce.CoreUnity.Contexts;
using UnityEngine;
using Fueler.Contexts.Stage.UseCases.Start;
using Fueler.Content.Stage.General.UseCases.StartStage;

namespace Fueler.Contexts.Stage
{
    public class StageContextInstaller : MonoBehaviour, IContextInstaller<StageContextInstance>
    {
        public void Install(IDIContainerBuilder container, StageContextInstance instance)
        {
            container.Bind<StageContextInstance>().FromInstance(instance);

            container.InstallServices();
            container.InstallGeneral();
            container.InstallLevel();
            container.InstallShip();

            container.Bind<ILoadUseCase>()
                .FromFunction(c => new LoadUseCase(
                    c.Resolve<ILoadStageUseCase>()
                    ));

            container.Bind<IStartUseCase>()
                .FromFunction(c => new StartUseCase(
                    c.Resolve<IStartStageUseCase>()
                    ));

            container.Bind<IEndUseCase>()
                .FromFunction(c => new EndUseCase(
                    c.Resolve<IEndStageUseCase>()
                    ));

            container.Bind<IStageContextInteractor>()
                .FromFunction(c => new StageContextInteractor(
                    c.Resolve<ILoadUseCase>(),
                    c.Resolve<IStartUseCase>(),
                    c.Resolve<IEndUseCase>()
                    ));
        }
    }
}
