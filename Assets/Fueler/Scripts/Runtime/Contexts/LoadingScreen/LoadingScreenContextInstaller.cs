using Fueler.Content.LoadingScreen.LoadingScreenUi;
using Fueler.Context.LoadingScreen.UseCases.Show;
using Juce.Core.DI.Builder;
using Juce.CoreUnity.Contexts;
using Juce.CoreUnity.ViewStack;
using JuceUnity.Core.DI.Extensions;

namespace Fueler.Contexts.LoadingScreen
{
    public class LoadingScreenContextInstaller : IContextInstaller<LoadingScreenContextInstance>
    {
        public void Install(IDIContainerBuilder container, LoadingScreenContextInstance instance)
        {
            container.Bind(instance.LoadingScreenUiInstaller);

            container.Bind<IUiViewStack>().FromServicesLocator();

            container.Bind<IShowUseCase>()
                .FromFunction(c => new ShowUseCase(
                    c.Resolve<IUiViewStack>(),
                    c.Resolve<ILoadingScreenUiInteractor>()
                    ));

            container.Bind<ILoadingScreenContextInteractor>()
                .FromFunction(c => new LoadingScreenContextInteractor(
                    c.Resolve<IShowUseCase>()
                    ));
        }
    }
}
