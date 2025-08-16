#!/bin/bash
echo "iniciando coverlet"
echo "Coverlet script executed from: ${PWD}"
dotnet test "../TrybeHotel.Test/TrybeHotel.test.csproj" --collect:"Xplat Code Coverage"