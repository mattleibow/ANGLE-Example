#pragma once

#include "OpenGLES.h"
#include "MainPage.g.h"

using namespace Concurrency;
using namespace Windows::Foundation;
using namespace Windows::UI::Core;
using namespace Windows::UI::Xaml;

namespace UniversalANGLE
{
    public ref class MainPage sealed
    {
    public:
        MainPage();
        virtual ~MainPage();

    private:
        void OnPageLoaded(Platform::Object^ sender, RoutedEventArgs^ e);
        void OnVisibilityChanged(CoreWindow^ sender, VisibilityChangedEventArgs^ args);
        void CreateRenderSurface();
        void DestroyRenderSurface();
        void RecoverFromLostDevice();
        void StartRenderLoop();
        void StopRenderLoop();

        OpenGLES mOpenGLES;
        EGLSurface mRenderSurface; // This surface is associated with a swapChainPanel on the page

        critical_section mRenderSurfaceCriticalSection;
        IAsyncAction^ mRenderLoopWorker;
    };
}
