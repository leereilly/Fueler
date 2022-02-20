﻿using Juce.Core.Loading;
using System.Threading;
using System.Threading.Tasks;

namespace Fueler.Contexts.LoadingScreen
{
    public interface ILoadingScreenContextInteractor
    {
        Task<ILoadingToken> Show(CancellationToken cancellationToken);
    }
}
