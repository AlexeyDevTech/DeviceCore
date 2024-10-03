﻿using ANG24.Core.Devices.Base.Abstract;

namespace ANG24.Core.Devices.Base.Interfaces.Behaviors
{
    /// <summary>
    /// Стандартный интерфейс всех паттернов поведения
    /// </summary>
    public interface IDeviceBehavior
    {
        void HandleData(object data);

        void SetDevice(DeviceBase device);
    }
}
