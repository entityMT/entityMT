using System;
using System.Linq;
using System.Reflection;
using MTPConfigurations.Abstractions.Enums;
using MTPConfigurations.Abstractions.Providers;
using MTPConfigurations.Implementation.UnitTests.Fakers;

namespace MTPConfigurations.Implementation.UnitTests.Providers
{
    public sealed class AttributeJoinTypeProviderUnitTests
    {
        private readonly Type _joinProviderType;
        
        public AttributeJoinTypeProviderUnitTests()
        {
            _joinProviderType =
                Assembly.Load(Constants.ASSEMBLY_NAME)
                    .GetTypes()
                    .First(t => t.FullName!.Contains($"{Constants.BASIC_TYPE_PROVIDERS_PATH}AttributeJoinTypeProvider"));
        }
        
        [Fact(DisplayName = "Inner join type identification test")]
        public void GetJoinType_InnerJoin_Success()
        {
            // act
            var innerJoinProviderType = _joinProviderType.MakeGenericType(typeof(FakeInnerJoinEntity));
            
            var joinTypeProvider = Activator.CreateInstance(innerJoinProviderType)
                as IJoinTypeProvider<FakeInnerJoinEntity>;

            var joinType = joinTypeProvider!
                .GetJoinType(
                    typeof(FakeInnerJoinEntity)
                        .GetProperty(nameof(FakeInnerJoinEntity.Name))!);
            
            // assert
            Assert.Equal(JoinType.Inner, joinType);
        }
        
        [Fact(DisplayName = "Left join type identification test")]
        public void GetJoinType_LeftJoin_Success()
        {
            // act
            var leftJoinProviderType = _joinProviderType.MakeGenericType(typeof(FakeLeftJoinEntity));
            
            var joinTypeProvider = Activator.CreateInstance(leftJoinProviderType)
                as IJoinTypeProvider<FakeLeftJoinEntity>;

            var joinType = joinTypeProvider!
                .GetJoinType(
                    typeof(FakeLeftJoinEntity)
                        .GetProperty(nameof(FakeLeftJoinEntity.Name))!);
            
            // assert
            Assert.Equal(JoinType.Left, joinType);
        }
        
        [Fact(DisplayName = "Right join type identification test")]
        public void GetJoinType_RightJoin_Success()
        {
            // act
            var leftJoinProviderType = _joinProviderType.MakeGenericType(typeof(FakeRightJoinEntity));
            
            var joinTypeProvider = Activator.CreateInstance(leftJoinProviderType)
                as IJoinTypeProvider<FakeRightJoinEntity>;

            var joinType = joinTypeProvider!
                .GetJoinType(
                    typeof(FakeRightJoinEntity)
                        .GetProperty(nameof(FakeRightJoinEntity.Name))!);
            
            // assert
            Assert.Equal(JoinType.Right, joinType);
        }
        
        [Fact(DisplayName = "Cross join type identification test")]
        public void GetJoinType_CrossJoin_Success()
        {
            // act
            var crossJoinProviderType = _joinProviderType.MakeGenericType(typeof(FakeCrossJoinEntity));
            
            var joinTypeProvider = Activator.CreateInstance(crossJoinProviderType)
                as IJoinTypeProvider<FakeCrossJoinEntity>;

            var joinType = joinTypeProvider!
                .GetJoinType(
                    typeof(FakeCrossJoinEntity)
                        .GetProperty(nameof(FakeCrossJoinEntity.Name))!);
            
            // assert
            Assert.Equal(JoinType.Cross, joinType);
        }
    }
}