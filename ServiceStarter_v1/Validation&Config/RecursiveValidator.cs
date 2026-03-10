using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStarter_v1
{
    public static class RecursiveValidator
    {
        public static bool TryValidateObjectRecursive(object objToValidate,
        List<ValidationResult> failedValidations,
        HashSet<object>? visitedObjs = null)
        {
            if (objToValidate == null) return true;
            if (visitedObjs == null) visitedObjs = new HashSet<object>();
            if (visitedObjs.Add(objToValidate) == false) return true;

            bool isValid = true;
            var context = new ValidationContext(objToValidate);
            isValid &= Validator.TryValidateObject(objToValidate, context, failedValidations, true);


            //Objekt ist eine Liste
            if (objToValidate is System.Collections.IEnumerable objAsList && objToValidate is not string)
            {
                foreach (var item in objAsList)
                {
                    isValid &= TryValidateObjectRecursive(item, failedValidations, visitedObjs);
                }
            }
            // Objekt ist eine Komplexe Klasse
            else if (IsComplexType(objToValidate))
            {
                var props = objToValidate.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                           .Where(p => p.GetIndexParameters().Length == 0);

                foreach (var prop in props)
                {
                    var val = prop.GetValue(objToValidate);
                    if (val != null)
                    {
                        // Hier gehen wir rekursiv tiefer
                        isValid &= TryValidateObjectRecursive(val, failedValidations, visitedObjs);
                    }
                }
                // Object ist ein einfaches Attribute
            }
            else { }
            return isValid;

        }

        private static bool IsComplexType(object obj)
        {
            Type objType = obj.GetType();
            return !objType.IsPrimitive &&
                !objType.IsEnum &&
                objType != typeof(string) &&
                objType != typeof(DateTime) &&
                objType != typeof(decimal);
        }
    }

}

