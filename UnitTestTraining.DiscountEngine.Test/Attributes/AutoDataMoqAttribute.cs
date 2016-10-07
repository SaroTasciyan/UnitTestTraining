using System;

using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.Xunit;

namespace UnitTestTraining.DiscountEngine.Test
{
    public class AutoDataMoqAttribute : AutoDataAttribute
    {
        public AutoDataMoqAttribute() : base(new Fixture().Customize(new AutoMoqCustomization())) { }

        public AutoDataMoqAttribute(Type type, params object[] parameters) : this()
        {
            object obj = Activator.CreateInstance(type, parameters);

            Fixture.Customize(obj as ICustomization);
        }
    }
}