# Welcome to the Dotlogix Core library!

The repo is currently in beta status so the API may change in the future. It is not recommended to use these libraries in production code but all major releases should be stable.

You can find all of the projects also on NuGet: https://www.nuget.org/profiles/dotlogix


## Current Roadmap:

Release Version 2 of the API.

#### All Projects:
- Add Documentation
- Fix style guide, ReSharper and "dotnet.settings" to help contributors to keep the style consistent

#### DotLogix.Core:
- Rework some of the concepts in the core library
- Add more awesome extension methods and classes

#### DotLogix.Architecture Pack:
- Create some Boilerplate generator or example projects to get the others started
- Add documentation about behaviour and how things should be used
- Add documentation about creating own repository handlers
- Add mocking framework (should be easy because everything is pure Linq and abstracted)
- Rework the decorator pattern to be more easy to implement and more flexible
- Add some common decorators, like prefiltering queries, auto assign values on store/add (like user-specific entities where queries should only contain user-data)
- Add attributes for entity decorators to express order and behaviour of decorators and in a more the expressive and easy way.
- Add some common decorators, like prefiltering queries, auto assign values on store/add (like user-specific entities where queries should only contain user-data)
- Add or replace methods with virtual methods to be able to override their behaviour
- Allow the users to auto apply decorators for entities based on a condition

- Optional: Allow custom methods to be used with the framework (stored procedure mapping)
- Optional: Also it's against the principle of single entity repositories, maybe a crud-base-repository for some of the option types (IDuration, IGuid, IInsertOnly, ...) would be useful.
- Optional: Reduce overhead of decorators somehow
- Optional: Add second-level caching for where queries written in C# Expressions (equality will be difficult)

#### DotLogix.Rest:
- Adopt Server to be compatible with OWIN, Katana and similar projects
- Add more intelligent mapping of results and also provide a more convenient way to customize results
- Add Http Rest Client
- Add Auto Mapping and Auto-Generation of JS and C# API Clients by using the attributes

#### DotLogix.Nodes:
- Add more converter factories and support for custom converters and attributes
- Test the async API against more complex data structures and other libraries
