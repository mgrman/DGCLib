using System;
using System.Linq.Expressions;
using System.Reflection;

namespace DGCLib_Base
{
    public static class ReflexionUtils
    {
        public static PropertyInfo PropertyOf<TInstance, TProperty>(Expression<Func<TInstance, TProperty>> propertyGetExpression)
        {
            return ((MemberExpression)propertyGetExpression.Body).Member as PropertyInfo;
        }
    }
}