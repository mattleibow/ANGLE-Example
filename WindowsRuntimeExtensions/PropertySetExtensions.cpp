#include "pch.h"
#include "PropertySetExtensions.h"

using namespace Platform;
using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;

using namespace WindowsRuntimeExtensions;

void PropertySetExtensions::AddSize(PropertySet^ propertySet, String^ key, Size value)
{
    propertySet->Insert(key, PropertyValue::CreateSize(value));
}

void PropertySetExtensions::AddSingle(PropertySet^ propertySet, String^ key, float value)
{
    propertySet->Insert(key, PropertyValue::CreateSingle(value));
}
