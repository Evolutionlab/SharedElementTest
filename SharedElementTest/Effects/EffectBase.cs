using System;
using System.Linq;
using Xamarin.Forms;

namespace SharedElementTest.Effects
{
    public abstract class EffectBase : RoutingEffect
    {
        protected EffectBase(string fullName) : base(fullName)
        {
        }

        public const string RESOLUTION_GROUP_NAME = "GamesWithGravitas.SharedElementTest";

        static readonly Func<bool> _trueFunc = () => true;

        protected static void CheckAddEffect<TElement, TEffect>(TElement element, string effectName)
            where TElement : Element
            where TEffect : Effect
        {
            CheckAddEffect<TElement, TEffect>(element, effectName, _trueFunc);
        }

        /// <summary>
        /// Add an effect, if neccessary
        /// </summary>
        protected static void CheckAddEffect<TElement, TEffect>(TElement element, string effectName, Func<bool> shouldExistFunc)
            where TElement : Element
            where TEffect : Effect
        {
            if (element == null)
            {
                return;
            }
            var shouldExist = shouldExistFunc();
            var existing = element.Effects.FirstOrDefault(x => x is TEffect);
            if (existing == null && shouldExist)
            {
                element.Effects.Add(Resolve(effectName));
            }
            else if (existing != null && !shouldExist)
            {
                element.Effects.Remove(existing);
            }
        }
    }
}

