#!/usr/bin/env bash

#exit if any command fails
set -e

artifactsFolder="./artifacts"

if [ -d $artifactsFolder ]; then  
  rm -R $artifactsFolder
fi

dotnet restore

# Ideally we would use the 'dotnet test' command to test netcoreapp and net451 so restrict for now 
# but this currently doesn't work due to https://github.com/dotnet/cli/issues/3073 so restrict to netcoreapp
# dotnet test ./test/emr-api-tests -c Release -f netcoreapp1.0

# Instead, run directly with mono for the full .net version 
dotnet build ./dotnet-rest-serializer-test -c Release -f net451

mono ./dotnet-rest-serializer-test/bin/Release/net451/*/dotnet-test-xunit.exe ./dotnet-rest-serializer-test/bin/Release/net451/*/dotnet-rest-serializer-test.dll

revision=${TRAVIS_JOB_ID:=1}  
revision=$(printf "%04d" $revision) 

dotnet pack ./dotnet-rest-serializer -c Release -o ./artifacts --version-suffix=alpha-$revision  