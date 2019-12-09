# Telegram Bot Framework for .NET Core

 [![NuGet](https://img.shields.io/nuget/v/IBWT.Framework.svg?style=flat-square&label=IBWT.Framework&maxAge=3600)](https://www.nuget.org/packages/IBWT.Framework)
 [![Build Status](https://img.shields.io/travis/pouladpld/IBWT.Framework.svg?style=flat-square&maxAge=3600)](https://travis-ci.org/pouladpld/IBWT.Framework)
 [![License](https://img.shields.io/github/license/pouladpld/IBWT.Framework.svg?style=flat-square&maxAge=2592000)](https://raw.githubusercontent.com/pouladpld/IBWT.Framework/master/LICENSE)

<img src="./docs/icon.png" alt="Telegram Bot Framework Logo" width=200 height=200 />

Simple framework for building Telegram bots 🤖. Ideal for running multiple chat bots inside a single ASP.NET Core app.

Publish **template** and **framework** commands:

Change version in .\template\Template.csproj
Then run
`dotnet pack .\template\Template.csproj`
`dotnet nuget push .\template\bin\Debug\IBWT.Framework.Template.1.4.0.nupkg -k <key> -s https://api.nuget.org/v3/index.json`

`dotnet build ".\src\IBWT.Framework\IBWT.Framework.csproj"`
`dotnet nuget push .\IBWT.Framework\bin\Debug\IBWT.Framework.2.2.0.nupkg -k <key> -s https://api.nuget.org/v3/index.json`


To install or update template you need to run command:
`dotnet new -i IBWT.Framework.template`

## Getting Started

This project targets .NET Core 2.2 so make sure you have Visual Studio 2017 or Visual Studio Code with [.NET Core](https://www.microsoft.com/net/download/core#/current) (v2.2 or above) installed.

Creating a bot with good architecture becomes very simple using this framework. Have a look at the [**Quick Start** wiki](./docs/wiki/quick-start/echo-bot.md) to make your fist _Echo Bot_.

There is much more you can do with your bot. See what's available at [**wikis**](./docs/wiki/README.md).

## Framework Features

- Allows you to have multiple bots running inside one app
- Able to share code(update handlers) between multiple bots
- Easy to use with webhooks(specially with Docker deployments)
- Optimized for making Telegram Games
- Simplifies many repititive tasks in developing bots

## Samples

Don't wanna read wikis? Read C# code of sample projects in [samples directory](./sample/).
