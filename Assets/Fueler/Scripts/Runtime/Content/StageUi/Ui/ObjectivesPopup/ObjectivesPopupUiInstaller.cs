﻿using Fueler.Content.StageUi.Ui.ObjectivesPopup.Entries;
using Fueler.Content.StageUi.Ui.ObjectivesPopup.Events;
using Fueler.Content.StageUi.Ui.ObjectivesPopup.UseCases.EnableObjective;
using Fueler.Content.StageUi.Ui.ObjectivesPopup.UseCases.HidePanelOnAnyKeyPress;
using Fueler.Content.StageUi.Ui.ObjectivesPopup.UseCases.SubscribeToAnyKeyPress;
using Juce.Core.DI.Builder;
using Juce.Core.DI.Installers;
using Juce.Core.Refresh;
using Juce.CoreUnity.TweenComponent;
using Juce.CoreUnity.ViewStack;
using Juce.CoreUnity.ViewStack.Entries;
using Juce.CoreUnity.Visibles;
using Juce.Input.InputActions;
using System.Collections.Generic;
using UnityEngine;

namespace Fueler.Content.StageUi.Ui.ObjectivesPopup
{
    public class ObjectivesPopupUiInstaller : MonoBehaviour, IInstaller
    {
        [Header("Animations")]
        [SerializeField] private TweenPlayerAnimation showAnimation = default;
        [SerializeField] private TweenPlayerAnimation hideAnimation = default;

        [Header("Contents")]
        [SerializeField] private List<ObjectiveEntry> objectivesEntries = default;

        private readonly AnyKeyInputAction anyKeyInputAction = new AnyKeyInputAction();

        public void Install(IDIContainerBuilder container)
        {
            container.Bind<ObjectivesPopupViewStackEntry>()
                .FromFunction(c => new ObjectivesPopupViewStackEntry(
                    typeof(IObjectivesPopupUiInteractor),
                    gameObject.transform,
                    new TweenPlayerAnimationVisible(
                        showAnimation,
                        hideAnimation
                        ),
                    isPopup: false,
                    new ViewStackEntryRefresh(
                        RefreshType.AfterShow, 
                        new CallbackRefreshable(c.Resolve<ISubscribeToAnyKeyPressUseCase>().Execute)
                        )
                    ));

            container.Bind<ObjectivesPopupEvents>().FromNew();

            container.Bind<IEnableObjectiveUseCase>()
                .FromFunction(c => new EnableObjectiveUseCase(
                    objectivesEntries
                    ));

            container.Bind<IHidePanelOnAnyKeyPressUseCase>()
                .FromFunction(c => new HidePanelOnAnyKeyPressUseCase(
                    c.Resolve<IUiViewStack>(),
                    c.Resolve<ObjectivesPopupEvents>()
                    ));

            container.Bind<ISubscribeToAnyKeyPressUseCase>()
                .FromFunction(c => new SubscribeToAnyKeyPressUseCase(
                    anyKeyInputAction,
                    c.Resolve<IHidePanelOnAnyKeyPressUseCase>()
                    ));

            container.Bind<IObjectivesPopupUiInteractor>()
                .FromFunction(c => new ObjectivesPopupUiInteractor(
                    c.Resolve<ObjectivesPopupEvents>(),
                    c.Resolve<IEnableObjectiveUseCase>()
                    ))
                .WhenInit((c, o) => c.Resolve<IUiViewStack>().Register(c.Resolve<ObjectivesPopupViewStackEntry>()))
                .WhenDispose((c, o) => c.Resolve<IUiViewStack>().Unregister(c.Resolve<ObjectivesPopupViewStackEntry>()))
                .NonLazy();
        }
    }
}
