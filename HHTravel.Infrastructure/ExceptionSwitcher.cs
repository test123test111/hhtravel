using System;
//using HHTravel.Base.Common.Framework.Logging;

namespace HHTravel.Infrastructure
{
    /// <summary>
    /// 异常开关
    /// </summary>
    public class ExceptionSwitcher<T> where T : Exception
    {
        /// <summary>
        /// 如果throwOnError 为true，才抛出异常
        /// </summary>
        public static Action<bool, T> TryThrow = (throwOnError, exception) =>
        {
            if (throwOnError)
            {
                throw exception;
            }
            else
            {
                //SysLog.WriteException("预设忽略的异常", exception);
            }
        };
    }
}