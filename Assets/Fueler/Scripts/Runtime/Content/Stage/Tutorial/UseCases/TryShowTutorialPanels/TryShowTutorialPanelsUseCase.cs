﻿using Fueler.Content.Stage.Tutorial.UseCases.TryShowAstronautsTutorialPanel;
using Fueler.Content.Stage.Tutorial.UseCases.TryShowControlsTutorial;
using Fueler.Content.Stage.Tutorial.UseCases.TryShowFuelTutorialPanel;
using Fueler.Content.Stage.Tutorial.UseCases.TryShowTimeTutorialPanel;
using System.Threading;
using System.Threading.Tasks;

namespace Fueler.Content.Stage.Tutorial.UseCases.TryShowTutorialPanels
{
    public class TryShowTutorialPanelsUseCase : ITryShowTutorialPanelsUseCase
    {
        private readonly ITryShowControlsTutorialPanelUseCase tryShowControlsTutorialPanelUseCase;
        private readonly ITryShowFuelTutorialPanelUseCase tryShowFuelTutorialPanelUseCase;
        private readonly ITryShowAstronautsTutorialPanelUseCase tryShowAstronautsTutorialPanelUseCase;
        private readonly ITryShowTimeTutorialPanelUseCase tryShowTimeTutorialPanelUseCase;

        public TryShowTutorialPanelsUseCase(
            ITryShowControlsTutorialPanelUseCase tryShowControlsTutorialPanelUseCase,
            ITryShowFuelTutorialPanelUseCase tryShowFuelTutorialPanelUseCase,
            ITryShowAstronautsTutorialPanelUseCase tryShowAstronautsTutorialPanelUseCase,
            ITryShowTimeTutorialPanelUseCase tryShowTimeTutorialPanelUseCase
            )
        {
            this.tryShowControlsTutorialPanelUseCase = tryShowControlsTutorialPanelUseCase;
            this.tryShowFuelTutorialPanelUseCase = tryShowFuelTutorialPanelUseCase;
            this.tryShowAstronautsTutorialPanelUseCase = tryShowAstronautsTutorialPanelUseCase;
            this.tryShowTimeTutorialPanelUseCase = tryShowTimeTutorialPanelUseCase;
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            await tryShowControlsTutorialPanelUseCase.Execute(cancellationToken);
            await tryShowFuelTutorialPanelUseCase.Execute(cancellationToken);
            await tryShowAstronautsTutorialPanelUseCase.Execute(cancellationToken);
            await tryShowTimeTutorialPanelUseCase.Execute(cancellationToken);
        }
    }
}
