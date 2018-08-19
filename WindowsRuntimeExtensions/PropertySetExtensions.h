#pragma once

using namespace Platform;
using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;

namespace WindowsRuntimeExtensions
{
    public ref class PropertySetExtensions sealed
    {
    public:
        static void AddSize(PropertySet^ propertySet, String^ key, Size value);
        static void AddSingle(PropertySet^ propertySet, String^ key, float value);
    };
}
