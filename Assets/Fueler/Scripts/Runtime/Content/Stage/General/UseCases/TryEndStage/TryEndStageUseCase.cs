﻿using Fueler.Content.Stage.Astrounats.UseCases.TryShowNeedToCollectAllAstronatusToaster;
using Fueler.Content.Stage.General.UseCases.AreStageObjectivesCompleted;
using Fueler.Content.Stage.General.UseCases.EndStage;
using Fueler.Content.Stage.Level.Data;

namespace Fueler.Content.Stage.General.UseCases.TryEndStage
{
    public class TryEndStageUseCase : ITryEndStageUseCase
    {
        private readonly ITryShowNeedToCollectAllAstronatusToasterUseCase tryShowNeedToCollectAllAstronatusToasterUseCase;
        private readonly IAreStageObjectivesCompletedUseCase areStageObjectivesCompletedUseCase;
        private readonly IEndStageUseCase endStageUseCase;

        public TryEndStageUseCase(
            ITryShowNeedToCollectAllAstronatusToasterUseCase tryShowNeedToCollectAllAstronatusToasterUseCase,
            IAreStageObjectivesCompletedUseCase areStageObjectivesCompletedUseCase,
            IEndStageUseCase endStageUseCase
            )
        {
            this.tryShowNeedToCollectAllAstronatusToasterUseCase = tryShowNeedToCollectAllAstronatusToasterUseCase;
            this.areStageObjectivesCompletedUseCase = areStageObjectivesCompletedUseCase;
            this.endStageUseCase = endStageUseCase;
        }

        public void Execute(LevelEndData levelEndedData)
        {
            if(levelEndedData.DestroyShip)
            {
                endStageUseCase.Execute(levelEndedData);
                return;
            }

            bool isCompleted = areStageObjectivesCompletedUseCase.Execute();

            if(!isCompleted)
            {
                tryShowNeedToCollectAllAstronatusToasterUseCase.Execute();
                return;
            }

            endStageUseCase.Execute(levelEndedData);
        }
    }
}
