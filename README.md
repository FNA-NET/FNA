# FNA.NET
[![Nuget](https://img.shields.io/nuget/vpre/FNA.NET)](https://www.nuget.org/packages/FNA.NET/)

FNA.NET is a opinioned fork of FNA. Its goal is to bring FNA to nuget package manager. It also contains some custom modification and extensions.

## Supported Platforms

### Android

| OS                            | Architectures         | Renderer             |
| ----------------------------- | --------------------- | -------------------- |
| Android                       | Arm64                 | OpenGL               |
| Android                       | Arm64                 | Vulkan(some devices) |

Notes:

* Android: It should run with OpenGL on most devices. It runs with Vulkan on few modern devices since it has driver issues.
* Android: Emulator is not supported as SDL3 GPU has no support for it.

### Apple

| OS                            | Architectures         | Renderer                     |
| ----------------------------- | --------------------- | ---------------------------- |
| iOS                           | Arm64                 | OpenGL, Metal                |
| macOS                         | Arm64, x64            | OpenGL, Metal                |
| tvOS                          | Arm64                 | OpenGL, Metal                |

Notes:

* iOS/tvOS: Simulator is not supported as SDL3 GPU has no support for it.

### Windows

| OS                            | Architectures         | Renderer                     |
| ----------------------------- | --------------------- | ---------------------------- |
| Windows                       | x64                   | OpenGL, D3D11, Vulkan        |
| UWP(XBox)                     | x64                   | D3D11                        |

Notes:

* There's a seperate package `FNA.NET.UWP` for UWP.
* UWP is supported through SDL2 and old version fna binaries only! SDL3 has dropped the support of UWP platform.

### Linux

| OS                            | Architectures         | Renderer                     |
| ----------------------------- | --------------------- | ---------------------------- |
| Ubuntu                        | x64, arm64            | OpenGL, Vulkan               |

Notes:

* Linux: Other Linux OS is not tested but it should work.

## FNA Documentation

https://fna-xna.github.io/docs/
