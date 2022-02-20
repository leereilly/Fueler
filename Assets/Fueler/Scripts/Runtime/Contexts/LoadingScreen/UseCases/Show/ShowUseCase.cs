﻿using Fueler.Content.LoadingScreen.LoadingScreenUi;
using Juce.Core.Loading;
using System.Threading;
using System.Threading.Tasks;

namespace Fueler.Context.LoadingScreen.UseCases.Show
{
    public class ShowUseCase : IShowUseCase
    {
        private readonly ILoadingScreenUiInteractor loadingScreenUiInteractor;

        public ShowUseCase(
            ILoadingScreenUiInteractor loadingScreenUiInteractor
            )
        {
            this.loadingScreenUiInteractor = loadingScreenUiInteractor;
        }

        public async Task<ILoadingToken> Execute(CancellationToken cancellationToken)
        {
            await loadingScreenUiInteractor.SetVisible(visibe: true, cancellationToken);

            return new CallbackLoadingToken(
                () => loadingScreenUiInteractor.SetVisible(visibe: false, cancellationToken).RunAsync()
                );
        }
    }
}
