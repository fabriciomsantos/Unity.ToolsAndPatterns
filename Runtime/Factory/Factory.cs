using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Tools.GamePatterns
{
    /// <summary>
    /// Product of the factory 
    /// <para>Can't not be inheritance from MonoBehaviour or ScriptableObject</para> 
    /// </summary>
    public abstract class FactoryProduct
    {
        public abstract string Name
        {
            get;
        }
    }

    /// <summary>
    /// Factory class
    /// </summary>
    /// <typeparam name="T">Class of the product inheritance from FactoryProduct</typeparam>
    public abstract class Factory<T> where T : FactoryProduct
    {
        public static Dictionary<string, Type> productsByName;
        private static bool IsInitialized => productsByName != null;

        protected static void InitializeFactory()
        {
            if (IsInitialized)
            {
                return;
            }

            var productTypes = Assembly.GetAssembly(typeof(T)).GetTypes().Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T)));

            //Dictionary for finding these by name later(could be an enum/id instead of string)
            productsByName = new Dictionary<string, Type>();

            //Get names and put them into the dictionary
            foreach (var type in productTypes)
            {
                var tempEffect = Activator.CreateInstance(type)as T;
                productsByName.Add(tempEffect.Name, type);
            }
        }

        public static T GetProduct(string productType)
        {
            InitializeFactory();

            if (productsByName.ContainsKey(productType))
            {
                var type = productsByName[productType];
                return (T)Activator.CreateInstance(type);
            }
            return null;
        }

        public static IEnumerable<T> GetProducts()
        {
            InitializeFactory();
            List<T> products = new List<T>();
            foreach (var product in productsByName.Values)
            {
                products.Add((T)Activator.CreateInstance(product));
            }
            return products;
        }

        public static IEnumerable<string> GetProductNames()
        {
            InitializeFactory();
            return productsByName.Keys;
        }
    }
}