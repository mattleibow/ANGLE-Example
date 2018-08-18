#pragma once

#include "app.g.h"
#include "MainPage.xaml.h"

namespace UniversalANGLE
{
    ref class App sealed
    {
    public:
        App();
        virtual void OnLaunched(Windows::ApplicationModel::Activation::LaunchActivatedEventArgs^ e) override;

    private:
        MainPage ^ mPage;
    };
}
