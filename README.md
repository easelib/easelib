# Ease Library

Ease is a .Net library to ease unit testing through IoC containers and Mocking.

Ease supports both [NUnit](https://github.com/nunit) & [XUnit](https://github.com/xunit) for unit tests and uses [Moq](https://github.com/moq) for mocking. It currently supports [DryIoc](https://github.com/dadhi/DryIoc) & [Unity](https://github.com/unitycontainer) IoC containers and has extensions with basic support for [Prism.Forms](https://github.com/prismlibrary).

Our philosophy is to embrace the magic of DI and the IoC containers that we are already using in our app development to make testing easier to write and manage. 

# Build Status

[![Build status](https://ci.appveyor.com/api/projects/status/py04o4modm9xg03k/branch/master?svg=true)](https://ci.appveyor.com/project/duanenewman/ease/branch/master)

# Installation

You can install the libraries from [Nuget.org](https://www.nuget.org/profiles/EaseLibrary):

Package                                                                                     |NUnit|XUnit|DryIoc| Unity|Prism Forms
--------------------------------------------------------------------------------------------|-----|-----|------|------|-----------
[Ease.NUnit.DryIoc](https://www.nuget.org/packages/Ease.NUnit.DryIoc/)                      |3.1.2|     | 4.1.4|      |
[Ease.NUnit.DryIoc.PrismForms](https://www.nuget.org/packages/Ease.NUnit.DryIoc.PrismForms/)|3.1.2|     | 4.1.4|      |7.2.0.1422
[Ease.NUnit.Unity](https://www.nuget.org/packages/Ease.NUnit.Unity/)                        |3.1.2|     |      |5.11.7|
[Ease.NUnit.Unity.PrismForms](https://www.nuget.org/packages/Ease.NUnit.Unity.PrismForms/)  |3.1.2|     |      |5.11.7|7.2.0.1422
[Ease.XUnit.DryIoc](https://www.nuget.org/packages/Ease.XUnit.DryIoc/)                      |     |2.4.1| 4.1.4|      |
[Ease.XUnit.DryIoc.PrismForms](https://www.nuget.org/packages/Ease.XUnit.DryIoc.PrismForms/)|     |2.4.1| 4.1.4|      |7.2.0.1422
[Ease.XUnit.Unity](https://www.nuget.org/packages/Ease.XUnit.Unity/)                        |     |2.4.1|      |5.11.7|
[Ease.XUnit.Unity.PrismForms](https://www.nuget.org/packages/Ease.XUnit.Unity.PrismForms/)  |     |2.4.1|      |5.11.7|7.2.0.1422

# Getting Started

## Basic Usage (NUnit & DryIoc)

After adding the Ease package to your test library simply reference the base test class:

```csharp
public class CartServiceTest : Ease.NUnit.DryIoc.NUnitDryIocContainerTestBase
```

Create a field to delegate the setup action for each type you will want mocked:

```csharp
private Action<Mock<ICartRepository>> onICartRepositoryMockCreated;
```

In your constructor:
* Register any concrete implementations needed by your test, including the class you will be testing
* Register any mocked types using a reference to the setup Action
* Use `RegisterPerTestSetup` to register any actions that need to be ran before each test, including assigning default actions to mock setup. With NUnit you can alternatively use the `[Setup]` method attribute if desired.

```csharp
public CartServiceTest
{
    //Register concrete implementation for interface
    RegisterType<IHttpMessageHandlerFactory, MockApiHttpHandlerFactory>(); 
    //Register the type you will be testing
    RegisterType<CartService>(); 
    //Register a mock for ICartRepositoryMock using the field onICartRepositoryMockCreated for setup
    RegisterMockType(() => onICartRepositoryMockCreated); 
    //This will run before each test method is called
    RegisterPerTestSetup(() => {
		onICartRepositoryMockCreated = ICartRepositoryDefaultSetup;
    });
}
```

Create default setup methods

```csharp
private void ICartRepositoryDefaultSetup(Mock<ICartRepository> mock)
{
    mock.Setup(r => r.GetProducts())
        .Returns(Task.FromResult(
            Mapper().Map<Dtos.CartProductDto[], CartProduct[]>(
                ResolveType<TestApiData>().Cart.ToArray())));
}
```

Create a test

```csharp
[Test]
public async Task LoadProducts_CallsICartRepositoryGetProducts()
{
    //You can set onICartRepositoryMockCreated to a different action here, before resolving CartService
    var cartService = await Resolve<CartService>(); //depends on ICartRepository
    cartService.LoadProducts();
    GetMock<ICartRepository>().Verify(m => m.GetProducts(), Times.Once);
}
```

## Xamarin.Forms

There is a sample for Xamarin.Forms with NUnit & DryIoc [here](https://github.com/EaseLibrary/XamarinFormsDemos)